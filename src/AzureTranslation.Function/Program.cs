using Azure.Identity;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = new HostBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables();

        var tempConfig = config.Build();
        var appConfigConnectionString = tempConfig.GetConnectionString("AppConfig");

        if (!string.IsNullOrWhiteSpace(appConfigConnectionString))
        {
            var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            var label = $"{context.HostingEnvironment.EnvironmentName}-{assemblyName}";

            var azureCredentials = new ChainedTokenCredential(new DefaultAzureCredential(), new EnvironmentCredential());

            config.AddAzureAppConfiguration(options =>
            {
                options.Connect(appConfigConnectionString)
                       .ConfigureKeyVault(keyVault =>
                       {
                           keyVault.SetCredential(azureCredentials);
                       })
                       .Select(KeyFilter.Any, LabelFilter.Null)
                       .Select(KeyFilter.Any, label)
                       .ConfigureRefresh(refreshOptions =>
                       {
                           refreshOptions.Register("Sentinel", label, refreshAll: true);
                           refreshOptions.SetRefreshInterval(TimeSpan.FromSeconds(86400));
                       });
            });
        }
    })
    .ConfigureServices((context, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.AddAzureAppConfiguration();
    })
    .ConfigureFunctionsWebApplication()
    ;

var host = builder.Build();
host.Run();