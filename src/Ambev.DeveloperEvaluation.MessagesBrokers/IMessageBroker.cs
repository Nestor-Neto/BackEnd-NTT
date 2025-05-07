

namespace Ambev.DeveloperEvaluation.MessagesBrokers
{
    /// <summary>Interface para publicação de mensagens em um sistema de mensageria.</summary>
    public interface IMessageBroker
    {
        /// <summary>Publica uma mensagem tipada em um tópico específico, serializando o objeto para JSON.</summary>
        Task PublishAsync<T>(string topic, T message) where T : class;

        /// <summary>Publica uma mensagem de texto simples em um tópico específico.</summary>
        Task PublishAsync(string topic, string message);
    }
}
