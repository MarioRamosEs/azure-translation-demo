using System.ComponentModel.DataAnnotations;

namespace AzureTranslation.Infrastructure.Options;

/// <summary>
/// Configuration options for Azure Service Bus.
/// </summary>
public sealed class ServiceBusOptions
{
    /// <summary>
    /// Gets or initializes the name of the queue for processing translations.
    /// </summary>
    [Required]
    [MinLength(1)]
    public string QueueName { get; init; }
}
