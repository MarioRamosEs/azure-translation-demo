namespace AzureTranslation.Core.Interfaces;

public interface ILanguageDetectionService
{
    Task<string> DetectLanguageAsync(string text, CancellationToken cancellationToken);
}
