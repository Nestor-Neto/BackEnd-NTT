using Ambev.DeveloperEvaluation.MessagesBrokers;
using Ambev.DeveloperEvaluation.Domain.Interface.IServices;
using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Ambev.DeveloperEvaluation.Domain.Interface;

namespace Ambev.DeveloperEvaluation.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services)
        {
            services.AddScoped<IMessageBroker, LogMessageBroker>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IMongoDBService, MongoDBService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
} 