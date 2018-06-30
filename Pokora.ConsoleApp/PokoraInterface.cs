using System;
using System.Collections.Generic;
using System.Text;
using Pokora.ConsoleApp.Display;
using Pokora.ConsoleApp.PlayerControllers;
using Pokora.GameMechanisms;

namespace Pokora.ConsoleApp
{
    public class PokoraInterface
    {
        private readonly Logger _logger;
        private readonly PokoraDisplayer _displayer;
        private Table _table;
        private readonly ConsoleNotifier _consoleNotifier;
        private readonly EventManager _eventManager;

        public PokoraInterface(
            Logger logger, 
            PokoraDisplayer displayer, 
            ConsoleNotifier consoleNotifier,
            EventManager eventManager)
        {
            _logger = logger;
            _displayer = displayer;
            _consoleNotifier = consoleNotifier;
            _eventManager = eventManager;
        }

        public void Setup(ConsoleNotifier notifier)
        {
            _table = new Table(10, 20, 1000, 3, notifier);

            _displayer.SetupDisplay(_table);
            _eventManager.EventReceived += _eventManager_EventReceived;

            _table.Join("Tommy", new ConsolePlayerController(_displayer));
            _table.Join("Ratchet", new ConsolePlayerController(_displayer));
            _table.Join("Corail", new ConsolePlayerController(_displayer));

            _logger.LogMessageAsync("Setting up interface");

            _table.Start();
        }

        private void _eventManager_EventReceived(string eventMessage)
        {
            _displayer.PushEvent(eventMessage);
        }
    }
}
