using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.MessagesBrokers
{
    /// <summary>
    /// Implementação do Message Broker que registra as mensagens em log ao invés de publicá-las em um broker real.
    /// Esta implementação é útil para desenvolvimento e testes, permitindo visualizar as mensagens que seriam publicadas.
    /// </summary>
    public class LogMessageBroker : IMessageBroker
    {
        private readonly ILogger<LogMessageBroker> _logger;

        public LogMessageBroker(ILogger<LogMessageBroker> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Publica uma mensagem genérica (objeto) em um tópico específico, registrando-a no log.
        /// </summary>
        /// <typeparam name="T">Tipo da mensagem a ser publicada</typeparam>
        /// <param name="topic">Tópico onde a mensagem será publicada</param>
        /// <param name="message">Mensagem a ser publicada</param>
        public Task PublishAsync<T>(string topic, T message) where T : class
        {
            _logger.LogInformation("Publicando mensagem no tópico {Topic}: {@Message}", topic, message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Publica uma mensagem de texto em um tópico específico, registrando-a no log.
        /// </summary>
        /// <param name="topic">Tópico onde a mensagem será publicada</param>
        /// <param name="message">Mensagem de texto a ser publicada</param>
        public Task PublishAsync(string topic, string message)
        {
            _logger.LogInformation("Publicando mensagem no tópico {Topic}: {Message}", topic, message);
            return Task.CompletedTask;
        }
    }
}
