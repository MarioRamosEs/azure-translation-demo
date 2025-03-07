using AzureTranslation.Core.Interfaces;
using AzureTranslation.Core.Services;
using AzureTranslation.Infrastructure.Options;
using AzureTranslation.Infrastructure.Repositories;
using AzureTranslation.Infrastructure.Services;

using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureTranslation.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddTableStorageServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<TableStorageOptions>().Bind(configuration.GetSection(nameof(TableStorageOptions))).ValidateDataAnnotations();

        services.AddAzureClients(builder =>
        {
            builder.AddTableServiceClient(configuration.GetConnectionString("StorageAccount"));
        });

        return services;
    }

    public static IServiceCollection AddAzureBusServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<ServiceBusOptions>().Bind(configuration.GetSection(nameof(ServiceBusOptions))).ValidateDataAnnotations();

        services.AddAzureClients(builder =>
        {
            builder.AddServiceBusClient(configuration.GetConnectionString("ServiceBus"));
        });

        services.AddScoped<IMessageBusService, AzureServiceBusService>();

        return services;
    }

    public static IServiceCollection AddTableStorageTranslationRepository(this IServiceCollection services)
    {
        return services.AddScoped<ITranslationRepository, TableStorageTranslationRepository>();
    }

    public static IServiceCollection AddAzureCognitiveLanguageServices(this IServiceCollection services)
    {
        services.AddScoped<ILanguageDetectionService, CognitiveServicesLanguageDetector>(); // TODO ver si esto tiene to be Scoped
        services.AddScoped<ITextTranslationService, CognitiveServicesTranslator>(); // TODO ver si esto tiene to be Scoped

        return services;
    }
}
