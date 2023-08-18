namespace Infrastructure.Transaction.Services.Models
{
    public class TransactionRequest
    {
        public TransactionRequest(int merchantId, string cardHolderName, string cardNumber, 
            string? validFrom, string validTo, string cvv2, decimal amount, string currencyCode)
        {
            MerchantId = merchantId;
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            ValidFrom = validFrom;
            ValidTo = validTo;
            Cvv2 = cvv2;
            Amount = amount;
            CurrencyCode = currencyCode;
        }

        public int MerchantId { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string? ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string Cvv2 { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}
