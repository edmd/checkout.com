using Infrastructure.Transaction.Services.Models;

namespace Infrastructure.Transaction.Services.Worker
{
    public interface ITransactionWorker
    {
        Task<TransactionStatusResponse> Create(TransactionRequest request);
        Task<TransactionResponse> Get(Guid id);
    }
}