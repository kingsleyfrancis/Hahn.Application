using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hahn.ApplicatonProcess.May2020.Business.Interfaces;
using Hahn.ApplicatonProcess.May2020.Business.Services;
using Hahn.ApplicatonProcess.May2020.Data;
using Hahn.ApplicatonProcess.May2020.Domain.UiModels;
using Hahn.ApplicatonProcess.May2020.Web.Common;
using Hahn.ApplicatonProcess.May2020.Web.Common.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Hahn.ApplicatonProcess.May2020.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Environment.SetEnvironmentVariable("BASEDIR", AppContext.BaseDirectory);
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddControllers().AddNewtonsoftJson();


            string connectionString = Configuration.GetConnectionString("DefaultConnectionString");

            services.AddDbContext<MainContext>(
              options => options.UseSqlServer(connectionString));


            //configure mvc and fluent validation
            services.AddMvc()
                .AddFluentValidation();

            //configure swagger doc
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hahn Apis", Version = "v1" });
            });

            //register validators
            services.AddTransient<IValidator<ApplicantUi>, ApplicantValidator>();

            //add automapper registrar
            ConfigureMapping(services);

            services.AddScoped<IApplicantService, ApplicantService>();
            services.AddScoped<ICountryValidationService, CountryValidationService>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hahn's Api V1");
            });

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureMapping(IServiceCollection services)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicantMap());
               
            });

            var mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
