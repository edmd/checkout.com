using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Controllers
{
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ApiController]
    [Authorize]
    public class PaymentsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentsController(IMapper mapper, IMediator mediator, 
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Produces(typeof(CreateTransactionResponse))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTransaction([FromBody]CreateTransactionRequest request)
        {
            if (request == null) {
                return BadRequest();
            }

            var result = await _mediator.Send(request);

            return Created($"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}{_httpContextAccessor.HttpContext?.Request.Path}/{result}", result);
        }

        [HttpGet("{transactionId}")]
        [Produces(typeof(GetTransactionResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTransactionById(Guid transactionId)
        {
            var response = await _mediator.Send(new GetTransactionRequest(transactionId));

            var transactionResponse = _mapper.Map<GetTransactionResponse>(response);
            return Ok(transactionResponse);
        }
    }
}