using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hahn.ApplicatonProcess.May2020.Business.Interfaces;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using Hahn.ApplicatonProcess.May2020.Domain.UiModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hahn.ApplicatonProcess.May2020.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantsController : ControllerBase
    {
        private readonly IApplicantService _applicantService;
        private readonly ICountryValidationService _countryService;
        private readonly IMapper _mapper;
        private readonly ILogger<ApplicantsController> _logger;

        public ApplicantsController(IApplicantService applicantService, 
            ICountryValidationService countryValidationService,
            IMapper mapper,
            ILogger<ApplicantsController> logger){
            _applicantService = applicantService;
            _countryService = countryValidationService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new applicant on the db
        /// </summary>
        /// <param name="applicantUi"></param>
        /// <param name="token">Cancellation token, passed by the framework, once cancellation is requested</param>
        /// <returns>Returns status code 201 on successful or status code 400 on failure</returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ApplicantUi applicantUi, CancellationToken token)
        {
            _logger.LogInformation("Create applicant called with {0} : {1}", nameof(applicantUi), JsonConvert.SerializeObject(applicantUi));

            if (!ModelState.IsValid)
            {
                _logger.LogError("Applicant's object not valid: {0}", JsonConvert.SerializeObject(ModelState));

                return BadRequest(ModelState);
            }

            //validate country
            var validationResult = await _countryService.Validate(applicantUi.CountryOfOrigin, token);

            if (!validationResult)
            {
                _logger.LogError("Country of origin is not valid");
                return BadRequest("Country of origin is not valid");
            }
                

            var applicant = _mapper.Map<ApplicantUi, Applicant>(applicantUi);
            var result = await _applicantService.Create(applicant, token);
            if (result.IsSuccessful)
            {
                _logger.LogInformation("Create applicant successful");
                applicantUi.ID = result.Result.ID;
                return Created($"/api/applicants/get/{applicant.ID}", applicantUi);
            }

            return BadRequest(result.Message);
        }

        /// <summary>
        /// Retrieves the applicant with the given id
        /// </summary>
        /// <param name="id">Applicant's id</param>
        /// <param name="token">Cancellation token, passed by the framework, once cancellation is requested</param>
        /// <returns>Applicant object on success or status code 400 on failure</returns>
        /// <example>1</example>
        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken token)
        {

            _logger.LogInformation("Get applicant called with {0} : {1}", nameof(id), id);

            if (id <= 0)
            {
                _logger.LogError("Invalid applicant id supplied");
                return BadRequest("Invalid applicant id supplied.");
            }
                

            var response = await _applicantService.GetById(id, token);
            if (!response.IsSuccessful)
            {
                _logger.LogError(response.Message);
                return BadRequest(response.Message);
            }
                

            var applicantUi = _mapper.Map <Applicant, ApplicantUi>(response.Result);

            _logger.LogInformation("Get applicant successful");
            return Ok(applicantUi);
        }

        /// <summary>
        /// Returns a list of all the applicants that have registered so far
        /// </summary>
        /// <param name="token">Cancellation token, passed by the framework, once cancellation is requested</param>
        /// <returns>Return 200 status code on success or 400 if something goes wrong</returns>
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {

            _logger.LogInformation("Get all applicants called");

            var response = await _applicantService.GetAll(token);
            if (!response.IsSuccessful)
            {
                _logger.LogInformation(response.Message);
                return Ok(new List<ApplicantUi>());
            }
                

            var applicants = _mapper.Map<List<Applicant>, List<ApplicantUi>>(response.Result);

            _logger.LogInformation("Get all applicant successful");
            return Ok(applicants);
        }

        /// <summary>
        /// Updates a given applicant already existing in the system
        /// </summary>
        /// <param name="applicantUi">The changes to update the applicant object with</param>
        /// <param name="token">Cancellation token, passed by the framework, once cancellation is requested</param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ApplicantUi applicantUi, CancellationToken token)
        {

            _logger.LogInformation("Update applicant called with {0} : {1}", nameof(applicantUi), JsonConvert.SerializeObject(applicantUi));

            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Update applicant called failed with {0} : {1}", nameof(ModelState), JsonConvert.SerializeObject(ModelState));
                return BadRequest(ModelState);
            }

            var countryValidationResult = await _countryService.Validate(applicantUi.CountryOfOrigin, token);
            if (!countryValidationResult)
            {
                _logger.LogError("Country of origin is not valid");
                return BadRequest("Country of origin is not valid");
            }
                

            if (applicantUi.ID <= 0)
            {
                _logger.LogError("Invalid applicant's id");
                return BadRequest("Invalid applicant's id");
            }
                

            var applicant = _mapper.Map<ApplicantUi, Applicant>(applicantUi);

            var result = await _applicantService.Update(applicant, token);
            if (!result.IsSuccessful)
            {
                _logger.LogError(result.Message);
                return BadRequest(result.Message);
            }

            _logger.LogInformation(result.Message);
            return Ok(applicantUi);
        }

        /// <summary>
        /// Deletes an applicant with the supplied id
        /// </summary>
        /// <param name="id">Applicant's id to delete</param>
        /// <param name="token">Cancellation token, passed by the framework, once cancellation is requested</param>
        /// <returns>Returns status code 200 on success or 400 on failure</returns>
        /// <example>1</example>
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken token)
        {
            _logger.LogInformation("Delete Applicant Called: {0}", nameof(id), id);
            if (id <= 0)
            {
                _logger.LogError("Invalid applicant's id");
                return BadRequest("Invalid applicant's id");
            }

            var deletionResult = await _applicantService.Delete(id, token);
            if (!deletionResult.IsSuccessful)
            {
                _logger.LogError(deletionResult.Message);
                return BadRequest(deletionResult.Message);
            }

            _logger.LogInformation("Log information successful");
            return Ok();
        }
    }
}