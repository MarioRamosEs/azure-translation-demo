using AzureTranslation.Common.Models;
using AzureTranslation.Core.Entities;

namespace AzureTranslation.Core.Interfaces;

public interface ITranslationRepository
{
    Task<TranslationEntity?> GetTranslationAsync(string translationId, CancellationToken cancellationToken);

    Task CreateTranslationAsync(TranslationEntity translation, CancellationToken cancellationToken);

    Task UpdateTranslationAsync(TranslationEntity translation, CancellationToken cancellationToken);
}
