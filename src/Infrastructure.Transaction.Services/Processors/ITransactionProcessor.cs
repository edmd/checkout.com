using Infrastructure.Transaction.Services.Models;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Transaction.Services.Processors
{
    public interface ITransactionProcessor
    {
        public Task<List<ValidationResult>>? ValidateTransaction(TransactionRequest request);
        public Task<TransactionStatusResponse>? ProcessTransaction(TransactionRequest request);
        public Task<TransactionStatusResponse>? RetrieveTransaction(Guid transactionId);
    }
}