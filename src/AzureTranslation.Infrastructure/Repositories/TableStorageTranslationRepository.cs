using Azure;
using Azure.Data.Tables;

using AzureTranslation.Common.Models;
using AzureTranslation.Core.Interfaces;
using AzureTranslation.Infrastructure.Constants;
using AzureTranslation.Infrastructure.Entities;
using AzureTranslation.Infrastructure.Options;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzureTranslation.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for storing and retrieving translation data using Azure Table Storage.
/// </summary>
internal sealed class TableStorageTranslationRepository : ITranslationRepository
{
    private readonly TableClient tableClient;
    private readonly ILogger<TableStorageTranslationRepository> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TableStorageTranslationRepository"/> class.
    /// </summary>
    /// <param name="tableServiceClient">The Azure Table Storage service client.</param>
    /// <param name="options">Configuration options for Table Storage settings.</param>
    /// <param name="logger">The logger for logging diagnostic information.</param>
    public TableStorageTranslationRepository(TableServiceClient tableServiceClient, IOptions<TableStorageOptions> options, ILogger<TableStorageTranslationRepository> logger)
    {
        tableClient = tableServiceClient.GetTableClient(options.Value.TranslationsTableName);
        this.logger = logger;
    }

    /// <summary>
    /// Creates a new translation record in the table storage.
    /// </summary>
    /// <param name="translation">The translation object to store.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="RequestFailedException">Thrown when storage operation fails.</exception>
    public async Task CreateTranslationAsync(Translation translation, CancellationToken cancellationToken)
    {
        try
        {
            var translationEntity = TranslationEntity.FromTranslation(translation);
            await tableClient.AddEntityAsync(translationEntity, cancellationToken: cancellationToken);
            logger.LogInformation("Translation with ID {TranslationId} created successfully", translation.Id);
        }
        catch (RequestFailedException ex)
        {
            logger.LogError(ex, "Failed to create translation with ID {TranslationId}", translation.Id);
            throw;
        }
    }

    /// <summary>
    /// Retrieves a translation record from table storage by its ID.
    /// </summary>
    /// <param name="translationId">The unique identifier of the translation to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation, containing the translation if found, or null if not found.</returns>
    /// <exception cref="Exception">Thrown when storage operation fails for reasons other than the entity not being found.</exception>
    public async Task<Translation?> GetTranslationAsync(string translationId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await tableClient.GetEntityAsync<TranslationEntity>(StorageConstants.PartitionKeys.Translation, translationId, cancellationToken: cancellationToken);
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

    /// <summary>
    /// Updates an existing translation record in table storage.
    /// </summary>
    /// <param name="translation">The translation object with updated values.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="Exception">Thrown when storage operation fails.</exception>
    public async Task UpdateTranslationAsync(Translation translation, CancellationToken cancellationToken)
    {
        try
        {
            var translationEntity = TranslationEntity.FromTranslation(translation);
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
