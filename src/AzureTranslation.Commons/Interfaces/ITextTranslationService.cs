namespace AzureTranslation.Core.Interfaces;

public interface ITextTranslationService
{
    Task<string> TranslateAsync(string text, string sourceLanguage, string destinationLanguage, CancellationToken cancellationToken);
}
