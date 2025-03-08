using Azure.AI.TextAnalytics;

using AzureTranslation.Core.Interfaces;

using Microsoft.Extensions.Logging;

namespace AzureTranslation.Infrastructure.Services;

internal sealed class CognitiveServicesLanguageDetector : ILanguageDetectionService
{
    private readonly TextAnalyticsClient textAnalyticsClient;
    private readonly ILogger<CognitiveServicesLanguageDetector> logger; // TODO do something with this logger

    public CognitiveServicesLanguageDetector(TextAnalyticsClient textAnalyticsClient, ILogger<CognitiveServicesLanguageDetector> logger)
    {
        this.textAnalyticsClient = textAnalyticsClient;
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task<string?> DetectLanguageAsync(string text, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(text))
        {
            logger.LogInformation("Empty text provided for Language Detection.");
            return null;
        }

        var detectedLanguage = await textAnalyticsClient.DetectLanguageAsync(text, cancellationToken: cancellationToken);

        return detectedLanguage.Value.Iso6391Name;
    }
}
