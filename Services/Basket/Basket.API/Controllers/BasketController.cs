using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    public class BasketController : APIController
    {
        private readonly IMediator _mediator;

        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
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
    }
}
