using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokora.Poker
{
    public class CombinationEvaluator
    {
        public CardCombination EvaluateCards(IReadOnlyCollection<Card> cards)
        {
            var combinationType = CombinationType.HighCard;

            var colors = cards.GroupBy(card => card.Color);
            var colorGroup = colors.FirstOrDefault(group => group.Count() >= 5);
            bool hasColor = colorGroup != null;
            bool hasStraight = false;
            var orderedCards = cards.GroupBy(card => card.Value).Select(group => group.First()).OrderByDescending(card => card.Value);
            int successiveCards = 1;
            double combinationHeight = 0;
            IList<Card> combination = new List<Card>();
            if (hasColor && cards.Count >= 5)
            {
                var orderedColorCards = colorGroup.OrderByDescending(card => card.Value);
                Card previousCard = orderedColorCards.First();
                combination = new List<Card>() { previousCard };
                foreach (var orderedCard in orderedColorCards.Skip(1))
                {
                    if (previousCard.Value == orderedCard.Value + 1)
                    {
                        successiveCards++;
                        combination.Add(orderedCard);
                    }
                    else if (successiveCards < 5)
                    {
                        successiveCards = 1;
                        combination.Clear();
                        combination.Add(orderedCard);
                    }

                    previousCard = orderedCard;
                }
            }
            else if (cards.Count >= 5)
            {
                Card previousCard = orderedCards.First();
                combination = new List<Card>() { previousCard };
                foreach (var orderedCard in orderedCards.Skip(1))
                {
                    if (previousCard.Value == orderedCard.Value + 1)
                    {
                        successiveCards++;
                        combination.Add(orderedCard);
                    }
                    else if (successiveCards < 5)
                    {
                        successiveCards = 1;
                        combination.Clear();
                        combination.Add(orderedCard);
                    }

                    previousCard = orderedCard;
                }
            }




            if (successiveCards == 4 && orderedCards.Any(card => card.Value == 14) &&
                combination.Any(card => card.Value == 2) && cards.Count >= 5)
            {
                hasStraight = true;
                combinationHeight = combination.Max(card => card.Value);
                combination.Add(orderedCards.FirstOrDefault(card => card.Value == 14));
            }
            else if (successiveCards >= 5 && cards.Count >= 5)
            {
                hasStraight = true;
                combinationHeight = combination.Max(card => card.Value);
                combination = combination.OrderByDescending(card => card.Value).Take(5).ToList();
            }

            if (hasColor && hasStraight && combination.All(card => colorGroup.Contains(card)) && cards.Count >= 5)
            {
                if (combinationHeight == 14)
                {
                    combinationType = CombinationType.RoyalFlush;
                }
                else
                {
                    combinationType = CombinationType.StraightFlush;
                }
            }
            else
            {
                var valueGroups = cards.GroupBy(card => card.Value).ToList();
                var quads = valueGroups.FirstOrDefault(group => group.Count() == 4);

                if (quads != null && cards.Count >= 4)
                {
                    combinationType = CombinationType.FourOfAKind;
                    combination = quads.ToList();
                    if (cards.Any(card => !combination.Contains(card)))
                    {
                        var remain = cards.Where(card => !combination.Contains(card)).OrderByDescending(card => card.Value).First();
                        combination.Add(remain);
                    }
                }
                else
                {
                    var trips = valueGroups.Where(group => group.Count() == 3);
                    IGrouping<int, Card> bestTrips = null;
                    if (trips.Count() == 2)
                    {
                        bestTrips = trips.OrderByDescending(trip => trip.First().Value).First();
                        valueGroups.Remove(bestTrips);
                    }
                    else if (trips.Count() == 1)
                    {
                        bestTrips = trips.First();
                        valueGroups.Remove(bestTrips);
                    }

                    var pairs = valueGroups.Where(group => group.Count() == 2 || group.Count() == 3);

                    if (bestTrips != null && pairs.Any() && cards.Count >= 5)
                    {
                        var bestPair = pairs.OrderByDescending(pair => pair.First().Value).First();
                        combination = bestTrips.Concat(bestPair.Take(2)).ToList();
                        combinationType = CombinationType.FullHouse;
                    }
                    else if (hasColor)
                    {
                        if (colorGroup.Count() >= 5)
                        {


                            combination = colorGroup.ToList().OrderByDescending(card => card.Value).Take(5).ToList();
                            combinationType = CombinationType.Flush;
                        }
                    }
                    else if (hasStraight && cards.Count >= 5)
                    {
                        combinationType = CombinationType.Straight;
                        combination = combination.OrderByDescending(card => card.Value).Take(5).ToList();
                    }
                    else if (bestTrips != null && cards.Count >= 3)
                    {
                        combinationType = CombinationType.ThreeOfAKind;
                        combination = bestTrips.ToList();
                        var remains = cards.Where(card => !combination.Contains(card)).OrderByDescending(card => card.Value).Take(2).ToList();


                        for (int i = 0; i < remains.Count; i++)
                        {
                            combination.Add(remains[i]);
                        }
                    }
                    else if (pairs.Count() >= 2 && cards.Count >= 4)
                    {
                        combination.Clear();
                        var bestPairs = pairs.OrderByDescending(pair => pair.First().Value).Take(2).ToList();
                        combination = combination.Concat(bestPairs[0].ToList()).ToList();
                        combination = combination.Concat(bestPairs[1].ToList()).ToList();

                        if (cards.Any(card => !combination.Contains(card)))
                        {
                            var remain = cards.Where(card => !combination.Contains(card)).OrderByDescending(card => card.Value).First();
                            combination.Add(remain);
                        }

                        combinationType = CombinationType.TwoPair;
                    }
                    else if (pairs.Count() == 1 && cards.Count >= 2)
                    {
                        var bestPair = pairs.OrderByDescending(pair => pair.First().Value).First();
                        combination = bestPair.ToList();
                        var remains = cards.Where(card => !combination.Contains(card)).OrderByDescending(card => card.Value).Take(3).ToList();

                        for (int i = 0; i < remains.Count; i++)
                        {
                            combination.Add(remains[i]);
                        }

                        combinationType = CombinationType.OnePair;
                    }
                    else
                    {
                        var bestHighCards = cards.OrderByDescending(card => card.Value).Take(5);
                        combination = bestHighCards.ToList();
                        combinationType = CombinationType.HighCard;
                    }

                }
            }

            combinationHeight = 0;

            for (int i = 0; i < Math.Min(5, combination.Count); i++)
            {
                var cardValue = combination[i].Value;
                if (cardValue == 14 && (combinationType == CombinationType.StraightFlush ||
                    combinationType == CombinationType.Straight) && combination.All(card => card.Value != 13))
                {
                    cardValue = 1;
                }

                combinationHeight += cardValue * Math.Pow(0.07, i);
            }

            var score = combinationHeight * Math.Pow(14, (int)combinationType);

            return new CardCombination(combinationType, combination.ToList(), score);
        }
    }
}
