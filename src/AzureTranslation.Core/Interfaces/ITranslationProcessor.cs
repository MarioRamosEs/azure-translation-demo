namespace AzureTranslation.Core.Interfaces;

/// <summary>
/// Service for processing translations.
/// </summary>
public interface ITranslationProcessor
{
    /// <summary>
    /// Processes a translation request using language detection and translation services.
    /// </summary>
    /// <param name="translationId">ID of the translation to process.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ProcessTranslationAsync(string translationId, CancellationToken cancellationToken);
}
