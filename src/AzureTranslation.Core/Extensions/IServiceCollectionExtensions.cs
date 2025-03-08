using AzureTranslation.Core.Interfaces;
using AzureTranslation.Core.Services;

using Microsoft.Extensions.DependencyInjection;

namespace AzureTranslation.Core.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddTranslationRequestService(this IServiceCollection services)
    {
        return services.AddScoped<ITranslationRequestService, TranslationRequestService>();
    }

    public static IServiceCollection AddTranslationProcessingServices(this IServiceCollection services)
    {
        return services.AddScoped<ITranslationProcessor, TranslationProcessor>();
    }
}
