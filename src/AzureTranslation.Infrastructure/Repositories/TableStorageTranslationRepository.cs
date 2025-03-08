using Azure.Data.Tables;

using AzureTranslation.Core.Entities;
using AzureTranslation.Core.Interfaces;
using AzureTranslation.Infrastructure.Options;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzureTranslation.Infrastructure.Repositories;

internal sealed class TableStorageTranslationRepository : ITranslationRepository
{
    private const string PartitionKey = "Translation";

    private readonly TableClient tableClient;
    private readonly ILogger<TableStorageTranslationRepository> logger;

    public TableStorageTranslationRepository(TableServiceClient tableServiceClient, IOptions<TableStorageOptions> options, ILogger<TableStorageTranslationRepository> logger)
    {
        tableClient = tableServiceClient.GetTableClient(options.Value.TranslationsTableName);
        this.logger = logger;
    }

    public Task CreateTranslationAsync(TranslationEntity translation, CancellationToken cancellationToken)
    {
        translation.PartitionKey = PartitionKey;
        return tableClient.AddEntityAsync(translation, cancellationToken: cancellationToken);
    }

    public async Task<TranslationEntity?> GetTranslationAsync(string translationId, CancellationToken cancellationToken)
    {
        try
        {
            return await tableClient.GetEntityAsync<TranslationEntity>(
                "Translation",
                translationId,
                cancellationToken: cancellationToken);
        }
        catch (Azure.RequestFailedException ex) when (ex.Status == 404)
        {
            logger.LogWarning("Translation with ID {TranslationId} not found", translationId);
            return null;
        }
    }

    public async Task UpdateTranslationAsync(TranslationEntity translation, CancellationToken cancellationToken)
    {
        translation.PartitionKey = PartitionKey;
        await tableClient.UpdateEntityAsync(translation, translation.ETag, TableUpdateMode.Merge, cancellationToken); // TODO Modificar esto
    }
}
