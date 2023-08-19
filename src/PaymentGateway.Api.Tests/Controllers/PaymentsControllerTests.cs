using AutoMapper;
using FluentAssertions;
using Infrastructure.Transaction.Services.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Tests.Controllers
{
    [TestFixture]
    public class PaymentsControllerTests
    {
        private readonly PaymentsController _sut;
        private readonly IMapper _mapper;
        private readonly Mock<ILoggerFactory> _mockfactory;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IHttpContextAccessor> _accessorMock;

        public PaymentsControllerTests()
        {
            _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<ApiMappingProfile>(); })
                .CreateMapper();

            _mockfactory = new Mock<ILoggerFactory>();

            _mediatorMock = new Mock<IMediator>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateTransactionRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateTransactionResponse(Guid.NewGuid(), Guid.NewGuid(), TransactionStatus.Accepted))
                .Verifiable("CreateTransactionRequest was not sent.");

            _accessorMock = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            context.Request.Scheme = "https";
            context.Request.Host = HostString.FromUriComponent("https:localhost");
            context.Request.Path = PathString.Empty;
            _accessorMock.Setup(_ => _.HttpContext).Returns(context);

            _sut = new PaymentsController(_mockfactory.Object, _mapper, _mediatorMock.Object, _accessorMock.Object) { };
        }

        [Test]
        public async Task Should_create_transaction_successfully()
        {
            // Arrange
            var transaction = new CreateTransactionRequest(
                123, "John Smith", "0000222233334444", "02/23", "02/28", "123", 99.99M, "GBP");

            //Act
            IActionResult result = await _sut.CreateTransaction(transaction);

            //Assert
            _mediatorMock.Verify(x => x.Send(
                It.IsAny<CreateTransactionRequest>(), 
                It.IsAny<CancellationToken>()), Times.Once());

            Assert.Multiple(() =>
            {
                result.Should().BeAssignableTo<CreatedResult>();
                var createdResult = result as CreatedResult;
                createdResult?.Value.Should().BeAssignableTo<CreateTransactionResponse>();
                var value = createdResult?.Value as CreateTransactionResponse;
                value?.TransactionId.Should().NotBeEmpty();
            });
        }

        //public async Task Should_create_transaction_unsuccessfully() { }
        //public async Task Should_get_transaction_successfully() { }
        //public async Task Should_get_transaction_unsuccessfully() { }
    }
}