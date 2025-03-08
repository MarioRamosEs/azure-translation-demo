using AzureTranslation.Common.Models;

namespace AzureTranslation.Core.Interfaces;

public interface ITranslationRepository
{
    Task<Translation?> GetTranslationAsync(string translationId, CancellationToken cancellationToken);

    Task CreateTranslationAsync(Translation translation, CancellationToken cancellationToken);

    Task UpdateTranslationAsync(Translation translation, CancellationToken cancellationToken);
}
