using AutoMapper;
using Infrastructure.Transaction.Services.Models;

namespace Infrastructure.Transaction.Services
{
    public class TransactionMappingProfile : Profile
    {
        public TransactionMappingProfile()
        {
            CreateMap<PaymentGateway.Data.Persistence.Entities.Transaction, TransactionResponse>();

            CreateMap<TransactionRequest, PaymentGateway.Data.Persistence.Entities.Transaction>();

            CreateMap<(TransactionStatusResponse response, TransactionRequest request), PaymentGateway.Data.Persistence.Entities.Transaction>()
                .ForMember(d => d.AcquirerTransactionId, opt => opt.MapFrom(src => src.response.AcquirerTransactionId))
                .ForMember(d => d.Status, opt => opt.MapFrom(src => src.response.Status))
                .ForMember(d => d.Amount, opt => opt.MapFrom(src => src.request.Amount))
                .ForMember(d => d.CardHolderName, opt => opt.MapFrom(src => src.request.CardHolderName))
                .ForMember(d => d.CardNumber, opt => opt.MapFrom(src => src.request.CardNumber))
                .ForMember(d => d.CurrencyCode, opt => opt.MapFrom(src => src.request.CurrencyCode))
                .ForMember(d => d.Cvv2, opt => opt.MapFrom(src => src.request.Cvv2))
                .ForMember(d => d.MerchantId, opt => opt.MapFrom(src => src.request.MerchantId))
                .ForMember(d => d.ValidFrom, opt => opt.MapFrom(src => src.request.ValidFrom))
                .ForMember(d => d.ValidTo, opt => opt.MapFrom(src => src.request.ValidTo));
        }
    }
}