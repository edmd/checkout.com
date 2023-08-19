using AutoMapper;
using Infrastructure.Transaction.Services.Workers;
using MediatR;
using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Handlers
{
    public class GetTransactionByIdHandler : IRequestHandler<GetTransactionRequest, GetTransactionResponse>
    {
        private readonly IMapper _mapper;
        private readonly ITransactionWorker _worker;

        public GetTransactionByIdHandler(ITransactionWorker worker, IMapper mapper)
        {
            _worker = worker;
            _mapper = mapper;
        }

        public async Task<GetTransactionResponse> Handle(GetTransactionRequest request, CancellationToken token)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return _mapper.Map<GetTransactionResponse>(await _worker.Get(request.TransactionId));
        }
    }
}