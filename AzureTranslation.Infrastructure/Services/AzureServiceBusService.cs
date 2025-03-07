using Azure.Messaging.ServiceBus;

using AzureTranslation.Core.Interfaces;
using AzureTranslation.Infrastructure.Options;

using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Options;

namespace AzureTranslation.Infrastructure.Services;

internal sealed class AzureServiceBusService : IMessageBusService, IAsyncDisposable
{
    private readonly ServiceBusSender serviceBusSender;
    private readonly ILogger<AzureServiceBusService> logger;

    public AzureServiceBusService(ServiceBusClient serviceBusClient,IOptions<ServiceBusOptions> options, ILogger<AzureServiceBusService> logger)
    {
        serviceBusSender = serviceBusClient!.CreateSender(options.Value.QueueName);
        this.logger = logger;
    }

    public async Task SendTranslationMessageAsync(string translationId, CancellationToken cancellationToken)
    {
        try
        {
            var message = new ServiceBusMessage()
            {
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

    public async ValueTask DisposeAsync()
    {
        if (serviceBusSender != null)
        {
            await serviceBusSender.DisposeAsync();
        }
    }
}
