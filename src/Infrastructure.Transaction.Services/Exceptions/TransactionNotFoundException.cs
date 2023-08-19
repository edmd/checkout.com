
namespace Infrastructure.Transaction.Services.Exceptions
{
    public class TransactionNotFoundException : Exception
    {
        public TransactionNotFoundException(string message)
            : base($"{message}")
        { }
    }
}