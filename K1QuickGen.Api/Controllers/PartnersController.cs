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

        public PartnersController(IPartnersSubmissionService service, ILogger<PartnersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Submit a new partner entry to MongoDB.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SubmitPartner([FromBody] PartnerSubmissionCreateDto dto)
        {
            if (dto == null || dto.CompanyId == Guid.Empty || dto.Form1065Id == Guid.Empty)
                return BadRequest("Invalid partner submission.");

            var outputDto = await _service.CreateAsync(dto);
            return Ok(outputDto);
        }

        /// <summary>
        /// Get all partner submissions from MongoDB.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<PartnerSubmission>>> GetAllPartners()
        {
            var submissions = await _service.GetAllAsync();
            return Ok(submissions);
        }

        /// <summary>
        /// Get partners by Form1065 ID (Guid).
        /// </summary>
        [HttpGet("by-form/{form1065Id}")]
        public async Task<ActionResult<List<PartnerSubmission>>> GetByForm(Guid form1065Id)
        {
            var partners = await _service.GetByForm1065IdAsync(form1065Id);
            return Ok(partners);
        }
    }
}
