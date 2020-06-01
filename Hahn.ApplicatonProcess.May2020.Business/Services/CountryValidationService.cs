using Hahn.ApplicatonProcess.May2020.Business.Interfaces;
using Hahn.ApplicatonProcess.May2020.Domain.UiModels;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Business.Services
{
    public class CountryValidationService : ICountryValidationService
    {
        public Task<bool> Validate(string country, CancellationToken token)
        {
            var task = Task.Run(async () => {

                try
                {
                    if (string.IsNullOrWhiteSpace(country))
                        return false;

                    country = country.Trim().ToLower();

                    var url = $"https://restcountries.eu/rest/v2/name/{country}?fullText=true";

                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                        var response = await client.GetAsync(url);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var countries = JsonConvert.DeserializeObject<List<Country>>(content);
                            return countries.Count > 0;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch(Exception e)
                {
                    return false;
                }              

            }, token);
            return task;
        }
    }
}
