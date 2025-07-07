using K1QuickGen.Api.Models;
using K1QuickGen.Api.Repositories;
using K1QuickGen.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartnerController : ControllerBase
    {
        private readonly PartnerSubmissionRepository _repository;

        public PartnerController(PartnerSubmissionRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Submit a new partner entry to MongoDB.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SubmitPartner([FromBody] PartnerSubmissionCreateDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.CompanyId))
                return BadRequest("Invalid partner submission.");

            var submission = new PartnerSubmission
            {
                Form1065Id = dto.Form1065Id,
                CompanyId = dto.CompanyId,
                PartnerName = dto.PartnerName,
                PartnerType = dto.PartnerType,
                SubmittedOn = DateTime.UtcNow
            };

            await _repository.InsertAsync(submission);
            return Ok(submission);
        }

        /// <summary>
        /// Get all partner submissions from MongoDB.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<PartnerSubmission>>> GetAllPartners()
        {
            var submissions = await _repository.GetAllAsync();
            return Ok(submissions);
        }

        [HttpGet("by-form/{form1065Id}")]
        public async Task<ActionResult<List<PartnerSubmission>>> GetByForm(string form1065Id)
        {
            var partners = await _repository.GetByForm1065IdAsync(form1065Id);
            return Ok(partners);
        }
    }
}
