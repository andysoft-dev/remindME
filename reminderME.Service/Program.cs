using reminderMEService;


IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .UseSystemd() 
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
