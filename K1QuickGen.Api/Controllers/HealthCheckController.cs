using K1QuickGen.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace K1QuickGen.Api.Controllers
{
    /// <summary>
    /// API Controller for system health monitoring.
    /// 
    /// Responsibilities:
    /// - Provides endpoints to check the health and connectivity of system components, specifically the RabbitMQ message broker.
    /// - Publishes test messages to RabbitMQ to verify messaging infrastructure is operational.
    /// - Logs health check events and errors for diagnostics.
    /// 
    /// Endpoints:
    /// - Check RabbitMQ connectivity (GET /api/healthcheck/rabbitmq)
    /// 
    /// This controller helps ensure reliable background processing and messaging by validating the status of RabbitMQ.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<HealthCheckController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheckController"/> class.
        /// </summary>
        /// <param name="rabbitMqService">The RabbitMQ service used for messaging health checks.</param>
        /// <param name="logger">The logger instance for logging health check events.</param>
        public HealthCheckController(
            IRabbitMqService rabbitMqService,
            ILogger<HealthCheckController> logger)
        {
            _rabbitMqService = rabbitMqService;
            _logger = logger;
        }

        /// <summary>
        /// Checks the health and connectivity of the RabbitMQ message broker.
        /// Publishes a test message to verify the connection and returns the status.
        /// </summary>
        /// <returns>
        /// 200 OK if the connection is successful; 500 Internal Server Error if the connection fails.
        /// </returns>
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