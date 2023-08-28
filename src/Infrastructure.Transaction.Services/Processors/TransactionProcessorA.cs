using Infrastructure.Transaction.Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Transaction.Services.Processors
{
    public class TransactionProcessorA : ITransactionProcessor
    {
        public async Task<List<ValidationResult>>? ValidateTransaction(TransactionRequest request)
        {
            await Task.Delay(1); // arbitrary processing
            return new List<ValidationResult>() { new ValidationResult("A is down") };
        }

        public async Task<TransactionStatusResponse>? ProcessTransaction(TransactionRequest request)
        {
            await Task.Delay(1);
            return new TransactionStatusResponse(Guid.Empty, Guid.Empty, TransactionStatus.Failed) { };
        }

        public async Task<TransactionStatusResponse>? RetrieveTransaction(Guid transactionId)
        {
            await Task.Delay(1);
            return new TransactionStatusResponse(transactionId, Guid.Empty, TransactionStatus.Failed);
        }
    }
}