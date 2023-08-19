using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Transaction.Services.Workers;

namespace Infrastructure.Transaction.Services
{
    public static class TransactionsServicesRegistration
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // Services registration
            services.AddTransient<ITransactionWorker, TransactionWorker>();

            return services;
        }
    }
}