using Hahn.ApplicatonProcess.May2020.Business.Interfaces;
using Hahn.ApplicatonProcess.May2020.Data;
using Hahn.ApplicatonProcess.May2020.Domain;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Business.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly MainContext _db;
        public ApplicantService(MainContext db)
        {
            _db = db;
        }
        public Task<BaseResponse<Applicant>> Create(Applicant applicant, CancellationToken token)
        {
            var task = Task.Run(async () => {

                if (applicant == null)
                    return new BaseResponse<Applicant>(false, "Applicant object is null", applicant);

                applicant.CreatedOn = DateTime.UtcNow;

                _db.Applicants.Add(applicant);
                await _db.SaveChangesAsync();

                return new BaseResponse<Applicant>(true, "Applicant created successfully", applicant);
            }, token);
            return task;
        }

        public Task<BaseResponse<bool>> Delete(int applicantId, CancellationToken token)
        {
            var task = Task.Run(async () =>
            {
                if (applicantId <= 0)
                    return new BaseResponse<bool>(false, "Invalid applicant id", false);

                var applicant = _db.Applicants.SingleOrDefault(a => a.ID == applicantId);
                if (applicant == null)
                    return new BaseResponse<bool>(false, "Applicant does not exist", false);

                _db.Applicants.Remove(applicant);
                await _db.SaveChangesAsync();

                return new BaseResponse<bool>(true, "Applicant deleted successfully", true);
            }, token);

            return task;
        }

        public Task<BaseResponse<List<Applicant>>> GetAll(CancellationToken token)
        {
            var task = Task.Run(() =>
            {
                var applicants = _db.Applicants.ToList();

                return new BaseResponse<List<Applicant>>(true, "Applicant retrieved successfully", applicants);

            },token);
            return task;
        }

        public Task<BaseResponse<Applicant>> GetById(int id, CancellationToken token)
        {
            var task = Task.Run(() =>
            {
                if (id <= 0)
                    return new BaseResponse<Applicant>(false, "Invalid applicant id", null);

                var applicant = _db.Applicants.SingleOrDefault(a => a.ID == id);

                if(applicant != null)
                {
                    return new BaseResponse<Applicant>(true, "Applicant retrieved successfully", applicant);
                }
                return new BaseResponse<Applicant>(false, "Applicant does not exist", null);
            }, token);
            return task;
        }

        public Task<BaseResponse<Applicant>> Update(Applicant applicant, CancellationToken token)
        {
            var task = Task.Run(async () =>
            {
                if (applicant == null)
                    return new BaseResponse<Applicant>(false, "Applicant not supplied.", applicant);

                if (applicant.ID == 0)
                    return new BaseResponse<Applicant>(false, "Applicant is not not supplied. Cannot update", applicant);

                var oldApplicant = _db.Applicants.SingleOrDefault(a => a.ID == applicant.ID);

                oldApplicant.Name = applicant.Name;
                oldApplicant.Hired = applicant.Hired;
                oldApplicant.FamilyName = applicant.FamilyName;
                oldApplicant.EmailAddress = applicant.EmailAddress;
                oldApplicant.CountryOfOrigin = applicant.CountryOfOrigin;
                oldApplicant.Age = applicant.Age;
                oldApplicant.Address = applicant.Address;

                _db.Applicants.Update(oldApplicant);
                await _db.SaveChangesAsync();

                return new BaseResponse<Applicant>(true, "Applicant updated successfully", oldApplicant);

            }, token);
            return task;
        }
    }
}
