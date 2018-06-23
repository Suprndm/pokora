using System;
using System.Collections.Generic;

namespace Pokora
{
    public class Pot
    {
        public Pot()
        {
            Participants = new List<Player>();
        }

        public double Amount { get; set; }
        public IList<Player> Participants { get; }

        public void DeclareParticipant(Player player)
        {
            if(Participants.Contains(player)) throw new Exception($"{player.Name} is already in the pot");
            Participants.Add(player);
        }
    }
}
