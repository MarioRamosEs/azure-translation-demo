namespace AzureTranslation.Core.Interfaces;

/// <summary>
/// Service for translating text between languages.
/// </summary>
public interface ITextTranslationService
{
    /// <summary>
    /// Translates text from one language to another.
    /// </summary>
    /// <param name="text">The text to translate.</param>
    /// <param name="sourceLanguage">The source language code.</param>
    /// <param name="destinationLanguage">The destination language code.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The translated text.</returns>
    Task<string> TranslateAsync(string text, string sourceLanguage, string destinationLanguage, CancellationToken cancellationToken);
}
