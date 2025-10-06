
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Jogos.Service.Application.Logging
{
    public class CustomerLoggerProvider : ILoggerProvider
    {

        readonly CustomLoggerProviderConfiguration loggerConfig;
        readonly ConcurrentDictionary<string, CustomerLogger> loggers = new ConcurrentDictionary<string, CustomerLogger>();

        public CustomerLoggerProvider(CustomLoggerProviderConfiguration config)
        {
            loggerConfig = config;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, loggerConfig));
        }
        public void Dispose()
        {
            loggers.Clear();
        }
    }
}

