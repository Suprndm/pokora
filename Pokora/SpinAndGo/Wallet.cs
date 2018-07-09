using System;
using System.Collections.Generic;
using System.Text;

namespace Pokora.SpinAndGo
{
    public class Wallet
    {
        public double Amount { get; private set; }

        public double Take(double amountToTake)
        {
            if (Amount < amountToTake)
                throw new Exception($"Insufficient amountToTake {amountToTake} on wallet {Amount}");

            Amount += -amountToTake;

            return amountToTake;
        }

        public void Receive(double amountToReceive)
        {
            Amount += amountToReceive;
        }
    }
}
