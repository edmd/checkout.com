using Infrastructure.Transaction.Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Transaction.Services.Processors
{
    public class TransactionProcessorB : ITransactionProcessor
    {
        public async Task<List<ValidationResult>>? ValidateTransaction(TransactionRequest request)
        {
            await Task.Delay(1); // arbitrary processing
            return null;
        }

        public async Task<TransactionStatusResponse>? ProcessTransaction(TransactionRequest request)
        {
            await Task.Delay(1); // arbitrary processing
            return new TransactionStatusResponse(Guid.NewGuid(), Guid.NewGuid(), TransactionStatus.Accepted) { };
        }

        public async Task<TransactionStatusResponse>? RetrieveTransaction(Guid transactionId)
        {
            await Task.Delay(1); // arbitrary processing
            return new TransactionStatusResponse(transactionId, Guid.NewGuid(), TransactionStatus.Success);
        }
    }
}