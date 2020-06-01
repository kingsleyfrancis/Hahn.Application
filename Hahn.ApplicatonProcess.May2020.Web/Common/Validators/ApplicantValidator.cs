using FluentValidation;
using Hahn.ApplicatonProcess.May2020.Domain.UiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Web.Common.Validators
{
    public class ApplicantValidator : AbstractValidator<ApplicantUi>
    {
        public ApplicantValidator()
        {
            RuleFor(x => x.Address).MinimumLength(10);
            RuleFor(x => x.Age).InclusiveBetween(20, 60);
            RuleFor(a => a.CountryOfOrigin).NotEmpty();
            RuleFor(a => a.EmailAddress).EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible);
            RuleFor(x => x.FamilyName).MinimumLength(5);
            RuleFor(a => a.Name).MinimumLength(5);
            RuleFor(a => a.Hired).NotNull();
        }
    }
}
