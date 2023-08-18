
namespace Infrastructure.Transaction.Services.Models
{
    public enum TransactionStatus
    {
        Pending = 1,
        Accepted = 2, 
        Processing = 4,
        Success = 8,
        Failed = 16, 
        Aborted = 32, 
        Refunded = 64, 
        Void = 128
    }
}