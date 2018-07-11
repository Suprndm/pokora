using NSubstitute;
using NUnit.Framework;
using Pokora.GameMechanisms;
using Pokora.IA.Risk;

namespace Pokora.Tests
{
    public class CashCriticalityEvaluatorTests
    {
        private CashCriticalityEvaluator _cashCriticalityEvaluator;
        private INotifier _notifier;
        private IPlayerController _controller;
        private Table _table;

        [SetUp]
        public void Setup()
        {
            _cashCriticalityEvaluator = new CashCriticalityEvaluator();
        }

        [Test]
        public void ShouldGiveANumberBetweenOneAndZero()
        {
            var criticality = _cashCriticalityEvaluator.EvaluateCashCriticality(500, 100, 200, 500);

            Assert.IsTrue(criticality <= 1);
            Assert.IsTrue(criticality >= 0);
        }
    }
}
