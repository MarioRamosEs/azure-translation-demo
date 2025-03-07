using System.ComponentModel.DataAnnotations;

namespace AzureTranslation.Infrastructure.Options;

public sealed class TableStorageOptions
{
    [Required]
    [MinLength(1)]
    public string TranslationsTableName { get; init; }
}
