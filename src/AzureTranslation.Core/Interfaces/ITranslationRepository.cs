using AzureTranslation.Common.Models;

namespace AzureTranslation.Core.Interfaces;

/// <summary>
/// Repository interface for translation data persistence operations.
/// </summary>
public interface ITranslationRepository
{
    /// <summary>
    /// Gets a translation by its ID.
    /// </summary>
    /// <param name="translationId">The ID of the translation to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>The translation if found; otherwise, null.</returns>
    Task<Translation?> GetTranslationAsync(string translationId, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new translation in the repository.
    /// </summary>
    /// <param name="translation">The translation to create.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateTranslationAsync(Translation translation, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing translation in the repository.
    /// </summary>
    /// <param name="translation">The translation with updated data.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateTranslationAsync(Translation translation, CancellationToken cancellationToken);
}
