using System.Text.Json.Serialization;

using AzureTranslation.Common.Enums;

namespace AzureTranslation.Common.Models;

public class Translation
{
    public string Id { get; set; }

    public string OriginalText { get; set; }

    public string TranslatedText { get; set; }

    public string DetectedLanguage { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TranslationStatus Status { get; set; }

    public string ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}
