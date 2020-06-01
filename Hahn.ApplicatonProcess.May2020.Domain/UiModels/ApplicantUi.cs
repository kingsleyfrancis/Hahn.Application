using System;
using System.Collections.Generic;
using System.Text;

namespace Hahn.ApplicatonProcess.May2020.Domain.UiModels
{
    public class ApplicantUi
    {
        /// <summary>
        /// The applicant's id
        /// </summary>
        /// <example>1</example>
        public int ID { get; set; }

        /// <summary>
        /// The applicant's first name
        /// </summary>
        /// <example>Kingsley</example>
        public string Name { get; set; }

        /// <summary>
        /// The applicant's last name
        /// </summary>
        /// <example>Ogbonnah</example>
        public string FamilyName { get; set; }

        /// <summary>
        /// The applicant's home address
        /// </summary>
        /// <example>12 Lovely Home Street</example>
        public string Address { get; set; }

        /// <summary>
        /// The applicant's country of origin
        /// </summary>
        /// <example>Nigeria</example>
        public string CountryOfOrigin { get; set; }

        /// <summary>
        /// The applicant's email address
        /// </summary>
        /// <example>test@gmail.com</example>
        public string EmailAddress { get; set; }

        /// <summary>
        /// The applicant's age
        /// </summary>
        /// <example>25</example>
        public int Age { get; set; }

        /// <summary>
        /// Determines where the applicant is hired or not
        /// </summary>
        /// <example>True</example>
        public bool Hired { get; set; }
    }
}
