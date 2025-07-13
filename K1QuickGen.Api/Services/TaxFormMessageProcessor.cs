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
    /// <summary>
    /// Background service responsible for processing messages from RabbitMQ related to Form 1065 operations.
    /// This service listens for published events and commands (such as form submissions, partner submissions,
    /// and PDF generation requests), deserializes them, and triggers the appropriate business logic or background tasks.
    /// 
    /// The processor uses dependency injection (DI) to resolve scoped services for each message, ensuring
    /// proper resource management and separation of concerns. It is a key component in enabling asynchronous,
    /// event-driven workflows within the application.
    /// </summary>
    public class TaxFormMessageProcessor : BackgroundService
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly ILogger<TaxFormMessageProcessor> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Constructor. Initializes a new instance of the <see cref="TaxFormMessageProcessor"/> class using Dependency Injection (DI).
        /// </summary>
        /// <param name="rabbitMqService">The RabbitMQ service for message consumption, injected via DI.</param>
        /// <param name="logger">The logger instance for logging processing events, injected via DI.</param>
        /// <param name="serviceScopeFactory">Factory for creating service scopes to resolve scoped dependencies, injected via DI.</param>
        public TaxFormMessageProcessor(
            IRabbitMqService rabbitMqService,
            ILogger<TaxFormMessageProcessor> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _rabbitMqService = rabbitMqService;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Starts the background service and begins consuming messages from RabbitMQ.
        /// </summary>
        /// <param name="stoppingToken">A cancellation token that indicates when to stop the service.</param>
        /// <returns>A completed task when the service is started.</returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("TaxForm Message Processor starting");

            _rabbitMqService.StartConsuming(ProcessMessage);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Processes a single message received from RabbitMQ by deserializing it and dispatching
        /// to the appropriate handler based on the message type.
        /// </summary>
        /// <param name="messageJson">The raw JSON string of the received message.</param>
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

                // The switch statement below dispatches the message to the correct handler
                // based on the type of message received from RabbitMQ.
                // Each case corresponds to a specific event or command in the system.
                switch (messageType)
                {
                    case nameof(Form1065SubmittedMessage):
                        // Handles events when a new Form 1065 has been submitted.
                        // Triggers any background logic related to form submission.
                        var form1065Message = JsonSerializer.Deserialize<Form1065SubmittedMessage>(messageJson);
                        ProcessForm1065Submitted(form1065Message);
                        break;

                    case nameof(PartnerSubmittedMessage):
                        // Handles events when a new partner has been submitted.
                        // Triggers any background logic related to partner submission.
                        var partnerMessage = JsonSerializer.Deserialize<PartnerSubmittedMessage>(messageJson);
                        ProcessPartnerSubmitted(partnerMessage);
                        break;

                    case nameof(GeneratePdfCommand):
                        // Handles commands to generate a PDF for a specific Form 1065.
                        // Triggers background PDF generation logic.
                        var pdfCommand = JsonSerializer.Deserialize<GeneratePdfCommand>(messageJson);
                        ProcessGeneratePdfCommand(pdfCommand);
                        break;

                    default:
                        // Handles unknown or unsupported message types.
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

        /// <summary>
        /// Handles processing of a Form1065SubmittedMessage event.
        /// </summary>
        /// <param name="message">The deserialized Form1065SubmittedMessage object.</param>
        private void ProcessForm1065Submitted(Form1065SubmittedMessage message)
        {
            if (message == null) return;

            _logger.LogInformation("Processing Form1065 submission for company {CompanyName} (ID: {FormId})",
                message.CompanyName, message.Form1065Id);

            // Example: Additional processing, validations, etc.
            // This runs in the background, after the HTTP response is sent
        }

        /// <summary>
        /// Handles processing of a PartnerSubmittedMessage event.
        /// </summary>
        /// <param name="message">The deserialized PartnerSubmittedMessage object.</param>
        private void ProcessPartnerSubmitted(PartnerSubmittedMessage message)
        {
            if (message == null) return;

            _logger.LogInformation("Processing Partner submission for {PartnerName} (ID: {PartnerId})",
                message.PartnerName, message.PartnerId);

            // Example: Additional processing, notifications, etc.
        }

        /// <summary>
        /// Handles processing of a GeneratePdfCommand message, including resolving scoped services
        /// and generating the PDF for the specified Form 1065.
        /// </summary>
        /// <param name="message">The deserialized GeneratePdfCommand object.</param>
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