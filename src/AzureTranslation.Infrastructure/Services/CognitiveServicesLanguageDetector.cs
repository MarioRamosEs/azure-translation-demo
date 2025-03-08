using Azure.AI.TextAnalytics;

using AzureTranslation.Core.Interfaces;

using Microsoft.Extensions.Logging;

namespace AzureTranslation.Infrastructure.Services;

/// <summary>
/// Implementation of language detection service using Azure Cognitive Services.
/// This class provides language detection capabilities using Azure's Text Analytics API.
/// </summary>
internal sealed class CognitiveServicesLanguageDetector : ILanguageDetectionService
{
    private readonly TextAnalyticsClient textAnalyticsClient;
    private readonly ILogger<CognitiveServicesLanguageDetector> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CognitiveServicesLanguageDetector"/> class.
    /// </summary>
    /// <param name="textAnalyticsClient">Client to access Azure's text analytics services.</param>
    /// <param name="logger">The logger for logging diagnostic information.</param>
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
