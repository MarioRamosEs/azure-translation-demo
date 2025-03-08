using System.Text.Json.Serialization;

using AzureTranslation.Common.Enums;

namespace AzureTranslation.Common.Models;

/// <summary>
/// Represents a translation request and its results.
/// </summary>
public class Translation
{
    /// <summary>
    /// Gets the unique identifier for the translation.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the original text to be translated.
    /// </summary>
    public required string OriginalText { get; init; }

    /// <summary>
    /// Gets or sets the translated text.
    /// </summary>
    public string TranslatedText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detected language code of the original text.
    /// </summary>
    public string DetectedLanguage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the translation.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TranslationStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the error message if the translation fails.
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the translation request was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the translation was completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }
}
