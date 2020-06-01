using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Business.Interfaces
{
    public interface ICountryValidationService
    {
        Task<bool> Validate(string country, CancellationToken token);
    }
}
