using AutoMapper;
using FluentAssertions;
using Infrastructure.Transaction.Services.Exceptions;
using Infrastructure.Transaction.Services.Models;
using Infrastructure.Transaction.Services.Workers;
using Moq;
using PaymentGateway.Data;

namespace Infrastructure.Transaction.Services.Tests
{
    [TestFixture]
    public class TransactionWorkerTests
    {
        private IMapper _mapper;
        public TransactionWorker _transactionWorker;
        public Mock<ITransactionsRepository> _transactionsRepository;

        [SetUp]
        public void BeforeEach()
        {
            _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<TransactionMappingProfile>(); })
                .CreateMapper();

            _transactionsRepository = new Mock<ITransactionsRepository>();
            _transactionsRepository.Setup(x => x.AddTransaction(
                It.IsAny<PaymentGateway.Data.Persistence.Entities.Transaction>())).ReturnsAsync(Guid.NewGuid);
            _transactionWorker = new TransactionWorker(_transactionsRepository.Object, _mapper);
        }

        [Test]
        public void Should_create_transaction_using_A()
        {
            // Arrange
            var _transaction = new TransactionRequest(
                int.MaxValue, "John Smith", "1111333322221111", "03/23", "03/28", "123", 99M, "GBP");

            // Act & Assert
            Assert.ThrowsAsync<TransactionFailedException>(() => _transactionWorker.Create(_transaction))
                .Message.Should().Be("Transaction rejected by Acquirer");
        }

        [Test]
        public async Task Should_process_transaction_using_B()
        {
            // Arrange
            var _transaction = new TransactionRequest(
                int.MaxValue, "John Smith", "4444333322221111", "03/23", "03/28", "123", 99M, "GBP");

            // Act
            var transactionResponse = await _transactionWorker.Create(_transaction);

            // Assert
            Assert.Multiple(() =>
            {
                transactionResponse.AcquirerTransactionId.Should().NotBeEmpty();
                transactionResponse.Status.Should().Be(TransactionStatus.Accepted);
                transactionResponse.TransactionId.Should().NotBeEmpty();
            });
        }

        [Test]
        public async Task Should_process_transaction_using_C()
        {
            // Arrange
            var _transaction = new TransactionRequest(
                int.MaxValue, "John Smith", "1234333322221111", "03/23", "03/28", "123", 99M, "GBP");

            // Act & Assert
            Assert.ThrowsAsync<TransactionFailedException>(() => _transactionWorker.Create(_transaction))
                .Message.Should().Be("Transaction rejected by Acquirer");
        }
    }
}