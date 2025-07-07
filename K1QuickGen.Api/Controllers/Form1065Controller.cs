using K1QuickGen.Api.Models;
using K1QuickGen.Api.Repositories;
using K1QuickGen.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Form1065Controller : ControllerBase
    {
        private readonly Form1065Repository _repo;

        public Form1065Controller(Form1065Repository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForm([FromBody] Form1065CreateDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.CompanyId))
                return BadRequest("Missing required fields.");

            var form = new Form1065
            {
                CompanyId = dto.CompanyId,
                CompanyName = dto.CompanyName,
                TaxYear = dto.TaxYear
                // Id and CreatedOn will be auto-generated
            };

            await _repo.InsertAsync(form);
            return Ok(form);
        }

        [HttpGet("{companyId}")]
        public async Task<ActionResult<List<Form1065>>> GetByCompany(string companyId)
        {
            var results = await _repo.GetByCompanyIdAsync(companyId);
            return Ok(results);
        }
    }
}
