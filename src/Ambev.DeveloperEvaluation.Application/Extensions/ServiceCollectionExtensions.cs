using Ambev.DeveloperEvaluation.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Ambev.DeveloperEvaluation.Domain.Interface.IServices;

namespace Ambev.DeveloperEvaluation.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<SaleApplicationService>();

            return services;
        }
    }
} 