using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pokora.ConsoleApp.Display;
using Pokora.GameMechanisms;

namespace Pokora.ConsoleApp.PlayerControllers
{
    public class ConsolePlayerController : IPlayerController
    {
        private readonly PokoraDisplayer _displayer;
        private Player _player;
        private IList<PlayerAction> _availableActions;
        public ConsolePlayerController(PokoraDisplayer displayer)
        {
            _displayer = displayer;
        }

        public void LinkPlayer(Player player)
        {
            _player = player;
        }


        public event Action<PlayerAction> ActionReceived;

        public void NotifyTurn()
        {
            _displayer.OnNewTurn(_player, _availableActions);
            var action = InterpreteAction();
            ActionReceived?.Invoke(action);
        }

        private PlayerAction InterpreteAction()
        {
            PlayerAction playerAction = null;
            while (playerAction == null)
            {
                var commandString = Console.ReadLine();

                var split = commandString.Split(" ");
                if (split.Length == 1)
                {
                    if (split[0] == "fold")
                        playerAction = new PlayerAction(_player, PlayerState.Fold, 0, 0);
                    if (split[0] == "check")
                        playerAction = new PlayerAction(_player, PlayerState.Check, 0, 0);
                    if (split[0] == "call")
                        playerAction = new PlayerAction(_player, PlayerState.Call, 0, 0, _availableActions.Single(action => action.State == PlayerState.Call).Amount);
                    if (split[0] == "allin")
                        playerAction = new PlayerAction(_player, PlayerState.AllIn, 0, 0, _availableActions.Single(action => action.State == PlayerState.AllIn).Amount);
                }
                else
                {
                    var amount = int.Parse(split[1]);
                    if (split[0] == "raise")
                    {
                        playerAction = new PlayerAction(_player, PlayerState.Raise, 0, 0, amount);
                    }
                    if (split[0] == "bet")
                    {
                        playerAction = new PlayerAction(_player, PlayerState.Bet, 0, 0, amount);
                    }
                }
            }

            return playerAction;
        }

        public void SendAvailableActions(IList<PlayerAction> actions)
        {
            _availableActions = actions;
        }
    }
}
