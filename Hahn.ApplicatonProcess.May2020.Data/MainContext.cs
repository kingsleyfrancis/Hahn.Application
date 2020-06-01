using System;
using System.Collections.Generic;
using System.Text;
using Hahn.ApplicatonProcess.May2020.Data.Configurations;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hahn.ApplicatonProcess.May2020.Data
{
    public class MainContext : DbContext
    {
        private readonly IConfiguration configuration;
        public MainContext(DbContextOptions<MainContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<Applicant> Applicants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicantConfiguration).Assembly);
        }
    }
}
