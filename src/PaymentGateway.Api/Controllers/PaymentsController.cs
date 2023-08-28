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
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentsController(ILogger<PaymentsController> logger, IMapper mapper, IMediator mediator, 
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
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

            var response = await _mediator.Send(request);

            _logger.LogInformation(response.ToString());

            return Created($"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}{_httpContextAccessor.HttpContext?.Request.Path}/{response}", 
                response);
        }

        [HttpGet("{transactionId}")]
        [Produces(typeof(GetTransactionResponse))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactionById(Guid transactionId)
        {
            var response = _mapper.Map<GetTransactionResponse>(
                await _mediator.Send(new GetTransactionRequest(transactionId)));

            _logger.LogInformation(response.ToString());
            return Ok(response);
        }
    }
}