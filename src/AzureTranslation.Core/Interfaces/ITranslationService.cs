using AzureTranslation.Common.Models;

namespace AzureTranslation.Core.Interfaces;

public interface ITranslationService
{
    Task<Translation> CreateTranslationRequestAsync(string text, CancellationToken cancellationToken);

    Task<Translation?> GetTranslationAsync(string translationId, CancellationToken cancellationToken);

    Task ProcessTranslationAsync(string translationId, CancellationToken cancellationToken);
}
