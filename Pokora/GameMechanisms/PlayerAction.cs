using System;
using System.Collections.Generic;
using System.Text;
using Pokora.GameMechanisms;

namespace Pokora
{
    public class PlayerAction
    {
        public PlayerAction(Player player, PlayerState state, double lower, double highest, double amount = 0)
        {
            State = state;
            Lower = lower;
            Highest = highest;
            Player = player;
            Amount = amount;
        }

        public Player Player { get;}

        public PlayerState State { get; }
        public double Lower { get;  }
        public double Highest { get;}
        public double Amount { get; }
    }
}
