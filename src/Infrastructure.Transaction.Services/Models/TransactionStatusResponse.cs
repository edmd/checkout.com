namespace Infrastructure.Transaction.Services.Models
{
    public class TransactionStatusResponse
    {
        public TransactionStatusResponse() { }

        public TransactionStatusResponse(Guid? transactionId, Guid acquirerTransactionId, TransactionStatus status)
        {
            TransactionId = transactionId;
            AcquirerTransactionId = acquirerTransactionId;
            Status = status;
        }

        public Guid? TransactionId { get; set; }
        public Guid AcquirerTransactionId { get; set; }
        public TransactionStatus Status { get; set; }
    }
}