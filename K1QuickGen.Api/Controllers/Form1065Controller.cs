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
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForm([FromBody] Form1065CreateDto dto)
        {
            if (dto == null || dto.CompanyId == Guid.Empty)
                return BadRequest("Missing required fields.");

            var returnDto = await _form1065Service.CreateFormAsync(dto);
            return Ok(returnDto);
        }

        [HttpGet("{companyId}")]
        public async Task<ActionResult<List<Form1065OutputDto>>> GetByCompany(Guid companyId)
        {
            var results = await _form1065Service.GetFormsByCompanyIdAsync(companyId);
            return Ok(results);
        }

        [HttpGet("generate/{form1065Id}")]
        public async Task<IActionResult> GenerateFormAndK1s(Guid form1065Id)
        {
            var result = await _form1065Service.GetFormWithK1sAsync(form1065Id);
            if (result == null) return NotFound("Form1065 not found");
            return Ok(result);
        }

        [HttpGet("generate-pdf/{form1065Id}")]
        public async Task<IActionResult> GeneratePdf(Guid form1065Id)
        {
            var dto = await _form1065Service.GetFormWithK1sAsync(form1065Id);
            if (dto == null) return NotFound();

            var pdfBytes = _pdfGenerator.GeneratePdf(dto);
            return File(pdfBytes, "application/pdf", $"Form1065_{dto.CompanyName}.pdf");
        }
    }
}
