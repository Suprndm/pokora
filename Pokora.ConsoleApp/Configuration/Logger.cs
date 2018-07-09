using System.Threading.Tasks;
using Monithor.Client;
using Monithor.Definitions;

namespace Pokora.ConsoleApp.Configuration
{
    public class Logger
    {
        private readonly IMonithorEmitter _monithorEmitter;

        public Logger(IMonithorEmitter monithorEmitter)
        {
            _monithorEmitter = monithorEmitter;
        }

        public Task LogMessageAsync(string message)
        {
            return Task.CompletedTask;
            //     return _monithorEmitter.Trace(MessageLevel.Informational, MessageType.Fonctional, "", message, "");
        }

        public Task LogErrorAsync(string errorMessage)
        {
            return Task.CompletedTask;
           // return _monithorEmitter.Trace(MessageLevel.Error, MessageType.Fonctional, "", errorMessage, "");
        }
    }
}
