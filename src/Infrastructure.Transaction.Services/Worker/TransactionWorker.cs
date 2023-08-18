using AutoMapper;
using Infrastructure.Transaction.Services.Models;
using Infrastructure.Transaction.Services.Processors;
using PaymentGateway.Data;

namespace Infrastructure.Transaction.Services.Worker
{
    public class TransactionWorker : ITransactionWorker
    {
        private ITransactionsRepository _repository;
        private IMapper _mapper;

        public TransactionWorker(ITransactionsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TransactionStatusResponse> Create(TransactionRequest request)
        {
            // Typically we would allow the system administrator to configure which Acquirer 
            // serves which corridors to take advantage of favourable rates, SLAs,
            // transaction load, clearing times etc;
            //
            // Here we will let the transaction itself determine which Acquirer processes it
            var processor = InstantiateProcessor(request.CardNumber);

            var response = await processor.ProcessTransaction(request);

            if (response.Status == TransactionStatus.Accepted)
            {
                // Prepare persistance
                var transaction = _mapper.Map<PaymentGateway.Data.Persistence.Entities.Transaction>((response, request));
                var recordsAffected = await _repository.AddTransaction(transaction);

                if(recordsAffected > 0)
                {
                    response.TransactionId = transaction.TransactionId;
                    return response;
                }
            }

            throw new Exception("CustomTransactionFailedException");
        }

        public async Task<TransactionResponse> Get(Guid id)
        {
            var transaction = await _repository.GetTransaction(id);

            if (transaction != null)
            {
                var processor = InstantiateProcessor(transaction.CardNumber);

                var response = await processor.RetrieveTransaction(transaction.TransactionId);

                // Transaction payout status updating should be part of a separate asynchronous queue
                transaction.Status = (int)response.Status;

                return _mapper.Map<TransactionResponse>(transaction);
            }

            throw new Exception("CustomTransactionNotFoundException");
        }

        private ITransactionProcessor InstantiateProcessor(string cardNumber)
        {
            return cardNumber switch
            {
                string s when s.StartsWith("1111") => new TransactionProcessorA(),
                string s when s.StartsWith("1234") => new TransactionProcessorC(),
                _ => new TransactionProcessorB(),
            };
        }
    }
}