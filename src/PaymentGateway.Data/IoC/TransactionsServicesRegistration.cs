using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Data.Persistence;

namespace PaymentGateway.Data.IoC
{
    public static class TransactionsServicesRegistration
    {
        public static IServiceCollection RegistraterDataServices(this IServiceCollection services)
        {
            // Repositories and services registration
            services.AddTransient<ITransactionsRepository, TransactionsRepository>();
            services.AddDbContext<PaymentGatewayDbContext>();

            return services;
        }
    }
}