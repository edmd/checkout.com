using Infrastructure.Transaction.Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Transaction.Services.Processors
{
    public class TransactionProcessorB : ITransactionProcessor
    {
        public async Task<List<ValidationResult>> ValidateTransaction(TransactionRequest request)
        {
            return null;
        }

        public async Task<TransactionStatusResponse> ProcessTransaction(TransactionRequest request)
        {
            return new TransactionStatusResponse(null, Guid.NewGuid(), TransactionStatus.Accepted) { };
        }

        public async Task<TransactionStatusResponse> RetrieveTransaction(Guid transactionId)
        {
            return new TransactionStatusResponse(transactionId, Guid.NewGuid(), TransactionStatus.Success);
        }
    }
}