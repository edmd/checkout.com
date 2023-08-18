using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Persistence;
using PaymentGateway.Data.Persistence.Entities;

namespace PaymentGateway.Data
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly PaymentGatewayDbContext _dbContext;

        public TransactionsRepository(PaymentGatewayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddTransaction(Transaction transaction)
        {
            _dbContext.Transactions.Add(transaction);

            int affectedRecords = await _dbContext.SaveChangesAsync();

            return affectedRecords;
        }

        public async Task<Transaction?> GetTransaction(Guid transactionId)
        {
            return await _dbContext.Transactions
                .FirstOrDefaultAsync(x => x.TransactionId == transactionId);
        }
    }
}
