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
            var orderedCards = cards.GroupBy(card=>card.Value).Select(group=>group.First()).OrderByDescending(card => card.Value);
            int successiveCards = 1;
            double combinationHeight = 0;
            IList<Card> combination = new List<Card>();

            if (hasColor)
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
            else
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
                combination.Any(card => card.Value == 2))
            {
                hasStraight = true;
                combinationHeight = combination.Max(card => card.Value);
                combination.Add(orderedCards.FirstOrDefault(card=>card.Value==14));
            }
            else if (successiveCards >= 5)
            {
                hasStraight = true;
                combinationHeight = combination.Max(card => card.Value);
                combination = combination.OrderByDescending(card => card.Value).Take(5).ToList();
            }

            if (hasColor && hasStraight && combination.All(card=> colorGroup.Contains(card)))
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

        
                if (quads != null)
                {
                    combinationType = CombinationType.FourOfAKind;
                    combinationHeight = quads.First().Value;
                    combination = quads.ToList();
                    var remain = cards.Where(card => !combination.Contains(card)).OrderByDescending(card => card.Value).First();
                    combination.Add(remain);
                    combinationHeight += 0.07 * remain.Value;
                } else
                {
                    var trips = valueGroups.Where(group => group.Count() == 3);
                    IGrouping<int, Card> bestTrips = null;
                    if (trips.Count() == 2)
                    {
                        bestTrips = trips.OrderByDescending(trip => trip.First().Value).First();
                        valueGroups.Remove(bestTrips);
                    }
                    else if(trips.Count() == 1)
                    {
                        bestTrips = trips.First();
                        valueGroups.Remove(bestTrips);
                    }

                    var pairs = valueGroups.Where(group => group.Count() == 2 || group.Count() == 3);

                    if (bestTrips != null && pairs.Any())
                    {
                        var bestPair = pairs.OrderByDescending(pair => pair.First().Value).First();
                        combination = bestTrips.Concat(bestPair.Take(2)).ToList();
                        combinationHeight = bestTrips.First().Value + 0.07 * bestPair.First().Value;
                        combinationType = CombinationType.FullHouse;
                    }
                    else if(hasColor)
                    {
                        if (colorGroup.Count() >= 5)
                        {
                         

                            combination = colorGroup.ToList().OrderByDescending(card => card.Value).Take(5).ToList();
                            combinationType = CombinationType.Flush;
                        }
                    } else if (hasStraight)
                    {
                        combinationType = CombinationType.Straight;
                        combination = combination.OrderByDescending(card => card.Value).Take(5).ToList();
                        combinationHeight = combination.First().Value;
                    } else if (bestTrips != null)
                    {
                        combinationType = CombinationType.ThreeOfAKind;
                        combination = bestTrips.ToList();
                        var remains = cards.Where(card => !combination.Contains(card)).OrderByDescending(card => card.Value).Take(2).ToList();
                        combinationHeight = bestTrips.First().Value + remains[0].Value * 0.7 +
                                            remains[1].Value * 0.7 * 0.7;
                        combination.Add(remains[0]);
                        combination.Add(remains[1]);
                    } else if (pairs.Count() >= 2)
                    {
                        combination.Clear();
                        var bestPairs = pairs.OrderByDescending(pair => pair.First().Value).Take(2).ToList();
                        combination = combination.Concat(bestPairs[0].ToList()).ToList();
                        combination = combination.Concat(bestPairs[1].ToList()).ToList();

                        var remain = cards.Where(card => !combination.Contains(card)).OrderByDescending(card => card.Value).First();

                        combination.Add(remain);

                        combinationHeight = bestPairs[0].First().Value + bestPairs[1].First().Value*0.07 +
                                            remain.Value * 0.07 * 0.07;
                        combinationType = CombinationType.TwoPair;
                    } else if (pairs.Count() == 1)
                    {
                        var bestPair = pairs.OrderByDescending(pair => pair.First().Value).First();
                        combination = bestPair.ToList();
                        var remains = cards.Where(card => !combination.Contains(card)).OrderByDescending(card => card.Value).Take(3).ToList();
                        combination.Add(remains[0]);
                        combination.Add(remains[1]);
                        combination.Add(remains[2]);
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

            for (int i = 0; i < 5; i++)
            {
                var cardValue = combination[i].Value;
                if (cardValue == 14 && (combinationType == CombinationType.StraightFlush ||
                    combinationType == CombinationType.Straight) && combination.All(card => card.Value != 13))
                {
                    cardValue = 1;
                }

                combinationHeight += cardValue * Math.Pow(0.07, i);
            }

            var score = combinationHeight * Math.Pow(14, (int) combinationType);

            return new CardCombination(combinationType, combination.ToList(), score);
        }
    }
}
