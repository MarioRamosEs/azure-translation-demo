using AzureTranslation.Core.Interfaces;
using AzureTranslation.Core.Services;

using Microsoft.Extensions.DependencyInjection;

namespace AzureTranslation.Core.Extensions;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to register core services.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds the translation request service to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection to enable method chaining.</returns>
    public static IServiceCollection AddTranslationRequestService(this IServiceCollection services)
    {
        return services.AddScoped<ITranslationRequestService, TranslationRequestService>();
    }

    /// <summary>
    /// Adds translation processing services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection to enable method chaining.</returns>
    public static IServiceCollection AddTranslationProcessingServices(this IServiceCollection services)
    {
        return services.AddScoped<ITranslationProcessor, TranslationProcessor>();
    }
}
