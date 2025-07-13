using K1QuickGen.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace K1QuickGen.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<HealthCheckController> _logger;

        public HealthCheckController(
            IRabbitMqService rabbitMqService,
            ILogger<HealthCheckController> logger)
        {
            _rabbitMqService = rabbitMqService;
            _logger = logger;
        }

        [HttpGet("rabbitmq")]
        public IActionResult CheckRabbitMq()
        {
            try
            {
                // Send a test message to verify connection
                _rabbitMqService.Publish(new { Type = "HealthCheck", Message = "Testing RabbitMQ connection", Timestamp = DateTime.UtcNow });

                return Ok(new { Status = "Connected", Message = "RabbitMQ connection test successful" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error testing RabbitMQ connection");
                return StatusCode(500, new { Status = "Error", Message = ex.Message });
            }
        }
    }
}