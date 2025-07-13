using K1QuickGen.Api.Interfaces;
using K1QuickGen.Api.Messages;
using K1QuickGen.Api.Models;
using K1QuickGen.Contracts.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Services
{
    /// <summary>
    /// Service responsible for publishing tax form-related messages to RabbitMQ.
    /// This service acts as an abstraction layer between business logic and the message broker,
    /// providing methods to publish events and commands such as form submissions, partner submissions,
    /// and PDF generation requests. It ensures that messaging concerns are separated from core business logic
    /// and centralizes all message publishing for tax form workflows.
    /// </summary>
    public class TaxFormMessagingService : ITaxFormMessagingService
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<TaxFormMessagingService> _logger;

        /// <summary>
        /// Constructor. Initializes a new instance of the <see cref="TaxFormMessagingService"/> class using Dependency Injection (DI).
        /// </summary>
        /// <param name="rabbitMqService">The RabbitMQ service for publishing messages, injected via DI.</param>
        /// <param name="logger">The logger instance for logging messaging events, injected via DI.</param>
        public TaxFormMessagingService(
            IRabbitMqService rabbitMqService,
            ILogger<TaxFormMessagingService> logger)
        {
            _rabbitMqService = rabbitMqService;
            _logger = logger;
        }

        /// <summary>
        /// Publishes a Form1065 submitted event to RabbitMQ.
        /// </summary>
        /// <param name="form">The output DTO containing details of the submitted Form 1065.</param>
        /// <returns>A completed task when the event is published.</returns>
        public Task PublishForm1065SubmittedAsync(Form1065OutputDto form)
        {
            try
            {
                var message = new Form1065SubmittedMessage
                {
                    Form1065Id = form.Form1065Id,
                    CompanyId = form.CompanyId,
                    CompanyName = form.CompanyName,
                    TaxYear = form.TaxYear
                };

                _rabbitMqService.Publish(message);
                _logger.LogInformation("Published Form1065 submission event for form {FormId}", form.Form1065Id);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish Form1065 submission event");
                // Don't rethrow - we don't want message publishing failures to affect the primary flow
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Publishes a partner submitted event to RabbitMQ.
        /// </summary>
        /// <param name="partner">The partner submission entity containing partner details.</param>
        /// <returns>A completed task when the event is published.</returns>
        public Task PublishPartnerSubmittedAsync(PartnerSubmission partner)
        {
            try
            {
                var message = new PartnerSubmittedMessage
                {
                    PartnerId = partner.Id,
                    Form1065Id = partner.Form1065Id,
                    CompanyId = partner.CompanyId,
                    PartnerName = partner.PartnerName ?? string.Empty
                };

                _rabbitMqService.Publish(message);
                _logger.LogInformation("Published partner submission event for partner {PartnerId}", partner.Id);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish partner submission event");
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Publishes a command to generate a PDF for a specific Form 1065 to RabbitMQ.
        /// </summary>
        /// <param name="form1065Id">The unique identifier of the Form 1065.</param>
        /// <param name="includeK1s">Whether to include K-1 schedules in the PDF generation.</param>
        /// <returns>A completed task when the command is published.</returns>
        public Task PublishGeneratePdfCommandAsync(Guid form1065Id, bool includeK1s = true)
        {
            try
            {
                var message = new GeneratePdfCommand
                {
                    Form1065Id = form1065Id,
                    IncludeK1s = includeK1s
                };

                _rabbitMqService.Publish(message);
                _logger.LogInformation("Published PDF generation command for form {FormId}", form1065Id);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish PDF generation command");
                return Task.CompletedTask;
            }
        }
    }
}