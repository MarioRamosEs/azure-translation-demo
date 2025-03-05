using AzureTranslation.Core.Interfaces;

namespace AzureTranslation.Core.Services;

internal sealed class CognitiveServicesLanguageDetector : ILanguageDetectionService
{
    /// <inheritdoc />
    public Task<string> DetectLanguageAsync(string text, CancellationToken cancellationToken) => throw new NotImplementedException();
}
