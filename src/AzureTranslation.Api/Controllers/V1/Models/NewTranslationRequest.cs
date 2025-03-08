using System.ComponentModel.DataAnnotations;

namespace AzureTranslation.API.Controllers.V1.Models;

/// <summary>
/// Represents a request to create a new translation.
/// </summary>
public sealed class NewTranslationRequest
{
    /// <summary>
    /// Gets the original text to be translated.
    /// </summary>
    [Required]
    public required string OriginalText { get; init; }
}
