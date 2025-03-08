using Azure;
using Azure.Data.Tables;

using AzureTranslation.Common.Models;
using AzureTranslation.Core.Interfaces;
using AzureTranslation.Infrastructure.Entities;
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

    public async Task CreateTranslationAsync(Translation translation, CancellationToken cancellationToken)
    {
        try
        {
            var translationEntity = TranslationEntity.FromTranslation(translation);
            translationEntity.PartitionKey = PartitionKey;
            await tableClient.AddEntityAsync(translationEntity, cancellationToken: cancellationToken);
            logger.LogInformation("Translation with ID {TranslationId} created successfully", translation.Id);
        }
        catch (RequestFailedException ex)
        {
            logger.LogError(ex, "Failed to create translation with ID {TranslationId}", translation.Id);
            throw;
        }
    }

    public async Task<Translation?> GetTranslationAsync(string translationId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await tableClient.GetEntityAsync<TranslationEntity>(PartitionKey, translationId, cancellationToken: cancellationToken);
            return response.Value.ToTranslation();
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            logger.LogWarning("Translation with ID {TranslationId} not found", translationId);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving translation with ID {TranslationId}", translationId);
            throw;
        }
    }

    public async Task UpdateTranslationAsync(Translation translation, CancellationToken cancellationToken)
    {
        try
        {
            var translationEntity = TranslationEntity.FromTranslation(translation);
            translationEntity.PartitionKey = PartitionKey;

            await tableClient.UpdateEntityAsync(translationEntity, ETag.All, cancellationToken: cancellationToken);

            logger.LogInformation("Translation with ID {TranslationId} updated successfully", translation.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update translation with ID {TranslationId}", translation.Id);
            throw;
        }
    }
}
