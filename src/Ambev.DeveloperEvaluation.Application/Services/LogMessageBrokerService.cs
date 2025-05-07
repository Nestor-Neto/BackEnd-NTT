using Ambev.DeveloperEvaluation.Domain.Interface.IServices;
using Ambev.DeveloperEvaluation.MessagesBrokers;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Services;

public class LogMessageBrokerService : IMessageBrokerService
{
    private readonly IMessageBroker _messageBroker;
    private readonly ILogger<LogMessageBrokerService> _logger;

    public LogMessageBrokerService(IMessageBroker messageBroker, ILogger<LogMessageBrokerService> logger)
    {
        _messageBroker = messageBroker;
        _logger = logger;
    }

    public async Task PublishSaleCreatedAsync(Guid saleId, string customerName, decimal totalAmount)
    {
        var message = new
        {
            SaleId = saleId,
            CustomerName = customerName,
            TotalAmount = totalAmount,
            Timestamp = DateTime.UtcNow
        };

        await _messageBroker.PublishAsync("sale.created", message);
    }

    public async Task PublishSaleCancelledAsync(Guid saleId, string reason)
    {
        var message = new
        {
            SaleId = saleId,
            Reason = reason,
            Timestamp = DateTime.UtcNow
        };

        await _messageBroker.PublishAsync("sale.cancelled", message);
    }

    public async Task PublishErrorAsync(string errorMessage, Exception? exception = null)
    {
        var message = new
        {
            Error = errorMessage,
            Exception = exception?.ToString(),
            Timestamp = DateTime.UtcNow
        };

        await _messageBroker.PublishAsync("error", message);
    }
} 