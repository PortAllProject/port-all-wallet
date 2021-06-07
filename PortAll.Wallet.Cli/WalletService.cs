using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PortAll.Wallet.Args;
using PortAll.Wallet.Commands;
using Volo.Abp.DependencyInjection;

namespace PortAll.Wallet
{
    public class WalletService : ITransientDependency
    {
        public ILogger<WalletService> Logger { get; set; }
        private ICommandLineArgumentParser CommandLineArgumentParser { get; }
        private ICommandSelector CommandSelector { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }

        public WalletService(ICommandLineArgumentParser commandLineArgumentParser, ICommandSelector commandSelector,
            IServiceScopeFactory serviceScopeFactory)
        {
            CommandLineArgumentParser = commandLineArgumentParser;
            CommandSelector = commandSelector;
            ServiceScopeFactory = serviceScopeFactory;

            Logger = NullLogger<WalletService>.Instance;
        }

        public async Task RunAsync(string[] args)
        {
            Logger.LogInformation("Port-All Wallet (https://portall.finance)");

            var commandLineArgs = CommandLineArgumentParser.Parse(args);

            try
            {
                var commandType = CommandSelector.Select(commandLineArgs);

                using var scope = ServiceScopeFactory.CreateScope();
                var command = (IConsoleCommand) scope.ServiceProvider.GetRequiredService(commandType);
                await command.ExecuteAsync(commandLineArgs);
            }
            catch (WalletUsageException usageException)
            {
                Logger.LogWarning(usageException.Message);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}