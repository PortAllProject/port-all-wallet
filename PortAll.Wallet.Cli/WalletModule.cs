using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PortAll.Wallet.Commands;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace PortAll.Wallet
{
    [DependsOn(
        typeof(AbpAutofacModule)
    )]
    public class WalletModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<WalletOptions>(options =>
            {
                options.Commands[HelpCommand.Name] = typeof(HelpCommand);
                
            });
        }
    }
}