using Azure;

using AzureTranslation.Core.Interfaces;
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

    public static IServiceCollection AddAzureCognitiveLanguageServices(this IServiceCollection services, IConfiguration configuration)
    {
        var translatorApiKey = configuration.GetValue<string>("TranslatorOptions:Key");
        if (string.IsNullOrWhiteSpace(translatorApiKey))
        {
            throw new InvalidOperationException("The TranslatorOptions:Key configuration value is missing.");
        }

        var languageApiKey = configuration.GetValue<string>("LanguageOptions:Key");
        if (string.IsNullOrWhiteSpace(languageApiKey))
        {
            throw new InvalidOperationException("The LanguageOptions:Key configuration value is missing.");
        }

        services.AddAzureClients(builder =>
        {
            builder.AddTextTranslationClient(new AzureKeyCredential(translatorApiKey), "francecentral"); // TODO load from configuration

            builder.AddTextAnalyticsClient(new Uri("https://francecentral.api.cognitive.microsoft.com/"), new AzureKeyCredential(languageApiKey)); // TODO load from configuration
        });

        services.AddScoped<ILanguageDetectionService, CognitiveServicesLanguageDetector>();
        services.AddScoped<ITextTranslationService, CognitiveServicesTranslator>();

        return services;
    }
}
