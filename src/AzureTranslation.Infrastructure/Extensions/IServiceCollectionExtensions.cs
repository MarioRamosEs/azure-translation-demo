using Azure;

using AzureTranslation.Core.Interfaces;
using AzureTranslation.Infrastructure.Options;
using AzureTranslation.Infrastructure.Repositories;
using AzureTranslation.Infrastructure.Services;

using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AzureTranslation.Infrastructure.Extensions;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to register infrastructure services.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds Azure Table Storage services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The configuration containing the connection information.</param>
    /// <returns>The service collection to enable method chaining.</returns>
    public static IServiceCollection AddTableStorageServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<TableStorageOptions>().Bind(configuration.GetSection(nameof(TableStorageOptions))).ValidateDataAnnotations();

        services.AddAzureClients(builder =>
        {
            builder.AddTableServiceClient(configuration.GetConnectionString("StorageAccount"));
        });

        return services;
    }

    /// <summary>
    /// Adds Azure Service Bus services to the service collection. Also implements <see cref="IMessageBusService"/> for sending messages to the Service Bus.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The configuration containing the connection information.</param>
    /// <returns>The service collection to enable method chaining.</returns>
    public static IServiceCollection AddAzureBusServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<ServiceBusOptions>().Bind(configuration.GetSection(nameof(ServiceBusOptions))).ValidateDataAnnotations();

        services.AddAzureClients(builder =>
        {
            builder.AddServiceBusClient(configuration.GetConnectionString("ServiceBus"));
        });

        services.AddSingleton<IMessageBusService, AzureServiceBusService>();

        return services;
    }

    /// <summary>
    /// Adds the Azure Table Storage implementation of <see cref="ITranslationRepository"/> to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection to enable method chaining.</returns>
    public static IServiceCollection AddTableStorageTranslationRepository(this IServiceCollection services)
    {
        return services.AddSingleton<ITranslationRepository, TableStorageTranslationRepository>();
    }

    /// <summary>
    /// Adds Azure Cognitive Services <see cref="ITextTranslationService"/> and <see cref="ILanguageDetectionService"/> implementations to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The configuration containing API keys and endpoints.</param>
    /// <returns>The service collection to enable method chaining.</returns>
    public static IServiceCollection AddAzureCognitiveLanguageServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<TranslatorOptions>()
            .Bind(configuration.GetSection(nameof(TranslatorOptions)))
            .ValidateDataAnnotations();

        services.AddOptionsWithValidateOnStart<LanguageOptions>()
            .Bind(configuration.GetSection(nameof(LanguageOptions)))
            .ValidateDataAnnotations();

        var translatorOptions = configuration.GetSection(nameof(TranslatorOptions)).Get<TranslatorOptions>()!;
        var languageOptions = configuration.GetSection(nameof(LanguageOptions)).Get<LanguageOptions>()!;

        services.AddAzureClients(builder =>
        {
            builder.AddTextTranslationClient(new AzureKeyCredential(translatorOptions.Key), translatorOptions.Region);

            builder.AddTextAnalyticsClient(new Uri(languageOptions.Endpoint), new AzureKeyCredential(languageOptions.Key));
        });

        services.AddSingleton<ILanguageDetectionService, CognitiveServicesLanguageDetector>();
        services.AddSingleton<ITextTranslationService, CognitiveServicesTranslator>();

        return services;
    }
}
