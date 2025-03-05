using AzureTranslation.Core.Entities;
using AzureTranslation.Core.Interfaces;

namespace AzureTranslation.Infrastructure.Repositories;

internal sealed class TranslationRepository : ITranslationRepository
{
    public Task CreateTranslationAsync(TranslationEntity translation, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<TranslationEntity> GetTranslationAsync(string translationId, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task UpdateTranslationAsync(TranslationEntity translation, CancellationToken cancellationToken) => throw new NotImplementedException();
}
