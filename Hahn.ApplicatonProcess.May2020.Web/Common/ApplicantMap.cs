using AutoMapper;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using Hahn.ApplicatonProcess.May2020.Domain.UiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Web.Common
{
    public class ApplicantMap : Profile
    {
        public ApplicantMap()
        {
            CreateMap<Applicant, ApplicantUi>()
                .ForSourceMember(a => a.CreatedOn, x => x.DoNotValidate())
                .ReverseMap();
        }
    }
}
