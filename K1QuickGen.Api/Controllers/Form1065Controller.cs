using K1QuickGen.Api.Interfaces;
using K1QuickGen.Api.Models;
using K1QuickGen.Api.Repositories;
using K1QuickGen.Api.Services;
using K1QuickGen.Contracts.Dtos;
using K1QuickGen.PdfGeneration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IForm1065Service _form1065Service;
        private readonly Form1065Repository _form1065Repo;
        private readonly PartnerSubmissionRepository _partnerRepo;
        private readonly IForm1065PdfGenerator _pdfGenerator;
        private readonly ILogger<Form1065Controller> _logger;

        public Form1065Controller(
            IForm1065Service form1065Service,
            Form1065Repository form1065Repo, 
            PartnerSubmissionRepository partnerRepo,
            IForm1065PdfGenerator pdfGenerator,
            ILogger<Form1065Controller> logger
            )
        {
            _form1065Service = form1065Service;
            _form1065Repo = form1065Repo;
            _partnerRepo = partnerRepo;
            _pdfGenerator = pdfGenerator;
        }

        /// <summary>
        /// API Controller for managing IRS Form 1065 submissions and related operations.
        /// 
        /// Responsibilities:
        /// - Handles creation, retrieval, and listing of Form 1065 records for partnerships.
        /// - Supports generating and downloading Form 1065 PDFs, including associated K-1 schedules.
        /// - Integrates with services and repositories for business logic, data persistence, and PDF generation.
        /// - Validates incoming data and provides structured responses for client applications.
        /// 
        /// Endpoints:
        /// - Submit a new Form 1065 (POST)
        /// - Retrieve a Form 1065 by ID (GET)
        /// - List all Form 1065 submissions for a company (GET)
        /// - Retrieve a Form 1065 with K-1 schedules (GET)
        /// - Generate and download a Form 1065 PDF (GET)
        /// 
        /// This controller is central to the workflow for partnership tax form automation and document generation.
        /// </summary>
        /// <param name="form1065Id">The unique identifier of the Form 1065.</param>
        /// <returns>
        /// Returns the Form1065OutputDto if found; otherwise, returns 404 Not Found.
        /// </returns>
        [HttpGet("{form1065Id:guid}")]
        public async Task<ActionResult<Form1065OutputDto>> GetById(Guid form1065Id)
        {
            var formDto = await _form1065Service.GetForm1065DtoByIdAsync(form1065Id);
            if (formDto == null)
                return NotFound();
            return Ok(formDto);
        }

        /// <summary>
        /// Creates a new Form 1065 (U.S. Return of Partnership Income) submission.
        /// This endpoint processes the complete form data, saves it to the database,
        /// and publishes a notification event to the message broker for background processing.
        /// </summary>
        /// <param name="dto">The Form 1065 creation data containing all required partnership information</param>
        /// <returns>
        /// Returns a Form1065OutputDto containing the created form's details including the generated Form1065Id.
        /// Returns BadRequest if required fields are missing or invalid.
        /// </returns>
        /// <remarks>
        /// This method follows the Command-Query Responsibility Segregation (CQRS) pattern:
        /// 1. Validates the incoming request data
        /// 2. Delegates business logic to the Form1065Service
        /// 3. The service handles database persistence and message publishing
        /// 4. Returns the created form details for immediate client use
        /// 
        /// If AutoGenerateK1s is enabled in the request, a background PDF generation
        /// command will be automatically triggered via the messaging system.
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> SubmitForm([FromBody] Form1065CreateDto dto)
        {
            if (dto == null || dto.CompanyId == Guid.Empty)
                return BadRequest("Missing required fields.");

            var returnDto = await _form1065Service.CreateFormAsync(dto);
            return Ok(returnDto);
        }

        /// <summary>
        /// Retrieves all Form 1065 submissions for a specific company.
        /// </summary>
        /// <param name="companyId">The unique identifier of the company.</param>
        /// <returns>
        /// Returns a list of Form1065OutputDto objects for the specified company.
        /// </returns>
        [HttpGet("{companyId}")]
        public async Task<ActionResult<List<Form1065OutputDto>>> GetByCompany(Guid companyId)
        {
            var results = await _form1065Service.GetFormsByCompanyIdAsync(companyId);
            return Ok(results);
        }

        /// <summary>
        /// Retrieves a Form 1065 submission along with its associated K-1 schedules.
        /// </summary>
        /// <param name="form1065Id">The unique identifier of the Form 1065.</param>
        /// <returns>
        /// Returns the Form1065OutputDto with K-1 schedules if found; otherwise, returns 404 Not Found.
        /// </returns>
        [HttpGet("generate/{form1065Id}")]
        public async Task<IActionResult> GenerateFormAndK1s(Guid form1065Id)
        {
            var result = await _form1065Service.GetFormWithK1sAsync(form1065Id);
            if (result == null) return NotFound("Form1065 not found");
            return Ok(result);
        }

        /// <summary>
        /// Generates a PDF for a specific Form 1065 submission and returns it as a file download.
        /// </summary>
        /// <param name="form1065Id">The unique identifier of the Form 1065.</param>
        /// <returns>
        /// Returns a PDF file if the form is found; otherwise, returns 404 Not Found.
        /// </returns>
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
