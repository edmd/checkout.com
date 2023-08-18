using Infrastructure.Transaction.Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Transaction.Services.Processors
{
    public class TransactionProcessorA : ITransactionProcessor
    {
        public async Task<List<ValidationResult>> ValidateTransaction(TransactionRequest request)
        {
            // Processing...

            return new List<ValidationResult>() { new ValidationResult("A is down") };
        }

        public async Task<TransactionStatusResponse> ProcessTransaction(TransactionRequest request)
        {
            // Processing...

            return new TransactionStatusResponse(null, Guid.NewGuid(), TransactionStatus.Failed) { };
        }

        public async Task<TransactionStatusResponse> RetrieveTransaction(Guid transactionId)
        {
            // Retrieve transaction...

            return new TransactionStatusResponse(transactionId, Guid.NewGuid(), TransactionStatus.Failed);
        }
    }
}