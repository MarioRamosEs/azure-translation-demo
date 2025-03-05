using Azure;
using Azure.Data.Tables;

namespace AzureTranslation.Commons.Models;

public class TranslationEntity : ITableEntity
{
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public string OriginalText { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? DetectedLanguage { get; set; }
    public string? TranslatedText { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
