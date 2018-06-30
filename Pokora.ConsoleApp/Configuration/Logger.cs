using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Monithor.Client;
using Monithor.Definitions;

namespace Pokora.ConsoleApp
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
            return _monithorEmitter.Trace(MessageLevel.Informational, MessageType.Fonctional, "", message, "");
        }

        public Task LogErrorAsync(string errorMessage)
        {
            return _monithorEmitter.Trace(MessageLevel.Error, MessageType.Fonctional, "", errorMessage, "");
        }
    }
}
