using K1QuickGen.Api.Interfaces;
using K1QuickGen.Api.Models;
using K1QuickGen.Api.Services;
using K1QuickGen.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartnersController : ControllerBase
    {
        private readonly IPartnersSubmissionService _service;
        private readonly ILogger<PartnersController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnersController"/> class.
        /// </summary>
        /// <param name="service">The service for partner submissions.</param>
        /// <param name="logger">The logger instance for this controller.</param>
        public PartnersController(IPartnersSubmissionService service, ILogger<PartnersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Submit a new partner entry to MongoDB.
        /// </summary>
        /// <param name="dto">The partner submission data transfer object.</param>
        /// <returns>
        /// Returns the created partner submission output DTO if successful; otherwise, returns BadRequest for invalid input.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> SubmitPartner([FromBody] PartnerSubmissionCreateDto dto)
        {
            if (dto == null || dto.CompanyId == Guid.Empty || dto.Form1065Id == Guid.Empty)
                return BadRequest("Invalid partner submission.");

            var outputDto = await _service.CreateAsync(dto);
            return Ok(outputDto);
        }

        /// <summary>
        /// Retrieves all partner submissions from MongoDB.
        /// </summary>
        /// <returns>
        /// Returns a list of all partner submissions.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<List<PartnerSubmission>>> GetAllPartners()
        {
            var submissions = await _service.GetAllAsync();
            return Ok(submissions);
        }

        /// <summary>
        /// Retrieves all partners associated with a specific Form1065 ID.
        /// </summary>
        /// <param name="form1065Id">The unique identifier of the Form 1065.</param>
        /// <returns>
        /// Returns a list of partner submissions for the specified Form1065 ID.
        /// </returns>
        [HttpGet("by-form/{form1065Id}")]
        public async Task<ActionResult<List<PartnerSubmission>>> GetByForm(Guid form1065Id)
        {
            var partners = await _service.GetByForm1065IdAsync(form1065Id);
            return Ok(partners);
        }
    }
}
