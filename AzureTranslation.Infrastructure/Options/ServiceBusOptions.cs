using System.ComponentModel.DataAnnotations;

namespace AzureTranslation.Infrastructure.Options;

public sealed class ServiceBusOptions
{
    [Required]
    [MinLength(1)]
    public string QueueName { get; init; }
}
