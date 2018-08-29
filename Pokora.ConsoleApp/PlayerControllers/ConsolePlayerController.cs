using System;
using System.Collections.Generic;
using System.Linq;
using Pokora.GameMechanisms;

namespace Pokora.ConsoleApp.PlayerControllers
{
    public class ConsolePlayerController : BaseController
    {
        public override PlayerAction Play(IList<PlayerAction> actions)
        {
            var action = InterpreteAction(actions);
            return action;
        }

        private PlayerAction InterpreteAction(IList<PlayerAction> actions)
        {
            PlayerAction playerAction = null;
            while (playerAction == null)
            {
                var commandString = Console.ReadLine();

                var split = commandString.Split(" ");
                if (split.Length == 1)
                {
                    if (split[0] == "fold" && actions.Any(a => a.State == PlayerState.Fold))
                        playerAction = new PlayerAction(Player, PlayerState.Fold, 0, 0);
                    if (split[0] == "check" && actions.Any(a => a.State == PlayerState.Check))
                        playerAction = new PlayerAction(Player, PlayerState.Check, 0, 0);
                    if (split[0] == "call" && actions.Any(a => a.State == PlayerState.Call))
                        playerAction = new PlayerAction(Player, PlayerState.Call, 0, 0, actions.Single(action => action.State == PlayerState.Call).Amount);
                    if (split[0] == "allin" && actions.Any(a => a.State == PlayerState.AllIn))
                        playerAction = new PlayerAction(Player, PlayerState.AllIn, 0, 0, actions.Single(action => action.State == PlayerState.AllIn).Amount);
                }
                else
                {
                    var amount = int.Parse(split[1]);
                    if (split[0] == "raise" && actions.Any(a => a.State == PlayerState.Raise))
                    {
                        playerAction = new PlayerAction(Player, PlayerState.Raise, 0, 0, amount);
                    }
                    if (split[0] == "bet" && actions.Any(a => a.State == PlayerState.Bet))
                    {
                        playerAction = new PlayerAction(Player, PlayerState.Bet, 0, 0, amount);
                    }
                }
            }

            return playerAction;
        }
    }
}
