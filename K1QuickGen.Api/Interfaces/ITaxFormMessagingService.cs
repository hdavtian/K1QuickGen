using K1QuickGen.Api.Messages;
using K1QuickGen.Api.Models;
using K1QuickGen.Contracts.Dtos;
using System;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Interfaces
{
    /// <summary>
    /// Service responsible for publishing tax form-related messages to the message broker
    /// </summary>
    public interface ITaxFormMessagingService
    {
        /// <summary>
        /// Publishes a Form1065 submitted event
        /// </summary>
        Task PublishForm1065SubmittedAsync(Form1065OutputDto form);

        /// <summary>
        /// Publishes a partner submitted event
        /// </summary>
        Task PublishPartnerSubmittedAsync(PartnerSubmission partner);

        /// <summary>
        /// Publishes a command to generate PDF for a specific form
        /// </summary>
        Task PublishGeneratePdfCommandAsync(Guid form1065Id, bool includeK1s = true);
    }
}