
namespace Infrastructure.Transaction.Services.Exceptions
{
    public class TransactionFailedException : Exception
    {
        public TransactionFailedException(string message)
            : base($"{message}")
        { }
    }
}