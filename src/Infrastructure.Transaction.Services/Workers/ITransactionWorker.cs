using Infrastructure.Transaction.Services.Models;

namespace Infrastructure.Transaction.Services.Workers
{
    public interface ITransactionWorker
    {
        Task<TransactionStatusResponse> Create(TransactionRequest request);
        Task<TransactionResponse> Get(Guid id);
    }
}