using System.Text.Json;

using Azure.Messaging.ServiceBus;

using AzureTranslation.Core.Interfaces;
using AzureTranslation.Infrastructure.Options;

using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Options;

namespace AzureTranslation.Infrastructure.Services;

/// <summary>
/// Implementation of message bus service using Azure Service Bus.
/// This class provides functionality to send translation messages to a Service Bus queue.
/// </summary>
internal sealed class AzureServiceBusService : IMessageBusService, IAsyncDisposable
{
    private readonly ServiceBusSender serviceBusSender;
    private readonly ILogger<AzureServiceBusService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureServiceBusService"/> class.
    /// </summary>
    /// <param name="serviceBusClient">The Service Bus client for accessing Azure Service Bus.</param>
    /// <param name="options">Configuration options for Service Bus settings.</param>
    /// <param name="logger">The logger for logging diagnostic information.</param>
    public AzureServiceBusService(ServiceBusClient serviceBusClient, IOptions<ServiceBusOptions> options, ILogger<AzureServiceBusService> logger)
    {
        serviceBusSender = serviceBusClient!.CreateSender(options.Value.QueueName);
        this.logger = logger;
    }

    /// <summary>
    /// Sends a translation message to the Service Bus queue.
    /// </summary>
    /// <param name="translationId">The unique identifier of the translation to process.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="Exception">Thrown when message sending fails.</exception>
    public async Task SendTranslationMessageAsync(string translationId, CancellationToken cancellationToken)
    {
        try
        {
            var payload = new { translationId };
            var jsonBody = JsonSerializer.Serialize(payload);

            var message = new ServiceBusMessage(jsonBody)
            {
                ContentType = "application/json",
                MessageId = translationId,
                CorrelationId = translationId,
            };

            await serviceBusSender.SendMessageAsync(message, cancellationToken);

            logger.LogInformation("Message for translation ID {TranslationId} sent to Service Bus", translationId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending message to Service Bus for translation ID {TranslationId}", translationId);
            throw;
        }
    }

    /// <summary>
    /// Asynchronously releases the unmanaged resources used by the <see cref="AzureServiceBusService"/>.
    /// </summary>
    /// <returns>A value task representing the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        if (serviceBusSender != null)
        {
            await serviceBusSender.DisposeAsync();
        }
    }
}
