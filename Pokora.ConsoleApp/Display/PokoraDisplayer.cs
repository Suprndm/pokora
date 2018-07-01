using System;
using System.Collections.Generic;
using System.Linq;
using Pokora.Cards;
using Pokora.GameMechanisms;

namespace Pokora.ConsoleApp.Display
{
    public class PokoraDisplayer
    {
        private Table _table;

        private IList<string> _events;

        public void SetupDisplay(Table table)
        {
            _table = table;
            _events = new List<string>();
            for (int i = 0; i < 40; i++)
            {
                _events.Add("");
            }
        }

        public void PushEvent(string eventMessage)
        {
           _events.RemoveAt(0);
            _events.Add(eventMessage);

            UpdateDisplay();
        }

        public void OnNewTurn(Player player, IList<PlayerAction> actions)
        {
            Console.Write($"{player.Name} - your actions : {string.Join(" | ", actions.Select(a=>$"{a.State} {a.Lower} - {a.Highest}"))} \n your choice :");
        }

        public void UpdateDisplay()
        {
            Console.Clear();
            // Draw players
            foreach (var player in _table.Players)
            {
                DrawPlayer(player);
            }

            DrawTable(_table);

            DrawEvents();
        }

        private void DrawPlayer(Player player)
        {
            string playerHandString = "";
            if (player.Hand != null)
            {
                playerHandString =
                    $"{player.Hand.Card1.BuildStringFromCard()} {player.Hand.Card2.BuildStringFromCard()}";
            }
            Console.WriteLine($"{player.Name}:{player.Cash} - {playerHandString} ->{player.Bid} - {player.State}");

        }

        private void DrawTable(Table table)
        {
            if (table.CurrentGame != null)
            {
                Console.WriteLine($"Cards : {CardsBuilder.BuildStringFromCards(_table.CurrentGame.Cards.Cards)}");

                if (table.CurrentGame.Pots != null && table.CurrentGame.Pots.Count > 0)
                {
                    foreach (var pot in table.CurrentGame.Pots)
                    {
                        DrawPot(pot);
                    }
                }
                else
                {
                    Console.WriteLine("Pot : 0");
                }
            }
        }

        private void DrawPot(Pot pot)
        {
            Console.WriteLine($"Pot with {string.Join(" and ", pot.Participants.Select(par=>par.Name))} : {pot.Amount}");
        }

        private void DrawEvents()
        {
            Console.WriteLine("Events:");
            foreach (var e in _events)
            {
                Console.WriteLine(e);
            }
        }
    }
}
