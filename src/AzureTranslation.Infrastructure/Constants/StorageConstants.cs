namespace AzureTranslation.Infrastructure.Constants;

/// <summary>
/// Contains constant values used for Azure Storage operations.
/// </summary>
public static class StorageConstants
{
    /// <summary>
    /// Constants related to table storage partition keys.
    /// </summary>
    public static class PartitionKeys
    {
        /// <summary>
        /// Default partition key for translation entities.
        /// </summary>
        public const string Translation = "Translation";
    }
}
