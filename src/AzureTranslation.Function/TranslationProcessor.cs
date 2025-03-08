using Azure.Messaging.ServiceBus;

using AzureTranslation.Core.Interfaces;

using DnsClient.Internal;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureTranslation.Function;

/// <summary>
/// Function to process translation requests.
/// </summary>
public class TranslationProcessor
{
    private readonly ITranslationProcessor translationProcessor;
    private readonly ILogger<TranslationProcessor> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslationProcessor"/> class.
    /// </summary>
    /// <param name="translationService">The translation service.</param>
    /// <param name="logger">The logger.</param>
    public TranslationProcessor(ITranslationProcessor translationService, ILogger<TranslationProcessor> logger)
    {
        this.translationProcessor = translationService;
        this.logger = logger;
    }

    /// <summary>
    /// Processes a translation request.
    /// </summary>
    /// <param name="message">The message to process.</param>
    /// <param name="messageActions">The message actions.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

            await translationProcessor.ProcessTranslationAsync(message.MessageId, cancellationToken);

            logger.LogInformation("Finished processing translation with ID: {TranslationId}", message.MessageId);

            await messageActions.CompleteMessageAsync(message, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing translation message with ID: {MessageId}", message.MessageId);

            // Let the exception propagate to retry the message based on the Service Bus retry policy
            throw;
        }
    }
}
