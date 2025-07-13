using K1QuickGen.Api.Interfaces;
using K1QuickGen.Api.Messages;
using K1QuickGen.PdfGeneration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Services
{
    public class TaxFormMessageProcessor : BackgroundService
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<TaxFormMessageProcessor> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TaxFormMessageProcessor(
            IRabbitMqService rabbitMqService,
            ILogger<TaxFormMessageProcessor> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _rabbitMqService = rabbitMqService;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TaxForm Message Processor starting");

            _rabbitMqService.StartConsuming(ProcessMessage);

            return Task.CompletedTask;
        }

        private void ProcessMessage(string messageJson)
        {
            try
            {
                // Try to deserialize as MessageBase to get the message type
                JsonDocument doc = JsonDocument.Parse(messageJson);
                if (!doc.RootElement.TryGetProperty("MessageType", out var messageTypeElement))
                {
                    _logger.LogWarning("Message doesn't contain MessageType property");
                    return;
                }

                string messageType = messageTypeElement.GetString();
                _logger.LogInformation("Processing message of type: {MessageType}", messageType);

                switch (messageType)
                {
                    case nameof(Form1065SubmittedMessage):
                        var form1065Message = JsonSerializer.Deserialize<Form1065SubmittedMessage>(messageJson);
                        ProcessForm1065Submitted(form1065Message);
                        break;

                    case nameof(PartnerSubmittedMessage):
                        var partnerMessage = JsonSerializer.Deserialize<PartnerSubmittedMessage>(messageJson);
                        ProcessPartnerSubmitted(partnerMessage);
                        break;

                    case nameof(GeneratePdfCommand):
                        var pdfCommand = JsonSerializer.Deserialize<GeneratePdfCommand>(messageJson);
                        ProcessGeneratePdfCommand(pdfCommand);
                        break;

                    default:
                        _logger.LogWarning("Unknown message type: {MessageType}", messageType);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message: {Message}", messageJson);
                throw; // Rethrow to trigger NACK
            }
        }

        private void ProcessForm1065Submitted(Form1065SubmittedMessage message)
        {
            if (message == null) return;

            _logger.LogInformation("Processing Form1065 submission for company {CompanyName} (ID: {FormId})",
                message.CompanyName, message.Form1065Id);

            // Example: Additional processing, validations, etc.
            // This runs in the background, after the HTTP response is sent
        }

        private void ProcessPartnerSubmitted(PartnerSubmittedMessage message)
        {
            if (message == null) return;

            _logger.LogInformation("Processing Partner submission for {PartnerName} (ID: {PartnerId})",
                message.PartnerName, message.PartnerId);

            // Example: Additional processing, notifications, etc.
        }

        private void ProcessGeneratePdfCommand(GeneratePdfCommand message)
        {
            if (message == null) return;

            _logger.LogInformation("Generating PDF for Form1065 ID: {FormId}", message.Form1065Id);

            try
            {
                // Create a scope to resolve scoped services
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var form1065Service = scope.ServiceProvider.GetRequiredService<IForm1065Service>();
                    var pdfGenerator = scope.ServiceProvider.GetRequiredService<IForm1065PdfGenerator>();

                    // Get the form with associated K1s
                    var formDto = form1065Service.GetFormWithK1sAsync(message.Form1065Id).Result;
                    if (formDto == null)
                    {
                        _logger.LogError("Form1065 not found for ID: {FormId}", message.Form1065Id);
                        return;
                    }

                    // Generate PDF
                    var pdfBytes = pdfGenerator.GeneratePdf(formDto);

                    _logger.LogInformation("Successfully generated PDF for {CompanyName}, size: {Size} bytes",
                        formDto.CompanyName, pdfBytes.Length);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate PDF for Form1065 ID: {FormId}", message.Form1065Id);
                throw; // Rethrow to trigger NACK
            }
        }
    }
}