using System;
using System.Collections.Generic;

namespace Pokora.GameMechanisms
{
    public class Pot
    {
        public Pot()
        {
            Participants = new List<Player>();
        }

        public void Earn(double amount)
        {
            Amount += amount;
        }

        public double Amount { get; private set; }
        public IList<Player> Participants { get; }

        public void DeclareParticipant(Player player)
        {
            if(Participants.Contains(player)) throw new Exception($"{player.Name} is already in the pot");
            Participants.Add(player);
        }

        public void RemoveParticipant(Player player)
        {
            if (Participants.Contains(player))
                Participants.Remove(player);
        }

        public void DeclareWinners(IList<KeyValuePair<Player, CardCombination>> winners)
        {
            foreach (var winner in winners)
            {
                winner.Key.Earn(Amount/ winners.Count);
            }
        }
    }
}
