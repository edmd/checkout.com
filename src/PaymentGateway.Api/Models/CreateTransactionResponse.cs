
using Infrastructure.Transaction.Services.Models;

namespace PaymentGateway.Api.Models
{
    public class CreateTransactionResponse
    {
        public CreateTransactionResponse() { }

        public CreateTransactionResponse(Guid transactionId, Guid acquirerTransactionId, TransactionStatus status)
        {
            TransactionId = transactionId;
            AcquirerTransactionId = acquirerTransactionId;
            Status = status;
        }

        public Guid TransactionId { get; set; }
        public Guid AcquirerTransactionId { get; set; }
        public TransactionStatus Status { get; set; }
    }
}