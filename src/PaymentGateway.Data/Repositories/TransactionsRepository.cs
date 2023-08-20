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

        public async Task<Guid> AddTransaction(Transaction transaction)
        {
            transaction.TransactionId = Guid.NewGuid();
            _dbContext.Transactions.Add(transaction);

            int affectedRecords = await _dbContext.SaveChangesAsync();

            if(affectedRecords > 0)
            {
                return transaction.TransactionId;
            }

            return Guid.Empty;
        }

        public async Task<Transaction?> GetTransaction(Guid transactionId)
        {
            return await _dbContext.Transactions
                .FirstOrDefaultAsync(x => x.TransactionId == transactionId);
        }
    }
}
