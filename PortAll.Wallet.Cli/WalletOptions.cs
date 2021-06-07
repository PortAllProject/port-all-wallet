using System;
using System.Collections.Generic;

namespace PortAll.Wallet
{
    public class WalletOptions
    {
        public Dictionary<string, Type> Commands { get; }

        public WalletOptions()
        {
            Commands = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        }
    }
}