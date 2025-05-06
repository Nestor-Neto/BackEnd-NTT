using Ambev.DeveloperEvaluation.MessageBroker;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services)
        {
            services.AddScoped<IMessageBroker, LogMessageBroker>();
            return services;
        }
    }
} 