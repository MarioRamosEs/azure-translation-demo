using AzureTranslation.Common.Models;

namespace AzureTranslation.Core.Interfaces;

public interface ITranslationService
{
    Task<string> CreateTranslationRequestAsync(string text, CancellationToken cancellationToken);

    Task<TranslationDto?> GetTranslationAsync(string translationId, CancellationToken cancellationToken);

    Task ProcessTranslationAsync(string translationId, CancellationToken cancellationToken);
}
