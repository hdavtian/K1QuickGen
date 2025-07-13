using K1QuickGen.Api.Interfaces;
using K1QuickGen.Api.Models;
using K1QuickGen.Api.Repositories;
using K1QuickGen.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Services
{
    /// <summary>
    /// Service for handling business logic related to partner submissions for Form 1065.
    /// This service abstracts the creation and retrieval of partner submission records,
    /// coordinates with the repository for data persistence, and publishes partner-related
    /// events to the message broker for asynchronous processing or integration with other systems.
    /// </summary>
    public class PartnersSubmissionService : IPartnersSubmissionService
    {
        private readonly PartnerSubmissionRepository _repository;
        private readonly ITaxFormMessagingService _messagingService;

        /// <summary>
        /// Constructor. Initializes a new instance of the <see cref="PartnersSubmissionService"/> class using Dependency Injection (DI).
        /// </summary>
        /// <param name="repository">Repository for partner submissions, injected via DI.</param>
        /// <param name="messagingService">Service for publishing partner-related messages, injected via DI.</param>
        public PartnersSubmissionService(PartnerSubmissionRepository repository, ITaxFormMessagingService messagingService)
        {
            _repository = repository;
            _messagingService = messagingService;
        }

        /// <summary>
        /// Creates a new partner submission, saves it to the database, and publishes a partner submission event.
        /// </summary>
        /// <param name="dto">The data transfer object containing all required partner information.</param>
        /// <returns>
        /// Returns a <see cref="PartnerSubmissionOutputDto"/> representing the created partner submission.
        /// </returns>
        public async Task<PartnerSubmissionOutputDto> CreateAsync(PartnerSubmissionCreateDto dto)
        {
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
                // Add other fields as needed
            };

            // Publish the partner submission event
            await _messagingService.PublishPartnerSubmittedAsync(submission);

            return outputDto;
        }

        /// <summary>
        /// Retrieves all partner submissions from the database.
        /// </summary>
        /// <returns>
        /// Returns a list of all <see cref="PartnerSubmission"/> records.
        /// </returns>
        public async Task<List<PartnerSubmission>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Retrieves all partner submissions associated with a specific Form 1065.
        /// </summary>
        /// <param name="form1065Id">The unique identifier of the Form 1065.</param>
        /// <returns>
        /// Returns a list of <see cref="PartnerSubmission"/> records for the specified Form 1065.
        /// </returns>
        public async Task<List<PartnerSubmission>> GetByForm1065IdAsync(Guid form1065Id)
        {
            return await _repository.GetByForm1065IdAsync(form1065Id);
        }
    }
}
