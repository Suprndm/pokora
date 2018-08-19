namespace Pokora.IA.Risk
{
    public class CashCriticalityEvaluator
    {
        public double EvaluateCashCriticality(double cash, double bid, double maxBid, double cashInvested, double winableAmount)
        {
            return Randomizer.Instance.Random.Next(1000)/1000d;
        }
    }
}
