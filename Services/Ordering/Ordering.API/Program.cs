using Ordering.Infrastructure.Data;
using Ordering.API.Extensions;
using Ordering.API;
using Serilog;
using Common.Logging;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().MigrateDatabase<OrderContext>((context, services) =>
        {
            var logger = services.GetService<ILogger<OrderContextSeed>>();
            OrderContextSeed.SeedAsync(context, logger).Wait();
        }).Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => 
        { 
            webBuilder.UseStartup<Startup>(); 
        }).UseSerilog(Logging.configureLogger);
}