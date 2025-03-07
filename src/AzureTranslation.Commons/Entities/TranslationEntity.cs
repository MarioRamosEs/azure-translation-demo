﻿using Azure;
using Azure.Data.Tables;

namespace AzureTranslation.Core.Entities;

public class TranslationEntity : ITableEntity
{
    public string PartitionKey { get; set; } // TODO check if we need this here

    public string RowKey { get; set; }

    public DateTimeOffset? Timestamp { get; set; }

    public ETag ETag { get; set; }

    public string OriginalText { get; set; }

    public string TranslatedText { get; set; }

    public string DetectedLanguage { get; set; }

    public string Status { get; set; }

    public string ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}
