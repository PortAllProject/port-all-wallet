using System;

namespace PortAll.Wallet
{
    public class WalletUsageException : Exception
    {
        public WalletUsageException(string message)
            : base(message)
        {

        }

        public WalletUsageException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
