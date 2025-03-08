using Azure;
using Azure.Data.Tables;

using AzureTranslation.Common.Models;
using AzureTranslation.Infrastructure.Constants;

namespace AzureTranslation.Infrastructure.Entities;

/// <summary>
/// Entity class for storing translations in Azure Table Storage.
/// Extends the base <see cref="Translation"/> class and implements <see cref="ITableEntity"/>.
/// </summary>
public class TranslationEntity : Translation, ITableEntity
{
    /// <summary>
    /// Gets or sets the partition key for the entity in Azure Table Storage.
    /// </summary>
    public string PartitionKey { get; set; } = StorageConstants.PartitionKeys.Translation;

    /// <summary>
    /// Gets or sets the row key for the entity in Azure Table Storage.
    /// </summary>
    public required string RowKey { get; set; }

    /// <summary>
    /// Gets or sets the timestamp for the entity in Azure Table Storage.
    /// </summary>
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the ETag for the entity in Azure Table Storage.
    /// </summary>
    public ETag ETag { get; set; }

    /// <summary>
    /// Creates a new <see cref="TranslationEntity"/> from a <see cref="Translation"/> object.
    /// </summary>
    /// <param name="translation">The translation object to convert.</param>
    /// <returns>A new <see cref="TranslationEntity"/> with properties copied from the translation object.</returns>
    public static TranslationEntity FromTranslation(Translation translation)
    {
        return new TranslationEntity
        {
            Id = translation.Id,
            RowKey = translation.Id,
            OriginalText = translation.OriginalText,
            TranslatedText = translation.TranslatedText,
            DetectedLanguage = translation.DetectedLanguage,
            Status = translation.Status,
            ErrorMessage = translation.ErrorMessage,
            CreatedAt = translation.CreatedAt,
            CompletedAt = translation.CompletedAt,
        };
    }

    /// <summary>
    /// Converts this entity to a standard <see cref="Translation"/> object.
    /// </summary>
    /// <returns>A new <see cref="Translation"/> object with copied properties from this entity.</returns>
    public Translation ToTranslation()
    {
        return new Translation
        {
            Id = Id,
            OriginalText = OriginalText,
            TranslatedText = TranslatedText,
            DetectedLanguage = DetectedLanguage,
            Status = Status,
            ErrorMessage = ErrorMessage,
            CreatedAt = CreatedAt,
            CompletedAt = CompletedAt,
        };
    }
}
