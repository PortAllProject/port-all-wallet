using System.Threading.Tasks;
using PortAll.Wallet.Args;

namespace PortAll.Wallet.Commands
{
    public interface IConsoleCommand
    {
        Task ExecuteAsync(CommandLineArgs commandLineArgs);

        string GetUsageInfo();

        string GetShortDescription();
    }
}