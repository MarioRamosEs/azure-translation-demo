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
    /// Implements <see cref="ITranslationRequestService"/> to handle translation requests.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection to enable method chaining.</returns>
    public static IServiceCollection AddTranslationRequestService(this IServiceCollection services)
    {
        return services.AddSingleton<ITranslationRequestService, TranslationRequestService>();
    }

    /// <summary>
    /// Implements <see cref="ITranslationProcessor"/> to handle translation processing.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection to enable method chaining.</returns>
    public static IServiceCollection AddTranslationProcessingServices(this IServiceCollection services)
    {
        return services.AddSingleton<ITranslationProcessor, TranslationProcessor>();
    }
}
