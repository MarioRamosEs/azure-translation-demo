using Azure.Messaging.ServiceBus;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureTranslation.Function;

public class TranslationProcessor
{
    private readonly ILogger<TranslationProcessor> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslationProcessor"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public TranslationProcessor(ILogger<TranslationProcessor> logger)
    {
        this.logger = logger;
    }

    [Function(nameof(TranslationProcessor))]
    public async Task Run(
        [ServiceBusTrigger("sbq-translation-requests-mr", Connection = "ServiceBus")] // TODO: Set queue name by configuration
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        logger.LogInformation("Message ID: {id}", message.MessageId);
        logger.LogInformation("Message Body: {body}", message.Body);
        logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}
