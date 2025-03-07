namespace AzureTranslation.Core.Interfaces;

public interface ILanguageDetectionService
{
    /// <summary>
    /// Obtains the Iso6391Name of the detected language for the given text.
    /// </summary>
    /// <param name="text">The text to detect the language for. </param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>The Iso6391Name of the detected language.</returns>
    Task<string?> DetectLanguageAsync(string text, CancellationToken cancellationToken);
}
