using System.ComponentModel.DataAnnotations;

namespace AzureTranslation.Infrastructure.Options;

/// <summary>
/// Configuration options for Azure Table Storage.
/// </summary>
public sealed class TableStorageOptions
{
    /// <summary>
    /// Gets or initializes the name of the table used to store translations.
    /// </summary>
    [Required]
    [MinLength(1)]
    public string TranslationsTableName { get; init; }
}
