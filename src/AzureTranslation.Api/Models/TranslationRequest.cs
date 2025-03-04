using System.ComponentModel.DataAnnotations;

namespace AzureTranslation.Api.Models;

public sealed class TranslationRequest
{
    [Required]
    public required string Input { get; init; }
}
