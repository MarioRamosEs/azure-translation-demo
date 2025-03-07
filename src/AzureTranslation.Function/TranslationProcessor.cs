using Azure.Messaging.ServiceBus;

using AzureTranslation.Core.Interfaces;

using DnsClient.Internal;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureTranslation.Function;

public class TranslationProcessor
{
    private readonly ITranslationService translationService;
    private readonly ILogger<TranslationProcessor> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslationProcessor"/> class.
    /// </summary>
    /// <param name="translationService">The translation service.</param>
    /// <param name="logger">The logger.</param>
    public TranslationProcessor(ITranslationService translationService, ILogger<TranslationProcessor> logger)
    {
        this.translationService = translationService;
        this.logger = logger;
    }

    [Function(nameof(TranslationProcessor))]
    public async Task Run(
        [ServiceBusTrigger("sbq-translation-requests-mr", Connection = "ServiceBus")] // TODO: Set queue name by configuration
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing translation message with ID: {MessageId}", message.MessageId);

        try
        {
            logger.LogInformation("Start processing translation with ID: {TranslationId}", message.MessageId);

            await translationService.ProcessTranslationAsync(message.MessageId, cancellationToken);

            logger.LogInformation("Finished processing translation with ID: {TranslationId}", message.MessageId);

            await messageActions.CompleteMessageAsync(message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing translation message with ID: {MessageId}", message.MessageId);

            // Let the exception propagate to retry the message based on the Service Bus retry policy
            throw;
        }
    }
}
