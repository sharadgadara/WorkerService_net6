using WorkerServiceCore;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(@"E:\Dev\Logfile.txt")
    .CreateLogger();

try
{
    Log.Information("Service starting .....");

    IHost host = Host.CreateDefaultBuilder(args)
        .UseWindowsService()  // it is important to convert worker service to Windows service
        .ConfigureServices(services =>
        {
            services.AddHostedService<Worker>();
        })
        .UseSerilog()
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "There was problem to starting service.");
    throw;
}
finally
{
    Log.CloseAndFlush();
}


