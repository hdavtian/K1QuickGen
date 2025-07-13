using K1QuickGen.Api.Interfaces;
using K1QuickGen.Api.Models;
using K1QuickGen.Api.Repositories;
using K1QuickGen.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Services
{
    public class PartnersSubmissionService : IPartnersSubmissionService
    {
        private readonly PartnerSubmissionRepository _repository;
        private readonly ITaxFormMessagingService _messagingService;

        public PartnersSubmissionService(PartnerSubmissionRepository repository, ITaxFormMessagingService messagingService)
        {
            _repository = repository;
            _messagingService = messagingService;
        }

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

        public async Task<List<PartnerSubmission>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<PartnerSubmission>> GetByForm1065IdAsync(Guid form1065Id)
        {
            return await _repository.GetByForm1065IdAsync(form1065Id);
        }
    }
}
