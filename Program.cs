using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using websiteWatcher;
using websiteWatcher.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication()
    .UseWhen<SafeBrowsingMiddleware>(context =>
    {
        return context.FunctionDefinition.Name == "Register";
    });

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Services.AddSingleton<PdfCreatorServices>();
builder.Services.AddSingleton<SafeBrowsingService>();

builder.Build().Run();
