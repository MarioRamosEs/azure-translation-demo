using System.ComponentModel.DataAnnotations;

namespace AzureTranslation.API.Controllers.V1.Models;

public sealed class NewTranslationRequest
{
    [Required]
    public required string OriginalText { get; init; }
}
