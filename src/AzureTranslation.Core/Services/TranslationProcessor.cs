using AzureTranslation.Common.Enums;
using AzureTranslation.Core.Interfaces;

using Microsoft.Extensions.Logging;

namespace AzureTranslation.Core.Services;

/// <summary>
/// Service for processing translations.
/// </summary>
internal sealed class TranslationProcessor : ITranslationProcessor
{
    private readonly ITranslationRepository translationRepository;
    private readonly ILanguageDetectionService languageDetectionService;
    private readonly ITextTranslationService textTranslationService;
    private readonly ILogger<TranslationProcessor> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslationProcessor"/> class.
    /// </summary>
    /// <param name="translationRepository">A valid <see cref="ITranslationRepository"/> instance.</param>
    /// <param name="languageDetectionService">A valid <see cref="ILanguageDetectionService"/> instance.</param>
    /// <param name="textTranslationService">A valid <see cref="ITextTranslationService"/> instance.</param>
    /// <param name="logger">The logger.</param>
    public TranslationProcessor(
            ITranslationRepository translationRepository,
            ILanguageDetectionService languageDetectionService,
            ITextTranslationService textTranslationService,
            ILogger<TranslationProcessor> logger)
    {
        this.textTranslationService = textTranslationService;
        this.languageDetectionService = languageDetectionService;
        this.translationRepository = translationRepository;
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task ProcessTranslationAsync(string translationId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing translation with ID: {TranslationId}", translationId);

        var translation = await translationRepository.GetTranslationAsync(translationId, cancellationToken);

        if (translation == null)
        {
            logger.LogWarning("Translation with ID {TranslationId} not found for processing", translationId);
            return;
        }

        if (translation.Status != TranslationStatus.Pending)
        {
            logger.LogWarning("Translation with ID {TranslationId} is not in pending status. Current status: {Status}", translationId, translation.Status);
            return;
        }

        try
        {
            // Detect language
            translation.DetectedLanguage = await languageDetectionService.DetectLanguageAsync(translation.OriginalText, cancellationToken) ?? "unknown";

            // Translate if not Spanish or unknown
            if (translation.DetectedLanguage != "es" && translation.DetectedLanguage != "unknown")
            {
                translation.TranslatedText = await textTranslationService.TranslateAsync(
                    translation.OriginalText,
                    translation.DetectedLanguage,
                    "es",
                    cancellationToken);
            }
            else
            {
                // If already Spanish, no translation needed
                translation.TranslatedText = translation.OriginalText;
            }

            // Update with success status
            translation.Status = TranslationStatus.Completed;
            translation.CompletedAt = DateTime.UtcNow;

            await translationRepository.UpdateTranslationAsync(translation, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing translation with ID: {TranslationId}", translationId);
            translation.Status = TranslationStatus.Failed;
            translation.ErrorMessage = ex.Message;
        }

        logger.LogInformation("Translation processing completed for ID: {TranslationId} with status: {Status}", translationId, translation.Status);
    }
}
