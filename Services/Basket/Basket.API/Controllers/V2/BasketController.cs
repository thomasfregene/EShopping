using Basket.Application.Commands;
using Basket.Application.Mapper;
using Basket.Application.Queries;
using Basket.Core.Entities;
using Common.Logging.Correlation;
using EventBus.Messages.Events;
using EventBus.Messges.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers.V2
{
    [ApiVersion("2")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BasketController> _logger;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;

        public BasketController(IMediator mediator, IPublishEndpoint publishEndpoint, ILogger<BasketController> logger, ICorrelationIdGenerator correlationIdGenerator)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _correlationIdGenerator = correlationIdGenerator;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _correlationIdGenerator = correlationIdGenerator;
            _logger.LogInformation("CorrelationId {correlationId}:", _correlationIdGenerator.Get());
            _mediator = mediator;
        }

        [Route("[action]")]
        [HttpPost]
        [MapToApiVersion("2")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]

        public async Task<IActionResult> Checkout([FromBody] BasketCheckoutV2 basketCheckout)
        {
            //get basket existing with username
            var query = new GetBasketByUsernameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if (basket == null)
            {
                return BadRequest();
            }
            var eventMsg = BasketMapper.Mapper.Map<BasketCheckoutEventV2>(basketCheckout);
            eventMsg.TotalPrice = basket.TotalPrice;
            eventMsg.CorrelationId = _correlationIdGenerator.Get();
            await _publishEndpoint.Publish(eventMsg);
            //remove basket
            var deleteCmd = new DeleteBasketByUsernameCommand(basketCheckout.UserName);
            await _mediator.Send(deleteCmd);
            // Console.ReadKey();
            return Accepted();
        }


    }
}
