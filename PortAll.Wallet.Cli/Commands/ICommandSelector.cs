using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using PortAll.Wallet.Args;
using Volo.Abp.DependencyInjection;

namespace PortAll.Wallet.Commands
{
    public interface ICommandSelector
    {
        Type Select(CommandLineArgs commandLineArgs);
    }

    public class CommandSelector : ICommandSelector, ITransientDependency
    {
        private WalletOptions Options { get; }

        public CommandSelector(IOptions<WalletOptions> options)
        {
            Options = options.Value;
        }

        public Type Select(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Command.IsNullOrWhiteSpace())
            {
                return typeof(HelpCommand);
            }

            return Options.Commands.GetOrDefault(commandLineArgs.Command)
                   ?? typeof(HelpCommand);
        }
    }
}