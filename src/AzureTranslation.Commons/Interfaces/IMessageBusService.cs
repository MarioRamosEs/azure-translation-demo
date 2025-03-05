namespace AzureTranslation.Core.Interfaces;

public interface IMessageBusService
{
    Task SendTranslationMessageAsync(string translationId, CancellationToken cancellationToken);
}
