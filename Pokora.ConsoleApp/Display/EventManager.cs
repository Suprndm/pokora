using System;

namespace Pokora.ConsoleApp.Display
{
    public class EventManager
    {
        public event Action<string> EventReceived;

        public void RaiseEvent(string message)
        {
            EventReceived?.Invoke(message);
        }
    }
}
