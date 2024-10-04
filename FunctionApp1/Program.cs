using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        
        services.AddApplicationInsightsTelemetryWorkerService();

        // ServiceBus integration
        services.AddAzureClients(builder =>
        {
            builder.AddServiceBusClient(context.Configuration["ServiceBusConnectionString"]);
        });

        
    })
    .Build();

await host.RunAsync(); 
