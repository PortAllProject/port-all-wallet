using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Volo.Abp;

namespace PortAll.Wallet
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
#if DEBUG
                .MinimumLevel.Override("PortAll.Wallet", LogEventLevel.Debug)
#else
                .MinimumLevel.Override("PortAll.Wallet", LogEventLevel.Information)
#endif
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File($"Logs/PortAllWallet-{DateTime.UtcNow:yyyy-MM-dd}.logs"))
                .WriteTo.Console()
                .CreateLogger();

            using var application = AbpApplicationFactory.Create<WalletModule>(
                options =>
                {
                    options.UseAutofac();
                    options.Services.AddLogging(c => c.AddSerilog());
                });
            application.Initialize();

            await application.ServiceProvider
                .GetRequiredService<WalletService>()
                .RunAsync(args);

            application.Shutdown();

            Log.CloseAndFlush();
        }
    }
}