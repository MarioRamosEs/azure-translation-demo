using Azure;
using Azure.Data.Tables;

using AzureTranslation.Common.Models;

namespace AzureTranslation.Infrastructure.Entities;

public class TranslationEntity : Translation, ITableEntity
{
    public string PartitionKey { get; set; }

    public string RowKey { get; set; }

    public DateTimeOffset? Timestamp { get; set; }

    public ETag ETag { get; set; }

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
}
