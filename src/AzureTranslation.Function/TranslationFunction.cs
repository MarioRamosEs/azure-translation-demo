using Azure.Messaging.ServiceBus;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureTranslation.Function;

public class TranslationFunction
{
    private readonly ILogger<TranslationFunction> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslationFunction"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public TranslationFunction(ILogger<TranslationFunction> logger)
    {
        this.logger = logger;
    }

    [Function(nameof(TranslationFunction))]
    public async Task Run(
        [ServiceBusTrigger("sbq-translation-requests-dev", Connection = "ServiceBus")] // TODO: Set queue name by configuration
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
