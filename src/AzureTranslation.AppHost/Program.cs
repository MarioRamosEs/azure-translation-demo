using System.Diagnostics;

using AzureTranslation.AppHost;

using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());

// Load Configuration
if (Debugger.IsAttached)
{
    builder.Configuration.AddJsonFile(@"appsettings.debug.json", optional: true, reloadOnChange: true);
}

builder.Configuration.AddJsonFile($@"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($@"appsettings.{Environment.UserName}.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();

// Configuration values
var appConfigConnectionString = builder.Configuration[Constants.ConnectionStrings.AppConfig];

// Projects
builder.AddProject<Projects.AzureTranslation_API>("azuretranslation-api")
       .WithEnvironment(Constants.ConnectionStrings.AppConfig, appConfigConnectionString)
       ;

builder.AddAzureFunctionsProject<Projects.AzureTranslation_Function>("azuretranslation-function")
       .WithEnvironment(Constants.ConnectionStrings.AppConfig, appConfigConnectionString)
       ;

var app = builder.Build();

try
{
    await app.RunAsync();
}
catch (Exception e)
{
    Console.WriteLine(e.ToString());
}
finally
{
    await app.DisposeAsync();
}

