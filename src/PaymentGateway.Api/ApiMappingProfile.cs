using AutoMapper;
using Infrastructure.Transaction.Services.Models;
using PaymentGateway.Api.Models;
using System.Text.RegularExpressions;

namespace PaymentGateway.Api
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<CreateTransactionRequest, TransactionRequest>();

            CreateMap<TransactionStatusResponse, CreateTransactionResponse>();

            CreateMap<TransactionResponse, GetTransactionResponse>()
                //.ForMember(d => d.CardNumber, opt => opt.MapFrom(src => MaskCardNumber(src)))
                .ForMember(d => d.CardNumber, opt => opt.MapFrom(src => src.CardNumber))
                .ForMember(d => d.Cvv2, opt => opt.Ignore())
                .ForMember(d => d.AcquirerTransactionId, opt => opt.MapFrom(src => src.AcquirerTransactionId))
                .ForMember(d => d.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(d => d.CardHolderName, opt => opt.MapFrom(src => src.CardHolderName))
                .ForMember(d => d.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(d => d.MerchantId, opt => opt.MapFrom(src => src.MerchantId))
                .ForMember(d => d.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(d => d.ValidFrom, opt => opt.MapFrom(src => src.ValidFrom))
                .ForMember(d => d.ValidTo, opt => opt.MapFrom(src => src.ValidTo));
        }

        private string MaskCardNumber(TransactionResponse src)
        {
            var firstDigits = src.CardNumber.Substring(0, 6);
            var lastDigits = src.CardNumber.Substring(src.CardNumber.Length - 4, 4);

            var requiredMask = new string('*', src.CardNumber.Length - firstDigits.Length - lastDigits.Length);

            var maskedString = string.Concat(firstDigits, requiredMask, lastDigits);
            return Regex.Replace(maskedString, ".{4}", "$0 ");
        }
    }
}