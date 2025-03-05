using AzureTranslation.Core.Interfaces;

namespace AzureTranslation.Infrastructure.Services;

internal sealed class CognitiveServicesTranslator : ITextTranslationService
{
    public Task<string> TranslateToSpanishAsync(string text, string sourceLanguage, CancellationToken cancellationToken) => throw new NotImplementedException();
}
