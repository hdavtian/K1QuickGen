using K1QuickGen.Api.Models;
using K1QuickGen.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Interfaces
{
    public interface IPartnersSubmissionService
    {
        Task<PartnerSubmissionOutputDto> CreateAsync(PartnerSubmissionCreateDto dto);
        Task<List<PartnerSubmission>> GetAllAsync();
        Task<List<PartnerSubmission>> GetByForm1065IdAsync(Guid form1065Id);
    }
}
