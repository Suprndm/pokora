using System;
namespace Pokora.IA.Risk
{
    public class CashCriticalityEvaluator
    {
        public double EvaluateCashCriticality(double cash, double bid, double maxBid, double cashInvested, double winableAmount)
        {
            double bettingRisk = (maxBid + cashInvested) / cash;
            double relativeGain = winableAmount / cash;

            double incentiveToBet = bettingRisk * relativeGain;
            double normalizedIncentive = Math.Exp(incentiveToBet) / (1 + Math.Exp(incentiveToBet));

            return Math.Min(1, bettingRisk);
        }
    }
}
