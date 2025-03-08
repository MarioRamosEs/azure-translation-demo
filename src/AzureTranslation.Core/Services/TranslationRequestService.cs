using AzureTranslation.Common.Enums;
using AzureTranslation.Common.Models;
using AzureTranslation.Core.Interfaces;

using Microsoft.Extensions.Logging;

namespace AzureTranslation.Core.Services;

internal sealed class TranslationRequestService : ITranslationRequestService
{
    private readonly ITranslationRepository translationRepository;
    private readonly IMessageBusService messageBusService;
    private readonly ILogger<TranslationRequestService> logger;

    public TranslationRequestService(
            ITranslationRepository translationRepository,
            IMessageBusService messageBusService,
            ILogger<TranslationRequestService> logger)
    {
        this.translationRepository = translationRepository;
        this.messageBusService = messageBusService;
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task<Translation> CreateTranslationRequestAsync(string text, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating translation request for text");

        var translation = new Translation
        {
            Id = Guid.NewGuid().ToString(),
            OriginalText = text,
            Status = TranslationStatus.Pending,
            CreatedAt = DateTime.UtcNow,
        };

        await translationRepository.CreateTranslationAsync(translation, cancellationToken);
        await messageBusService.SendTranslationMessageAsync(translation.Id, cancellationToken);

        logger.LogInformation("Translation request created with ID: {TranslationId}", translation.Id);

        return translation;
    }

    /// <inheritdoc />
    public async Task<Translation?> GetTranslationAsync(string translationId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting translation with ID: {TranslationId}", translationId);

        var entity = await translationRepository.GetTranslationAsync(translationId, cancellationToken);

        if (entity == null)
        {
            logger.LogWarning("Translation with ID {TranslationId} not found", translationId);
            return null;
        }

        return entity;
    }
}
