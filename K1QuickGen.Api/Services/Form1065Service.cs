using K1QuickGen.Api.Repositories;
using K1QuickGen.Contracts.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Services
{
    public class Form1065Service
    {
        private readonly Form1065Repository _form1065Repo;
        private readonly PartnerSubmissionRepository _partnerRepo;

        public Form1065Service(Form1065Repository form1065Repo, PartnerSubmissionRepository partnerRepo)
        {
            _form1065Repo = form1065Repo;
            _partnerRepo = partnerRepo;
        }

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
    }
}
