using Basket.Application.Commands;
using Basket.Application.Mapper;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Common.Logging.Correlation;
using EventBus.Messges.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    public class BasketController : APIController
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BasketController> _logger;
        private readonly ICorrelationIdGenerator _correlationIdGenerator;

        public BasketController(IMediator mediator, IPublishEndpoint publishEndpoint,ILogger<BasketController> logger,  ICorrelationIdGenerator correlationIdGenerator)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _correlationIdGenerator = correlationIdGenerator;
            _logger.LogInformation("CorrelationId {correlationId}:", _correlationIdGenerator.Get());
            
        }

        [HttpGet]
        [Route("[action]/{username}", Name = "GetBasketByUsername")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> GetBasketByUsername(string username)
        {
            var query = new GetBasketByUsernameQuery(username);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }

        [HttpPost]
        [Route("CreateBasket")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> CreateBasket([FromBody] CreateShoppingCartCommand createShoppingCartCommand)
        {
            
            var basket = await _mediator.Send(createShoppingCartCommand);
            return Ok(basket);
        }

        [HttpDelete]
        [Route("[action]/{username}", Name = "DeleteBasketByUsername")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> DeleteBasketByUsername(string username)
        {
            var query = new DeleteBasketByUsernameCommand(username);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]

        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            //get basket existing with username
            var query = new GetBasketByUsernameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if (basket == null)
            {
                return BadRequest();
            }
            var eventMsg = BasketMapper.Mapper.Map<BasketCheckoutEvent>(basketCheckout);
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
