using System.Diagnostics;

using Asp.Versioning;

using Azure.Identity;

using AzureTranslation.Api;

using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var assemblyName = typeof(Program).Assembly.GetName().Name;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory(),
});

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());

/* Load Configuration */

if (Debugger.IsAttached)
{
    builder.Configuration.AddJsonFile(@"appsettings.debug.json", optional: true, reloadOnChange: true);
}

builder.Configuration.AddJsonFile($@"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($@"appsettings.{Environment.UserName}.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();

// Load configuration from Azure App Configuration, and set Key Vault client for secrets...
var appConfigurationConnectionString = builder.Configuration.GetConnectionString(@"AppConfig");

var useAppConfiguration = !string.IsNullOrWhiteSpace(appConfigurationConnectionString);

if (useAppConfiguration)
{
    var azureCredentials = new ChainedTokenCredential(new DefaultAzureCredential(), new EnvironmentCredential());

    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        var label = $@"{builder.Environment.EnvironmentName}-{assemblyName}";

        options.Connect(appConfigurationConnectionString)
               .ConfigureKeyVault(keyVault =>
               {
                   keyVault.SetCredential(azureCredentials);
               })
               .Select(KeyFilter.Any, LabelFilter.Null) // Load configuration values with no label
               .Select(KeyFilter.Any, label) // Override with any configuration values specific to current application
               .ConfigureRefresh(refreshOptions =>
               {
                   refreshOptions.Register(@"Sentinel", label, refreshAll: true);
                   refreshOptions.SetRefreshInterval(TimeSpan.FromSeconds(86400)); // Default is 30 seconds (https://learn.microsoft.com/en-us/azure/azure-app-configuration/enable-dynamic-configuration-aspnet-core#reload-data-from-app-configuration), set this to a day.
               })
               ;
    }, optional: false);

    builder.Services.AddAzureAppConfiguration();
}

/* Logging Configuration */

var applicationInsightsConnectionString = builder.Configuration.GetConnectionString(Constants.ConnectionStrings.ApplicationInsights);

builder.Logging.AddApplicationInsights((telemetryConfiguration) => telemetryConfiguration.ConnectionString = applicationInsightsConnectionString, (_) => { })
               .AddConsole();

if (Debugger.IsAttached)
{
    builder.Logging.AddDebug();
}

builder.AddServiceDefaults();

builder.AddAzureServiceBusClient("ServiceBus");

builder.Services.AddHealthChecks();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddApiVersioning(options =>
                 {
                     options.AssumeDefaultVersionWhenUnspecified = true;
                     options.ReportApiVersions = true;
                     options.ApiVersionReader = new UrlSegmentApiVersionReader();
                     options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
                 });

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
