using Azure;
using Azure.Data.Tables;

using AzureTranslation.Common.Models;

namespace AzureTranslation.Core.Entities;

public class TranslationEntity : Translation, ITableEntity
{
    public string PartitionKey { get; set; }

    public string RowKey { get; set; }

    public DateTimeOffset? Timestamp { get; set; }

    public ETag ETag { get; set; }
}
