using MediatR;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Api.Models
{
    public class CreateTransactionRequest : IRequest<CreateTransactionResponse>
    {
        public CreateTransactionRequest(int merchantId, string cardHolderName, string cardNumber, 
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

        [Required]
        public int MerchantId { get; set; }

        [Required]
        public string CardHolderName { get; set; }

        [Required]
        public string CardNumber { get; set; }

        public string? ValidFrom { get; set; }

        [Required]
        public string ValidTo { get; set; }

        [Required]
        public string Cvv2 { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string CurrencyCode { get; set; }
    }
}
