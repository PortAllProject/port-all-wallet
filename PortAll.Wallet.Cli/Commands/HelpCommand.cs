using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using PortAll.Wallet.Args;
using Volo.Abp.DependencyInjection;

namespace PortAll.Wallet.Commands
{
    public class HelpCommand : IConsoleCommand, ITransientDependency
    {
        public const string Name = "help";

        public ILogger<HelpCommand> Logger { get; set; }
        private WalletOptions WalletOptions { get; }
        private IServiceScopeFactory ServiceScopeFactory { get; }

        public HelpCommand(IOptions<WalletOptions> walletOptions,
            IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
            WalletOptions = walletOptions.Value;

            Logger = NullLogger<HelpCommand>.Instance;
        }

        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (string.IsNullOrWhiteSpace(commandLineArgs.Target))
            {
                Logger.LogInformation(GetUsageInfo());
                return Task.CompletedTask;
            }

            if (!WalletOptions.Commands.ContainsKey(commandLineArgs.Target))
            {
                Logger.LogWarning($"There is no command named {commandLineArgs.Target}.");
                Logger.LogInformation(GetUsageInfo());
                return Task.CompletedTask;
            }

            var commandType = WalletOptions.Commands[commandLineArgs.Target];

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var command = (IConsoleCommand) scope.ServiceProvider.GetRequiredService(commandType);
                Logger.LogInformation(command.GetUsageInfo());
            }

            return Task.CompletedTask;
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("    paw <command> <target> [options]");
            sb.AppendLine("");
            sb.AppendLine("Command List:");
            sb.AppendLine("");

            foreach (var command in WalletOptions.Commands.ToArray())
            {
                string shortDescription;

                using (var scope = ServiceScopeFactory.CreateScope())
                {
                    shortDescription = ((IConsoleCommand) scope.ServiceProvider
                        .GetRequiredService(command.Value)).GetShortDescription();
                }

                sb.Append("    > ");
                sb.Append(command.Key);
                sb.Append(string.IsNullOrWhiteSpace(shortDescription) ? "" : ":");
                sb.Append(" ");
                sb.AppendLine(shortDescription);
            }

            sb.AppendLine("");
            sb.AppendLine("To get a detailed help for a command:");
            sb.AppendLine("");
            sb.AppendLine("    paw help <command>");
            sb.AppendLine("");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Show command line help. Write ` paw help <command> `";
        }
    }
}