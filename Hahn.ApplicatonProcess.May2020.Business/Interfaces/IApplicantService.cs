using Hahn.ApplicatonProcess.May2020.Domain;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Business.Interfaces
{
    public interface IApplicantService
    {
        Task<BaseResponse<Applicant>> Create(Applicant applicant, CancellationToken token);

        Task<BaseResponse<Applicant>> Update(Applicant applicant, CancellationToken token);

        Task<BaseResponse<List<Applicant>>> GetAll(CancellationToken token);

        Task<BaseResponse<bool>> Delete(int applicantId, CancellationToken token);
        Task<BaseResponse<Applicant>> GetById(int id, CancellationToken token);
    }
}
