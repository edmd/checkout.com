using PaymentGateway.Data.Persistence.Entities;

namespace PaymentGateway.Data
{
    public interface ITransactionsRepository
    {
        Task<int> AddTransaction(Transaction transaction);

        Task<Transaction?> GetTransaction(Guid transactionId);
    }
}