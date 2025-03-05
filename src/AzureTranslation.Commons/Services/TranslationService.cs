using AzureTranslation.Common.Models;
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

    public Task<string> CreateTranslationRequestAsync(string text, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task<TranslationDto> GetTranslationAsync(string translationId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task ProcessTranslationAsync(string translationId, CancellationToken cancellationToken) => throw new NotImplementedException();
}
