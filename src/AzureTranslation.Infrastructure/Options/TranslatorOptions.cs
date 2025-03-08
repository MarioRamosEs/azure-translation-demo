using System.ComponentModel.DataAnnotations;

namespace AzureTranslation.Infrastructure.Options;

/// <summary>
/// Configuration options for the Azure Translator service.
/// </summary>
public sealed class TranslatorOptions
{
    /// <summary>
    /// Gets or initializes the Azure region where the Translator resource is deployed.
    /// </summary>
    [Required]
    [MinLength(1)]
    public required string Region { get; init; }

    /// <summary>
    /// Gets or initializes the API key for the Translator service.
    /// </summary>
    [Required]
    [MinLength(1)]
    public required string Key { get; init; }
}
