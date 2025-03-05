namespace AzureTranslation.Core.Interfaces;

public interface ITextTranslationService
{
    Task<string> TranslateToSpanishAsync(string text, string sourceLanguage, CancellationToken cancellationToken); // TODO ver si puedo hacerlo generico
}
