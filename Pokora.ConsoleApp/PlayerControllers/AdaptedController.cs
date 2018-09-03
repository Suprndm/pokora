using System;
using System.Collections.Generic;
using System.Linq;
using Pokora.GameMechanisms;
using Pokora.IA;
using Pokora.IA.Decision;
using Pokora.IA.Risk;

namespace Pokora.ConsoleApp.PlayerControllers
{
    class AdaptedController : BaseController
    {
        private readonly QualityEvaluator _qualityEvaluator;
        private readonly DecisionEvaluator _decisionEvaluator;
        private readonly CashCriticalityEvaluator _cashCriticalityEvaluator;
        private readonly AgressivityEvaluator _agressivityEvaluator;
        private readonly IList<TableResult> _ias;

        public AdaptedController(IList<TableResult> ias)
        {
            _ias = ias;
            _agressivityEvaluator = new AgressivityEvaluator();
            _qualityEvaluator = new QualityEvaluator();
            _decisionEvaluator = new DecisionEvaluator();
            _cashCriticalityEvaluator = new CashCriticalityEvaluator();
        }

        public override PlayerAction Play(IList<PlayerAction> actions)
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

                var agressivity = _agressivityEvaluator.EvaluateAggressivity(Table.Players.Where(p => p != Player).ToList());
                TableResult iaToUse = _ias[1];
                //Console.WriteLine(agressivity);
                if (agressivity > 0.3)
                {
                    iaToUse = _ias[1];
                }
                else
                {
                    iaToUse = _ias[0];
                }

                //Console.WriteLine(agressivity);


                var action = _decisionEvaluator.Decide(actions, quality, cashCriticality, true, iaToUse.EllipticAreas);
                return (action);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
