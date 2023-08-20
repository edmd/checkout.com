using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Data.Persistence;
using PaymentGateway.Data.Persistence.Entities;

namespace PaymentGateway.Data.Tests
{
    //https://learn.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking
    [TestFixture]
    public class TransactionsRepositoryTests
    {
        private ITransactionsRepository _transactionsRepository;
        private PaymentGatewayDbContext _paymentGatewayDbContext;
        private Transaction _transaction;

        [SetUp]
        public void BeforeEach()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PaymentGatewayDbContext>()
                .UseInMemoryDatabase("PaymentGatewayDb");
            _paymentGatewayDbContext = new PaymentGatewayDbContext(optionsBuilder.Options);
            _transactionsRepository = new TransactionsRepository(_paymentGatewayDbContext);

            _transaction = new Transaction(
                Guid.NewGuid(), Guid.NewGuid(), int.MaxValue, "John Smith",
                "4444333322221111", "03/23", "03/28", "123", 99M, "GBP", 2);
        }

        [Test]
        public async Task Should_save_transaction_successfully()
        {
            // Act
            var result = await _transactionsRepository.AddTransaction(_transaction);

            // Assert
            result.Should().NotBeEmpty();
            Assert.IsInstanceOf(typeof(Guid), result);
        }

        [Test]
        public async Task Should_retrieve_transaction_successfully()
        {
            // Arrange
            await _transactionsRepository.AddTransaction(_transaction);

            // Act
            var result = await _transactionsRepository.GetTransaction(_transaction.TransactionId);

            // Assert
            Assert.Multiple(() =>
            {
                result?.AcquirerTransactionId.Should().Be(_transaction.AcquirerTransactionId);
                result?.Amount.Should().Be(_transaction.Amount);
                result?.CardHolderName.Should().Be(_transaction.CardHolderName);
                result?.CardNumber.Should().Be(_transaction.CardNumber);
                result?.CurrencyCode.Should().Be(_transaction.CurrencyCode);
                result?.Cvv2.Should().Be(_transaction.Cvv2);
                result? .MerchantId.Should().Be(_transaction.MerchantId);
                result?.Status.Should().Be(_transaction.Status);
                result?.TransactionId.Should().Be(_transaction.TransactionId);
                result?.ValidFrom.Should().Be(_transaction.ValidFrom);
                result?.ValidTo.Should().Be(_transaction.ValidTo);
            });
        }
    }
}