using K1QuickGen.Contracts.Dtos;
using K1QuickGen.Domain.Models;
using K1QuickGen.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReturnsController : ControllerBase
    {
        private readonly ILogger<ReturnsController> _logger;
        private readonly ReturnRepository _repo;

        public ReturnsController(ReturnRepository repo, ILogger<ReturnsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PartnershipReturnDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var model = new PartnershipReturn
            {
                CompanyId = dto.CompanyId,
                PartnershipName = dto.PartnershipName,
                EIN = dto.EIN,
                TaxYear = dto.TaxYear
            };

            await _repo.InsertAsync(model);
            _logger.LogInformation("Created partnership return for {Name} ({EIN})", model.PartnershipName, model.EIN);

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _repo.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("by-company/{companyId}")]
        public async Task<IActionResult> GetByCompany(Guid companyId)
        {
            var results = await _repo.GetByCompanyIdAsync(companyId);
            return Ok(results);
        }
    }
}
