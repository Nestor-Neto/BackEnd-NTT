

namespace Ambev.DeveloperEvaluation.Domain.Interface.IServices
{
    public interface IMessageBrokerService
    {
        Task PublishSaleCreatedAsync(Guid saleId, string customerName, decimal totalAmount);
        Task PublishSaleCancelledAsync(Guid saleId, string reason);
        Task PublishErrorAsync(string errorMessage, Exception? exception = null);
    }
} 