using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        // Register Application Insights (for telemetry and monitoring)
        services.AddApplicationInsightsTelemetryWorkerService();

        // Register the Service Bus client for Azure Service Bus integration
        services.AddAzureClients(builder =>
        {
            builder.AddServiceBusClient(context.Configuration["ServiceBusConnectionString"]);
        });

        // Add any other services or configurations here
    })
    .Build();

await host.RunAsync(); // Ensure this is awaited
