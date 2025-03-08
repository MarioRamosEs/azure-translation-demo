using AzureTranslation.Common.Models;

namespace AzureTranslation.Core.Interfaces;

/// <summary>
/// Service for managing translation requests from the API.
/// </summary>
public interface ITranslationRequestService
{
    /// <summary>
    /// Creates a translation request and queues it for asynchronous processing.
    /// </summary>
    /// <param name="text">Original text to translate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created translation entity.</returns>
    Task<Translation> CreateTranslationRequestAsync(string text, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves an existing translation by its ID.
    /// </summary>
    /// <param name="translationId">ID of the translation.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The translation entity or null if it doesn't exist.</returns>
    Task<Translation?> GetTranslationAsync(string translationId, CancellationToken cancellationToken);
}
