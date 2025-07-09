using K1QuickGen.Api.Models;
using K1QuickGen.Api.Repositories;
using K1QuickGen.Api.Services;
using K1QuickGen.Contracts.Dtos;
using K1QuickGen.PdfGeneration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Form1065Controller : ControllerBase
    {
        private readonly Form1065Service _form1065Service;
        private readonly Form1065Repository _form1065Repo;
        private readonly PartnerSubmissionRepository _partnerRepo;
        private readonly IForm1065PdfGenerator _pdfGenerator;

        public Form1065Controller(
            Form1065Service form1065Service,
            Form1065Repository form1065Repo, 
            PartnerSubmissionRepository partnerRepo,
            IForm1065PdfGenerator pdfGenerator
            )
        {
            _form1065Service = form1065Service;
            _form1065Repo = form1065Repo;
            _partnerRepo = partnerRepo;
            _pdfGenerator = pdfGenerator;
        }

        [HttpGet("{form1065Id:guid}")]
        public async Task<ActionResult<Form1065OutputDto>> GetById(Guid form1065Id)
        {
            var formDto = await _form1065Service.GetForm1065DtoByIdAsync(form1065Id);
            if (formDto == null)
                return NotFound();
            return Ok(formDto);
            //var form = await _form1065Repo.GetByIdAsync(form1065Id);
            //if (form == null)
            //    return NotFound();

            //var dto = new Form1065OutputDto
            //{
            //    Form1065Id = form.Id,
            //    CompanyName = form.CompanyName,
            //    CompanyId = form.CompanyId,
            //    Ein = form.Ein,
            //    TotalAssets = form.TotalAssets,
            //    TaxYear = form.TaxYear,
            //    BusinessActivity = form.BusinessActivity,
            //    ProductOrService = form.ProductOrService,
            //    BusinessCode = form.BusinessCode,
            //    Address = form.Address,
            //    City = form.City,
            //    State = form.State,
            //    ZipCode = form.ZipCode,
            //    Country = form.Country,
            //    DateBusinessStarted = form.DateBusinessStarted,
            //    IsFinalReturn = form.IsFinalReturn,
            //    IsAmendedReturn = form.IsAmendedReturn,
            //    PartnerK1s = new List<K1ScheduleDto>() // or fetch if needed
            //};
            //return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForm([FromBody] Form1065CreateDto dto)
        {
            if (dto == null || dto.CompanyId == Guid.Empty)
                return BadRequest("Missing required fields.");

            var form = new Form1065
            {
                CompanyName = dto.CompanyName,
                CompanyId = dto.CompanyId,
                Ein = dto.Ein,
                TotalAssets = dto.TotalAssets,
                TaxYear = dto.TaxYear,
                BusinessActivity = dto.BusinessActivity,
                ProductOrService = dto.ProductOrService,
                BusinessCode = dto.BusinessCode,
                Address = dto.Address,
                City = dto.City,
                State = dto.State,
                ZipCode = dto.ZipCode,
                Country = dto.Country,
                DateBusinessStarted = dto.DateBusinessStarted,
                IsFinalReturn = dto.IsFinalReturn,
                IsAmendedReturn = dto.IsAmendedReturn
            };

            await _form1065Repo.InsertAsync(form);

            var returnDto = new Form1065OutputDto
            {
                Form1065Id = form.Id,
                CompanyName = form.CompanyName,
                CompanyId = form.CompanyId,
                Ein = form.Ein,
                TotalAssets = form.TotalAssets,
                TaxYear = form.TaxYear,
                BusinessActivity = form.BusinessActivity,
                ProductOrService = form.ProductOrService,
                BusinessCode = form.BusinessCode,
                Address = form.Address,
                City = form.City,
                State = form.State,
                ZipCode = form.ZipCode,
                Country = form.Country,
                DateBusinessStarted = form.DateBusinessStarted,
                IsFinalReturn = form.IsFinalReturn,
                IsAmendedReturn = form.IsAmendedReturn,
                PartnerK1s = new List<K1ScheduleDto>()
            };

            return Ok(returnDto);
        }

        [HttpGet("{companyId}")]
        public async Task<ActionResult<List<Form1065>>> GetByCompany(Guid companyId)
        {
            var results = await _form1065Repo.GetByCompanyIdAsync(companyId);
            return Ok(results);
        }

        [HttpGet("generate/{form1065Id}")]
        public async Task<IActionResult> GenerateFormAndK1s(Guid form1065Id)
        {
            var form = await _form1065Repo.GetByIdAsync(form1065Id);
            if (form == null) return NotFound("Form1065 not found");

            var partners = await _partnerRepo.GetByForm1065IdAsync(form1065Id);

            var result = new Form1065OutputDto
            {
                Form1065Id = form.Id,
                CompanyName = form.CompanyName,
                TaxYear = form.TaxYear,
                PartnerK1s = partners.Select(p => new K1ScheduleDto
                {
                    PartnerName = p.PartnerName,
                    PartnerType = p.PartnerType,
                    OwnershipPercentage = p.OwnershipPercentage,
                    CapitalContribution = p.CapitalContribution,
                    Email = p.Email
                }).ToList()
            };

            return Ok(result);
        }

        [HttpGet("generate-pdf/{form1065Id}")]
        public async Task<IActionResult> GeneratePdf(Guid form1065Id)
        {
            var form = await _form1065Repo.GetByIdAsync(form1065Id);
            if (form == null) return NotFound();

            var partners = await _partnerRepo.GetByForm1065IdAsync(form1065Id);

            var dto = await _form1065Service.GetForm1065DtoByIdAsync(form1065Id);

            var pdfBytes = _pdfGenerator.GeneratePdf(dto);
            return File(pdfBytes, "application/pdf", $"Form1065_{form.CompanyName}.pdf");
        }
    }
}
