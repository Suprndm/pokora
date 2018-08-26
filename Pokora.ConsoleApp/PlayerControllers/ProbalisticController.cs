using System;
using System.Collections.Generic;
using System.Linq;
using Pokora.GameMechanisms;
using Pokora.IA;
using Pokora.IA.Decision;
using Pokora.IA.Risk;

namespace Pokora.ConsoleApp.PlayerControllers
{
    class ProbalisticController : BaseController
    {
        private readonly QualityEvaluator _qualityEvaluator;
        private readonly DecisionEvaluator _decisionEvaluator;
        private readonly CashCriticalityEvaluator _cashCriticalityEvaluator;

        public ProbalisticController(IDictionary<PlayerState, EllipticArea> areas)
        {
            _qualityEvaluator = new QualityEvaluator();
            _decisionEvaluator = new DecisionEvaluator(areas);
            _cashCriticalityEvaluator = new CashCriticalityEvaluator();
        }

        public override void NotifyTurn()
        {
            try
            {
                var totalPlayerInvestedInPots = Table.CurrentGame.Pots
                    .Where(pot => pot.Participants.Contains(Player))
                    .Sum(pot => pot.Amount / pot.Participants.Count);

                var winableAmount = Table.CurrentGame.Pots
                    .Where(pot => pot.Participants.Contains(Player))
                    .Sum(pot => pot.Amount);

                var maxBid = Table.Players.Max(player => player.Bid);

                var quality = _qualityEvaluator.EvalQualityScore(Player.Hand, Table.CurrentGame.Cards.Cards.ToList());

                var cashCriticality = _cashCriticalityEvaluator.EvaluateCashCriticality(Player.Cash, Player.Bid, maxBid, totalPlayerInvestedInPots, winableAmount);

                //Console.WriteLine($"quality {quality} , critic {cashCriticality}");
                if (Player.Name == "Corail")
                {
                    var test = 0;
                }
                var action = _decisionEvaluator.Decide(AvailableActions, quality, cashCriticality);
                //if (Player.Name == "Ratchet") Console.WriteLine($"{Player.Name}: {action.State}");
                SendAction(action);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
