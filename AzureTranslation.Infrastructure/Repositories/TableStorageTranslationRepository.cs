using Azure.Data.Tables;

using AzureTranslation.Core.Entities;
using AzureTranslation.Core.Interfaces;
using AzureTranslation.Infrastructure.Options;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzureTranslation.Infrastructure.Repositories;

internal sealed class TableStorageTranslationRepository : ITranslationRepository
{
    private readonly TableClient tableClient;
    private readonly ILogger<TableStorageTranslationRepository> logger;

    public TableStorageTranslationRepository(TableServiceClient tableServiceClient, IOptions<TableStorageOptions> options, ILogger<TableStorageTranslationRepository> logger)
    {
        tableClient = tableServiceClient.GetTableClient(options.Value.TranslationsTableName);
        this.logger = logger;
    }

    public Task CreateTranslationAsync(TranslationEntity translation, CancellationToken cancellationToken)
    {
        return tableClient.AddEntityAsync(translation, cancellationToken: cancellationToken); // Todo establecer aqui el partition key
    }

    public Task<TranslationEntity> GetTranslationAsync(string translationId, CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task UpdateTranslationAsync(TranslationEntity translation, CancellationToken cancellationToken) => throw new NotImplementedException();
}
