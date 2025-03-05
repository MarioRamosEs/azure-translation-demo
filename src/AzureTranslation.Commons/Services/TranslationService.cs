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
            PartitionKey = "Translation", // TODO: porque ponemos esto aqui
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
    public Task<TranslationDto> GetTranslationAsync(string translationId, CancellationToken cancellationToken) => throw new NotImplementedException();

    /// <inheritdoc />
    public Task ProcessTranslationAsync(string translationId, CancellationToken cancellationToken) => throw new NotImplementedException();
}
