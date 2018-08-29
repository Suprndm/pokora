using System;
using System.Collections.Generic;
using System.Linq;
using Pokora.Cards;
using Pokora.GameMechanisms;
using Pokora.SpinAndGo;

namespace Pokora.ConsoleApp.Display
{
    public class PokoraDisplayer
    {
        private SpinAndGoGame _spinAndGoGame;
        private bool _consoleDisplayEnabled = true;
        private IList<string> _events;
        private bool _isBusy;
        private bool _redrawLater;

        public bool IsDisabled { get; set; }

        public void SetupDisplay(IList<User> users)
        {
            _events = new List<string>();
            for (int i = 0; i < 30; i++)
            {
                _events.Add("");
            }
        }

        public void SetupGameDisplay(SpinAndGoGame spinAndGoGame)
        {
            _spinAndGoGame = spinAndGoGame;
        }

        public bool IsSoloGame { get; set; }

        public void PushEvent(string eventMessage)
        {
            if (_events == null || IsDisabled) return;
            _events.RemoveAt(0);
            _events.Add(eventMessage);

            UpdateDisplay();
        }

        public void OnNewTurn(Player player, IList<PlayerAction> actions)
        {
            Console.Write($"{player.Name} - your actions : {string.Join(" | ", actions.Select(a => $"{a.State} {a.Lower} - {a.Highest}"))} \n your choice :");
        }

        public void SetConsoleDisplayState(bool isEnable)
        {
            _consoleDisplayEnabled = isEnable;
        }

        public void UpdateDisplay()
        {
            if (IsDisabled) return;

            if (_isBusy)
            {
                _redrawLater = true;
                return;
            }

            _isBusy = true;

            if (_consoleDisplayEnabled)
                Console.Clear();

            // Draw Game
            DrawGame();

            // Draw players
            foreach (var player in _spinAndGoGame.Table.Players)
            {
                DrawPlayer(player);
            }

            DrawTable(_spinAndGoGame.Table);

            DrawEvents();

            _isBusy = false;

            if (_redrawLater)
            {
                _redrawLater = false;
                UpdateDisplay();
            }
        }

        private void DrawGame()
        {
            Draw($"Spin&Go n°{_spinAndGoGame.GameCount} - Fee : {_spinAndGoGame.Fee}  - Prize : {_spinAndGoGame.Prize}");
            Draw(string.Join(" | ", _spinAndGoGame.Users.Select(user => $"{user.Name} ({user.Cash})")));
            Draw("-----------------------------------");
        }

        private void DrawPlayer(Player player)
        {
            string playerHandString = "";
            if (player.Hand != null )
            {
                playerHandString =
                    $"{player.Hand.Card1.BuildStringFromCard()} {player.Hand.Card2.BuildStringFromCard()}";

                if (IsSoloGame && player.Name != "Corail")
                    playerHandString = "XXX";
            }
            Draw($"{player.Name}:{player.Cash} - {playerHandString} ->{player.Bid} - {player.State}");

        }

        private void DrawTable(Table table)
        {
            if (table.CurrentGame != null)
            {
                Draw($"Cards : {CardsBuilder.BuildStringFromCards(_spinAndGoGame.Table.CurrentGame.Cards.Cards)}");

                if (table.CurrentGame.Pots != null && table.CurrentGame.Pots.Count > 0)
                {
                    foreach (var pot in table.CurrentGame.Pots)
                    {
                        DrawPot(pot);
                    }
                }
                else
                {
                    Draw("Pot : 0");
                }
            }
        }

        private void DrawPot(Pot pot)
        {
            Draw($"Pot with {string.Join(" and ", pot.Participants.Select(par => par.Name))} : {pot.Amount}");
        }

        private void DrawEvents()
        {
            Draw("Events:");
            foreach (var e in _events)
            {
                Draw(e);
            }
        }

        private void Draw(string message)
        {
            if (_consoleDisplayEnabled)
            {
                Console.WriteLine(message);
            }
        }
    }
}
