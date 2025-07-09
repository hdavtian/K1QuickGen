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
    public class PartnersController : ControllerBase
    {
        private readonly PartnerSubmissionRepository _repository;

        public PartnersController(PartnerSubmissionRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Submit a new partner entry to MongoDB.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SubmitPartner([FromBody] PartnerSubmissionCreateDto dto)
        {
            if (dto == null || dto.CompanyId == Guid.Empty || dto.Form1065Id == Guid.Empty)
                return BadRequest("Invalid partner submission.");

            var submission = new PartnerSubmission
            {
                Form1065Id = dto.Form1065Id,
                CompanyId = dto.CompanyId,
                PartnerName = dto.PartnerName,
                PartnerType = dto.PartnerType,
                OwnershipPercentage = dto.OwnershipPercentage,
                CapitalContribution = dto.CapitalContribution,
                Email = dto.Email,
                SubmittedOn = DateTime.UtcNow,
                SSNOrEIN = dto.SSNOrEIN,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,
                Country = dto.Country,
                IsForeignPartner = dto.IsForeignPartner,
                IsTaxExemptEntity = dto.IsTaxExemptEntity,
                BeginningCapitalAccount = dto.BeginningCapitalAccount,
                EndingCapitalAccount = dto.EndingCapitalAccount,
                ShareOfIncome = dto.ShareOfIncome,
                ShareOfDeductions = dto.ShareOfDeductions
            };

            await _repository.InsertAsync(submission);

            var outputDto = new PartnerSubmissionOutputDto
            {
                Id = submission.Id,
                Form1065Id = submission.Form1065Id,
                CompanyId = submission.CompanyId,
                PartnerName = submission.PartnerName,
                PartnerType = submission.PartnerType,
                OwnershipPercentage = submission.OwnershipPercentage,
                CapitalContribution = submission.CapitalContribution,
                Email = submission.Email,
            };

            return Ok(outputDto);
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

        /// <summary>
        /// Get partners by Form1065 ID (Guid).
        /// </summary>
        [HttpGet("by-form/{form1065Id}")]
        public async Task<ActionResult<List<PartnerSubmission>>> GetByForm(Guid form1065Id)
        {
            var partners = await _repository.GetByForm1065IdAsync(form1065Id);
            return Ok(partners);
        }
    }
}
