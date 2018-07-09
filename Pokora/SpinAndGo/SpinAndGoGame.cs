using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pokora.GameMechanisms;

namespace Pokora.SpinAndGo
{
    public class SpinAndGoGame
    {
        private readonly double _multiplier;
        private readonly INotifier _notifier;
        public IList<User> Users { get; private set; }
        private bool _tableEnded;

        private Player _winningPlayer;
        public double Prize { get; set; }
        public double Fee { get; set; }
        public Table Table { get; set; }
        public int GameCount { get; private set; }

        public SpinAndGoGame(double entranceFee, INotifier notifier, int gameCount)
        {
            GameCount = gameCount;
            Fee = entranceFee;
            _notifier = notifier;

            Fee = entranceFee;

            _multiplier = GetMultiplier();

            Prize = Fee * _multiplier;

            Users = new List<User>();
        }

        public void Setup(IList<User> users)
        {
            Users = users;
        }

        public Task<User> LaunchAsync()
        {
            return Task.Run(async () =>
             {
                 Table = new Table(10, 20, 1000, 3, _notifier);

                 foreach (var user in Users)
                 {
                     Table.Join(user.Name, user.Controller);
                     user.Pay(Fee);
                 }

                 Table.TableFinished += _table_TableFinished;
                 Table.Start();

                 while (_tableEnded == false)
                 {
                     await Task.Delay(2000);
                 }

                 return Users.Single(user => user.Name == _winningPlayer.Name);
             });
        }

        private void _table_TableFinished(Player player)
        {
            _tableEnded = true;
            _winningPlayer = player;
            _notifier.SpinAndGoWonBy(player);
            Table.Dispose();
        }

        private double GetMultiplier()
        {
            var randomInt = Randomizer.Instance.Random.Next(1000000);

            if (randomInt < 1)
                return 12000;
            else if (randomInt < 31)
                return 240;
            else if (randomInt < 106)
                return 120;
            else if (randomInt < 1106)
                return 25;
            else if (randomInt < 6106)
                return 10;
            else if (randomInt < 81106)
                return 6;
            else if (randomInt < 265612)
                return 4;
            else return 2;

        }


    }
}
