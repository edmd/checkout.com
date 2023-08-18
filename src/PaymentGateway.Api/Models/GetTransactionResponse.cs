
using System.Text.RegularExpressions;

namespace PaymentGateway.Api.Models
{
    public class GetTransactionResponse
    {
        private string _cardNumber;
        public GetTransactionResponse() { }

        public GetTransactionResponse(Guid transactionId, Guid acquirerTransactionId, int merchantId, 
            string cardHolderName, string cardNumber, string? validFrom, string validTo, string cvv2, 
            decimal amount, string currencyCode, int status)
        {
            TransactionId = transactionId;
            AcquirerTransactionId = acquirerTransactionId;
            MerchantId = merchantId;
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            ValidFrom = validFrom;
            ValidTo = validTo;
            Cvv2 = cvv2;
            Amount = amount;
            CurrencyCode = currencyCode;
            Status = status;
        }

        public Guid TransactionId { get; set; }
        public Guid AcquirerTransactionId { get; set; }
        public int MerchantId { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber {
            get {
                // TODO: Triage ApiMappingProfile.MaskCardNumber
                var firstDigits = _cardNumber.Substring(0, 6);
                var lastDigits = _cardNumber.Substring(_cardNumber.Length - 4, 4);

                var requiredMask = new string('*', _cardNumber.Length - firstDigits.Length - lastDigits.Length);

                var maskedString = string.Concat(firstDigits, requiredMask, lastDigits);
                return Regex.Replace(maskedString, ".{4}", "$0 ");
            }
            set { _cardNumber = value; }
        }
        public string? ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string Cvv2 { get { return ""; } set {  } }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public int Status { get; set; }
    }
}