namespace AzureTranslation.Core.Interfaces;

/// <summary>
/// Defines the contract for a service that sends translation messages to a message bus.
/// </summary>
public interface IMessageBusService
{
    /// <summary>
    /// Sends a translation message asynchronously.
    /// </summary>
    /// <param name="translationId">The ID of the translation to be sent.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendTranslationMessageAsync(string translationId, CancellationToken cancellationToken);
}
