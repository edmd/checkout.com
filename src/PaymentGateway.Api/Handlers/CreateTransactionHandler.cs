using AutoMapper;
using Infrastructure.Transaction.Services.Models;
using Infrastructure.Transaction.Services.Workers;
using MediatR;
using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Handlers
{
    public class CreateTransactionHandler : IRequestHandler<CreateTransactionRequest, CreateTransactionResponse>
    {
        private readonly IMapper _mapper;
        private readonly ITransactionWorker _worker;

        public CreateTransactionHandler(ITransactionWorker worker, IMapper mapper)
        {
            _worker = worker;
            _mapper = mapper;
        }

        public async Task<CreateTransactionResponse> Handle(CreateTransactionRequest request, CancellationToken token)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return _mapper.Map<CreateTransactionResponse>(await _worker.Create(_mapper.Map<TransactionRequest>(request)));
        }
    }
}