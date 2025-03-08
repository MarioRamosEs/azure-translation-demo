using AzureTranslation.Core.Interfaces;
using AzureTranslation.Core.Services;

using Microsoft.Extensions.DependencyInjection;

namespace AzureTranslation.Core.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddTranslationService(this IServiceCollection services)
    {
        services.AddScoped<ITranslationService, TranslationService>(); // TODO ver si esto tiene que ser Scoped

        return services;
    }
}
