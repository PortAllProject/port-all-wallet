using System;
using System.Threading.Tasks;
using PortAll.Wallet.Args;
using Volo.Abp.DependencyInjection;

namespace PortAll.Wallet.Commands
{
    public class GenerateCommand : IConsoleCommand, ITransientDependency
    {
        public const string Name = "generate";

        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Target == null)
            {
                throw new WalletUsageException(
                    "Module name is missing!" +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }
            
            return Task.CompletedTask;
        }

        public string GetUsageInfo()
        {
            throw new System.NotImplementedException();
        }

        public string GetShortDescription()
        {
            throw new System.NotImplementedException();
        }
    }
}