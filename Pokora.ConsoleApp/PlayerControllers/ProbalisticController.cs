﻿using System;
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
        private readonly bool _useEllipse;
        private IDictionary<PlayerState, EllipticArea> _areas;

        public ProbalisticController(IDictionary<PlayerState, EllipticArea> areas, bool useEllipse = false)
        {
            _areas = areas;
            _useEllipse = useEllipse;
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

                //Console.WriteLine($"quality {quality} , critic {cashCriticality}");
           
                var action = _decisionEvaluator.Decide(actions, quality, cashCriticality, _useEllipse, _areas);
                //if (Player.Name == "Ratchet") Console.WriteLine($"{Player.Name}: {action.State}");
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
