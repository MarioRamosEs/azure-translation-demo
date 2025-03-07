using AzureTranslation.Common.Enums;
using AzureTranslation.Common.Models;
using AzureTranslation.Core.Entities;
using AzureTranslation.Core.Interfaces;

using Microsoft.Extensions.Logging;

namespace AzureTranslation.Core.Services;

internal sealed class TranslationService : ITranslationService
{
    private readonly ITranslationRepository translationRepository;
    private readonly ILanguageDetectionService languageDetectionService;
    private readonly ITextTranslationService textTranslationService;
    private readonly IMessageBusService messageBusService;
    private readonly ILogger<TranslationService> logger;

    public TranslationService(
            ITranslationRepository translationRepository,
            ILanguageDetectionService languageDetectionService,
            ITextTranslationService textTranslationService,
            IMessageBusService messageBusService,
            ILogger<TranslationService> logger)
    {
        this.textTranslationService = textTranslationService;
        this.languageDetectionService = languageDetectionService;
        this.translationRepository = translationRepository;
        this.messageBusService = messageBusService;
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task<string> CreateTranslationRequestAsync(string text, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating translation request for text");

        var translationId = Guid.NewGuid().ToString();

        var translationEntity = new TranslationEntity
        {
            RowKey = translationId,
            OriginalText = text,
            Status = TranslationStatus.Pending.ToString(),
            CreatedAt = DateTime.UtcNow,
        };

        await translationRepository.CreateTranslationAsync(translationEntity, cancellationToken);
        await messageBusService.SendTranslationMessageAsync(translationId, cancellationToken);

        logger.LogInformation("Translation request created with ID: {TranslationId}", translationId);

        return translationId;
    }

    /// <inheritdoc />
    public async Task<TranslationDto?> GetTranslationAsync(string translationId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting translation with ID: {TranslationId}", translationId);

        var entity = await translationRepository.GetTranslationAsync(translationId, cancellationToken);

        if (entity == null)
        {
            logger.LogWarning("Translation with ID {TranslationId} not found", translationId);
            return null;
        }

        return new TranslationDto
        {
            Id = entity.RowKey,
            OriginalText = entity.OriginalText,
            TranslatedText = entity.TranslatedText,
            DetectedLanguage = entity.DetectedLanguage,
            Status = Enum.Parse<TranslationStatus>(entity.Status),
            ErrorMessage = entity.ErrorMessage,
            CreatedAt = entity.CreatedAt,
            CompletedAt = entity.CompletedAt,
        };
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
            translation.Status = TranslationStatus.Completed.ToString();
            translation.CompletedAt = DateTime.UtcNow;

            await translationRepository.UpdateTranslationAsync(translation, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing translation with ID: {TranslationId}", translationId);
            translation.Status = TranslationStatus.Failed.ToString();
            translation.ErrorMessage = ex.Message;
        }

        await translationRepository.UpdateTranslationAsync(translation, cancellationToken);
        logger.LogInformation("Translation processing completed for ID: {TranslationId} with status: {Status}", translationId, translation.Status);
    }
}
