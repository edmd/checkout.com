using Infrastructure.Transaction.Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Transaction.Services.Processors
{
    public class TransactionProcessorC : ITransactionProcessor
    {
        public async Task<List<ValidationResult>> ValidateTransaction(TransactionRequest request)
        {
            return new List<ValidationResult>() {
                    new ValidationResult("Card Expired", new string[] { nameof(request.CardNumber) })
                };
        }

        public async Task<TransactionStatusResponse> ProcessTransaction(TransactionRequest request)
        {
            return null;
        }

        public async Task<TransactionStatusResponse> RetrieveTransaction(Guid transactionId)
        {
            return new TransactionStatusResponse(transactionId, Guid.NewGuid(), TransactionStatus.Failed);
        }
    }
}