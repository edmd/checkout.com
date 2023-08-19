using AutoMapper;
using FluentAssertions;
using Infrastructure.Transaction.Services.Models;
using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Tests.Mappings
{
    [TestFixture]
    public class ApiMappingProfileTests
    {
        private readonly IMapper _mapper;
        public ApiMappingProfileTests()
        {
            _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<ApiMappingProfile>(); })
                .CreateMapper();
        }

        [Test]
        public void Should_map_CreateTransactionRequest_properties()
        {
            //CreateMap<CreateTransactionRequest, TransactionRequest>();

            //Arrange
            var source = new CreateTransactionRequest(
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
            var result = _mapper.Map<TransactionRequest>(source);

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
        public void Should_map_TransactionStatusResponse_properties()
        {
            //CreateMap<TransactionStatusResponse, CreateTransactionResponse>();

            //Arrange
            var source = new TransactionStatusResponse(
                transactionId: Guid.NewGuid(), 
                acquirerTransactionId: Guid.NewGuid(), 
                status: TransactionStatus.Success
            );

            //Act
            var result = _mapper.Map<CreateTransactionResponse>(source);

            //Assert
            Assert.Multiple(() =>
            {
                result.AcquirerTransactionId.Should().Be(source.AcquirerTransactionId);
                result.Status.Should().Be(source.Status);
                result.TransactionId.Should().Be(source.TransactionId.ToString());
            });
        }

        [Test]
        public void Should_map_GetTransactionRequest_properties()
        {
            //CreateMap<TransactionResponse, GetTransactionResponse>()

            //Arrange
            var source = new TransactionResponse(
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
                status: 1
            );

            //Act
            var result = _mapper.Map<GetTransactionResponse>(source);

            //Assert
            Assert.Multiple(() =>
            {
                result.AcquirerTransactionId.Should().Be(source.AcquirerTransactionId);
                result.Amount.Should().Be(source.Amount);
                result.CardHolderName.Should().Be(source.CardHolderName);
                result.CardNumber.Should().Contain(source.CardNumber.Substring(0, 4));
                result.CardNumber.Should().Contain("*");
                result.CurrencyCode.Should().Be(source.CurrencyCode);
                result.Cvv2.Should().BeEmpty();
                result.MerchantId.Should().Be(source.MerchantId);
                result.Status.Should().Be(source.Status);
                result.TransactionId.Should().Be(source.TransactionId);
                result.ValidFrom.Should().Be(source.ValidFrom);
                result.ValidTo.Should().Be(source.ValidTo);
            });
        }
    }
}