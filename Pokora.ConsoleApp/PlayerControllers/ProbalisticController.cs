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
        private readonly RiskEvaluator _riskEvaluator;

        public ProbalisticController()
        {
            _qualityEvaluator = new QualityEvaluator();
            _decisionEvaluator = new DecisionEvaluator();
            _riskEvaluator = new RiskEvaluator();
        }

        public override void NotifyTurn()
        {
            var maxBid = Table.Players.Max(player => player.Bid);

            var quality = _qualityEvaluator.EvalQualityScore(Player.Hand, Table.CurrentGame.Cards.Cards.ToList());

            var risk = _riskEvaluator.EvaluateRisk(Player.Cash, Player.Bid, maxBid);

            var action = _decisionEvaluator.Decide(AvailableActions, quality, risk);

            SendAction(action);
        }
    }
}
