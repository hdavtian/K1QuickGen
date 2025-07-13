using K1QuickGen.Api.Interfaces;
using K1QuickGen.Api.Models;
using K1QuickGen.Api.Repositories;
using K1QuickGen.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Services
{
    /// <summary>
    /// Service for handling business logic related to Form 1065 (U.S. Return of Partnership Income).
    /// This service abstracts operations such as creating, retrieving, and aggregating Form 1065 data,
    /// as well as coordinating related partner submissions and publishing relevant events to the message broker.
    /// It acts as the main orchestrator between the API layer, repositories, and messaging infrastructure.
    /// </summary>
    public class Form1065Service : IForm1065Service
    {
        private readonly Form1065Repository _form1065Repo;
        private readonly PartnerSubmissionRepository _partnerRepo;
        private readonly ITaxFormMessagingService _messagingService;

        /// <summary>
        /// Constructor. Initializes a new instance of the <see cref="Form1065Service"/> class using Dependency Injection (DI).
        /// </summary>
        /// <param name="form1065Repo">Repository for Form 1065 data, injected via DI.</param>
        /// <param name="partnerRepo">Repository for partner submissions, injected via DI.</param>
        /// <param name="messagingService">Service for publishing tax form-related messages, injected via DI.</param>
        public Form1065Service(
            Form1065Repository form1065Repo, 
            PartnerSubmissionRepository partnerRepo,
            ITaxFormMessagingService messagingService)
        {
            _form1065Repo = form1065Repo;
            _partnerRepo = partnerRepo;
            _messagingService = messagingService;
        }

        /// <summary>
        /// Retrieves a Form 1065 and its associated partner K-1 schedules by the form's unique identifier.
        /// </summary>
        /// <param name="form1065Id">The unique identifier of the Form 1065.</param>
        /// <returns>
        /// Returns a <see cref="Form1065OutputDto"/> with all form and partner details, or null if not found.
        /// </returns>
        public async Task<Form1065OutputDto> GetForm1065DtoByIdAsync(Guid form1065Id)
        {
            var form = await _form1065Repo.GetByIdAsync(form1065Id);
            if (form == null) return null;

            var partners = await _partnerRepo.GetByForm1065IdAsync(form1065Id);

            return new Form1065OutputDto
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
                PartnerK1s = partners.Select(p => new K1ScheduleDto
                {
                    PartnerName = p.PartnerName,
                    PartnerType = p.PartnerType,
                    OwnershipPercentage = p.OwnershipPercentage,
                    CapitalContribution = p.CapitalContribution,
                    Email = p.Email
                }).ToList()
            };
        }

        /// <summary>
        /// Creates a new Form 1065 record in the database and publishes related events to the message broker.
        /// If AutoGenerateK1s is enabled, also triggers a background PDF generation command.
        /// </summary>
        /// <param name="dto">The data transfer object containing all required Form 1065 information.</param>
        /// <returns>
        /// Returns a <see cref="Form1065OutputDto"/> representing the created form.
        /// </returns>
        public async Task<Form1065OutputDto> CreateFormAsync(Form1065CreateDto dto)
        {
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

            var outputDto = new Form1065OutputDto
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

            // Publish the form submission event
            await _messagingService.PublishForm1065SubmittedAsync(outputDto);

            // If auto-generate is enabled, also publish a PDF generation command
            if (dto.AutoGenerateK1s)
            {
                await _messagingService.PublishGeneratePdfCommandAsync(form.Id);
            }

            return outputDto;
        }

        /// <summary>
        /// Retrieves all Form 1065 submissions for a specific company.
        /// </summary>
        /// <param name="companyId">The unique identifier of the company.</param>
        /// <returns>
        /// Returns a list of <see cref="Form1065OutputDto"/> objects for the specified company.
        /// </returns>
        public async Task<List<Form1065OutputDto>> GetFormsByCompanyIdAsync(Guid companyId)
        {
            var forms = await _form1065Repo.GetByCompanyIdAsync(companyId);
            return forms.Select(form => new Form1065OutputDto
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
            }).ToList();
        }

        /// <summary>
        /// Retrieves a Form 1065 and its associated K-1 schedules by the form's unique identifier.
        /// </summary>
        /// <param name="form1065Id">The unique identifier of the Form 1065.</param>
        /// <returns>
        /// Returns a <see cref="Form1065OutputDto"/> with all form and partner details, or null if not found.
        /// </returns>
        public async Task<Form1065OutputDto> GetFormWithK1sAsync(Guid form1065Id)
        {
            var form = await _form1065Repo.GetByIdAsync(form1065Id);
            if (form == null) return null;

            var partners = await _partnerRepo.GetByForm1065IdAsync(form1065Id);

            return new Form1065OutputDto
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
                PartnerK1s = partners.Select(p => new K1ScheduleDto
                {
                    PartnerName = p.PartnerName,
                    PartnerType = p.PartnerType,
                    OwnershipPercentage = p.OwnershipPercentage,
                    CapitalContribution = p.CapitalContribution,
                    Email = p.Email
                }).ToList()
            };
        }
    }
}
