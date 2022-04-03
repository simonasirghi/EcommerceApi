using ImageProcessorWorkerService;
using ImageProcessorWorkerService.Storage;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.Configure<AzureBlobStorageKeys>(configuration.GetSection("AzureKeys"));
        services.AddHostedService<Worker>();
        
    })
    .Build();

await host.RunAsync();
