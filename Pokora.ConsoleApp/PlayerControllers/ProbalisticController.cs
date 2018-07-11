using System.Linq;
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

        public ProbalisticController()
        {
            _qualityEvaluator = new QualityEvaluator();
            _decisionEvaluator = new DecisionEvaluator();
            _cashCriticalityEvaluator = new CashCriticalityEvaluator();
        }

        public override void NotifyTurn()
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

            var action = _decisionEvaluator.Decide(AvailableActions, quality, cashCriticality);

            SendAction(action);
        }
    }
}
