using Azure.AI.Translation.Text;

using AzureTranslation.Core.Interfaces;

using Microsoft.Extensions.Logging;

namespace AzureTranslation.Infrastructure.Services;

/// <summary>
/// Implementation of the text translation service using Azure Cognitive Services.
/// This class provides translation capabilities using Azure's Text Translation API.
/// </summary>
internal sealed class CognitiveServicesTranslator : ITextTranslationService
{
    private readonly TextTranslationClient textTranslationClient;
    private readonly ILogger<CognitiveServicesTranslator> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CognitiveServicesTranslator"/> class.
    /// </summary>
    /// <param name="textTranslationClient">Client to access Azure's text translation services.</param>
    /// <param name="logger">The logger.</param>
    public CognitiveServicesTranslator(TextTranslationClient textTranslationClient, ILogger<CognitiveServicesTranslator> logger)
    {
        this.textTranslationClient = textTranslationClient;
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task<string> TranslateAsync(string text, string sourceLanguage, string destinationLanguage, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(text))
        {
            logger.LogInformation("Empty text provided for translation. Returning original text.");
            return text;
        }

        if (string.IsNullOrEmpty(destinationLanguage))
        {
            throw new ArgumentException("Destination language cannot be empty.", nameof(destinationLanguage));
        }

        var response = await textTranslationClient.TranslateAsync(destinationLanguage, text, sourceLanguage: sourceLanguage);

        var translation = response.Value.FirstOrDefault();

        if (translation == null || translation.Translations == null || !translation.Translations.Any())
        {
            logger.LogWarning("No translation found for text: '{TextSummary}'", text.Length <= 50 ? text : text[..47] + "...");
            return text;
        }

        var translatedText = translation.Translations[0].Text ?? string.Empty;

        return translatedText;
    }
}
