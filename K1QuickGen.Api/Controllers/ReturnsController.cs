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

        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnsController"/> class.
        /// </summary>
        /// <param name="repo">The repository for partnership returns.</param>
        /// <param name="logger">The logger instance for this controller.</param>
        public ReturnsController(ReturnRepository repo, ILogger<ReturnsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new partnership return and saves it to the database.
        /// </summary>
        /// <param name="dto">The data transfer object containing partnership return details.</param>
        /// <returns>
        /// Returns a 201 Created response with the created partnership return if successful;
        /// otherwise, returns a 400 Bad Request if the input is invalid.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PartnershipReturnCreateDto dto)
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

        /// <summary>
        /// Retrieves a partnership return by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the partnership return.</param>
        /// <returns>
        /// Returns the partnership return if found; otherwise, returns 404 Not Found.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _repo.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Retrieves all partnership returns associated with a specific company.
        /// </summary>
        /// <param name="companyId">The unique identifier of the company.</param>
        /// <returns>
        /// Returns a list of partnership returns for the specified company.
        /// </returns>
        [HttpGet("by-company/{companyId}")]
        public async Task<IActionResult> GetByCompany(Guid companyId)
        {
            var results = await _repo.GetByCompanyIdAsync(companyId);
            return Ok(results);
        }
    }
}
