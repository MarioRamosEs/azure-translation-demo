using AzureTranslation.Commons.Models;

namespace AzureTranslation.API.Abstractions;

/// <summary>
/// Provides services for managing translation requests and monitoring their status.
/// </summary>
public interface ITranslationApiService
{
    /// <summary>
    /// Creates a new translation request for the specified text.
    /// </summary>
    /// <param name="originalText">The text to be translated.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A unique identifier for the translation request.</returns>
    Task<string> CreateTranslationRequestAsync(string originalText, CancellationToken cancellationToken);

    ///// <summary>
    ///// Retrieves the current status of a translation request.
    ///// </summary>
    ///// <param name="requestId">The unique identifier of the translation request.</param>
    ///// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    ///// <returns>The current status of the translation request.</returns>
    //Task<TranslationStatus> GetTranslationStatusAsync(string requestId, CancellationToken cancellationToken);
}