using System.ComponentModel.DataAnnotations;

namespace AzureTranslation.Infrastructure.Options;

/// <summary>
/// Configuration options for the Azure Language service.
/// </summary>
public sealed class LanguageOptions
{
    /// <summary>
    /// Gets or initializes the API key for the Language service.
    /// </summary>
    [Required]
    [MinLength(1)]
    public required string Key { get; init; }

    /// <summary>
    /// Gets or initializes the endpoint URL for the Language service.
    /// </summary>
    [Required]
    [MinLength(1)]
    public required string Endpoint { get; init; }
}
