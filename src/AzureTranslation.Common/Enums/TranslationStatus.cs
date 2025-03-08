namespace AzureTranslation.Common.Enums;

/// <summary>
/// Represents the status of a translation request.
/// </summary>
public enum TranslationStatus
{
    /// <summary>
    /// The translation request has been created but not yet processed.
    /// </summary>
    Pending,

    /// <summary>
    /// The translation request is currently being processed.
    /// </summary>
    Processing,

    /// <summary>
    /// The translation has been successfully completed.
    /// </summary>
    Completed,

    /// <summary>
    /// The translation process has failed.
    /// </summary>
    Failed,
}