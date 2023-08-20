using AutoMapper;
using FluentAssertions;
using Infrastructure.Transaction.Services.Models;

namespace Infrastructure.Transaction.Services.Tests
{
    [TestFixture]
    public class TransactionMappingProfileTests
    {
        private readonly IMapper _mapper;
        public TransactionMappingProfileTests()
        {
            _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<TransactionMappingProfile>(); })
                .CreateMapper();
        }

        [Test]
        public void Should_map_Transaction_properties()
        {
            //CreateMap<PaymentGateway.Data.Persistence.Entities.Transaction, TransactionResponse>();

            //Arrange
            var source = new PaymentGateway.Data.Persistence.Entities.Transaction(
                transactionId: Guid.NewGuid(), 
                acquirerTransactionId: Guid.NewGuid(),
                merchantId: int.MaxValue,
                cardHolderName: Guid.NewGuid().ToString(),
                cardNumber: Guid.NewGuid().ToString(),
                validFrom: Guid.NewGuid().ToString(),
                validTo: Guid.NewGuid().ToString(),
                cvv2: Guid.NewGuid().ToString(),
                amount: int.MaxValue,
                currencyCode: Guid.NewGuid().ToString(),
                status: 2
            );

            //Act
            var result = _mapper.Map<TransactionResponse>(source);

            //Assert
            Assert.Multiple(() =>
            {
                result.Amount.Should().Be(source.Amount);
                result.CardHolderName.Should().Be(source.CardHolderName);
                result.CardNumber.Should().Be(source.CardNumber);
                result.CurrencyCode.Should().Be(source.CurrencyCode);
                result.Cvv2.Should().Be(source.Cvv2);
                result.MerchantId.Should().Be(source.MerchantId);
                result.ValidFrom.Should().Be(source.ValidFrom);
                result.ValidTo.Should().Be(source.ValidTo);
            });
        }

        [Test]
        public void Should_map_TransactionRequest_properties()
        {
            //CreateMap<TransactionRequest, PaymentGateway.Data.Persistence.Entities.Transaction>();

            //Arrange
            var source = new TransactionRequest(
                merchantId: int.MaxValue,
                cardHolderName: Guid.NewGuid().ToString(),
                cardNumber: Guid.NewGuid().ToString(),
                validFrom: Guid.NewGuid().ToString(),
                validTo: Guid.NewGuid().ToString(),
                cvv2: Guid.NewGuid().ToString(),
                amount: int.MaxValue,
                currencyCode: Guid.NewGuid().ToString()
            );

            //Act
            var result = _mapper.Map<PaymentGateway.Data.Persistence.Entities.Transaction>(source);

            //Assert
            Assert.Multiple(() =>
            {
                result.Amount.Should().Be(source.Amount);
                result.CardHolderName.Should().Be(source.CardHolderName);
                result.CardNumber.Should().Be(source.CardNumber);
                result.CurrencyCode.Should().Be(source.CurrencyCode);
                result.Cvv2.Should().Be(source.Cvv2);
                result.MerchantId.Should().Be(source.MerchantId);
                result.ValidFrom?.Should().Be(source.ValidFrom);
                result.ValidTo.Should().Be(source.ValidTo);
            });
        }

        //CreateMap<(TransactionStatusResponse response, TransactionRequest request), PaymentGateway.Data.Persistence.Entities.Transaction>()
    }
}