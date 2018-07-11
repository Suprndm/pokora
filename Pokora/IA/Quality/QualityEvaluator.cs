using System;
using System.Collections.Generic;
using System.Linq;
using Pokora.GameMechanisms;
using Pokora.IA.Quality;
using Pokora.Poker;

namespace Pokora.IA
{
    public class QualityEvaluator : IQualityEvaluator
    {
        private const double maxCombinationScore = 4330955887260.6328;
        private const double minCombinationCardsScore = 103.18947827999999;

        private const double maxHandScore = 2936;
        private const double minHandScore = 43.96;

        private readonly IList<ScoreRange> _scoreRanges = new List<ScoreRange>
        {
            new ScoreRange(2, 1.64, 3.46),
            new ScoreRange(3, 1.77, 5.76),
            new ScoreRange(4, 1.87, 10.34),
            new ScoreRange(5, 2.01, 12.63),
        };

        private readonly CombinationEvaluator _combinationEvaluator;

        public QualityEvaluator()
        {
            _combinationEvaluator = new CombinationEvaluator();
        }

        public double EvalQualityScore(PlayerHand playerHand, IList<Card> tableCards)
        {
            var handQuality = ComputeQualityOfCards(new List<Card> { playerHand.Card1, playerHand.Card2 });

            if (tableCards.Count == 0)
            {
                return handQuality;
            }
            else
            {
                var tableCardsQuality = ComputeQualityOfCards(tableCards);
                var set = tableCards.ToList();
                set.Add(playerHand.Card1);
                set.Add(playerHand.Card2);

                var setQuality = ComputeQualityOfCards(set);

                var broughtQuality = setQuality - tableCardsQuality;
                return broughtQuality;
            }
        }


        private double ComputeQualityOfCards(IList<Card> cards)
        {
            var cardsCombination = _combinationEvaluator.EvaluateCards(cards.ToList());
            var cardsScore = Math.Log10(cardsCombination.Score);

            var scoraRange = _scoreRanges.Single(range => range.CardsCount == cardsCombination.Cards.Count);
            var quality = Math.Round((cardsScore - scoraRange.Min) /(scoraRange.Max- scoraRange.Min), 3);

            return quality;
        }
    }
}
