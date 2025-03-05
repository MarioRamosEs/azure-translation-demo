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
        // Options configuration
        services.Configure<TableStorageOptions>(configuration.GetSection("TableStorage"));

        // Azure Services
        services.AddAzureClients(builder =>
        {
            // Table Storage
            builder.AddTableServiceClient(configuration.GetConnectionString("TableStorage"));
        });

        return services;
    }

    public static IServiceCollection AddServiceBusServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Options configuration
        services.Configure<ServiceBusOptions>(configuration.GetSection("ServiceBus"));

        // Azure Services
        services.AddAzureClients(builder =>
        {
            // Service Bus
            builder.AddServiceBusClient(configuration.GetConnectionString("ServiceBus"));
        });

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
