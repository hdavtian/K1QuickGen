using K1QuickGen.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Interfaces
{
    public interface IForm1065Service
    {
        Task<Form1065OutputDto> GetForm1065DtoByIdAsync(Guid form1065Id);
        Task<Form1065OutputDto> CreateFormAsync(Form1065CreateDto dto);
        Task<List<Form1065OutputDto>> GetFormsByCompanyIdAsync(Guid companyId);
        Task<Form1065OutputDto> GetFormWithK1sAsync(Guid form1065Id);
    }
}
