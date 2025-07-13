using K1QuickGen.Api.Interfaces;
using K1QuickGen.Api.Messages;
using K1QuickGen.Api.Models;
using K1QuickGen.Contracts.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Services
{
    public class TaxFormMessagingService : ITaxFormMessagingService
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<TaxFormMessagingService> _logger;

        public TaxFormMessagingService(
            IRabbitMqService rabbitMqService,
            ILogger<TaxFormMessagingService> logger)
        {
            _rabbitMqService = rabbitMqService;
            _logger = logger;
        }

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