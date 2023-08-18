
using MediatR;

namespace PaymentGateway.Api.Models
{
    public class GetTransactionRequest : IRequest<GetTransactionResponse>
    {
        public GetTransactionRequest(Guid transactionId)
        {
            TransactionId = transactionId;
        }

        public Guid TransactionId { get; set; }
    }
}