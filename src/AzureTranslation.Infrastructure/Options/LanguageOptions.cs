namespace AzureTranslation.Infrastructure.Options;

/// <summary>
/// Configuration options for the Azure Language service.
/// </summary>
public sealed class LanguageOptions
{
    /// <summary>
    /// Gets or initializes the API key for the Language service.
    /// </summary>
    public string Key { get; init; }

    /// <summary>
    /// Gets or initializes the endpoint URL for the Language service.
    /// </summary>
    public string Endpoint { get; init; }
}
