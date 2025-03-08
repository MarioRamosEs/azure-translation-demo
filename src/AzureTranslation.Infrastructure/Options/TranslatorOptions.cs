namespace AzureTranslation.Infrastructure.Options;

/// <summary>
/// Configuration options for the Azure Translator service.
/// </summary>
public sealed class TranslatorOptions
{
    /// <summary>
    /// Gets or initializes the Azure region where the Translator resource is deployed.
    /// </summary>
    public string Region { get; init; }

    /// <summary>
    /// Gets or initializes the API key for the Translator service.
    /// </summary>
    public string Key { get; init; }

    /// <summary>
    /// Gets or initializes the endpoint URL for the Translator service.
    /// </summary>
    public string Endpoint { get; init; }
}
