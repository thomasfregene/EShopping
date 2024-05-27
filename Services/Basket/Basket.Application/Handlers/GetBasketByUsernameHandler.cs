using Basket.Application.Mapper;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Handlers
{
    public class GetBasketByUsernameHandler : IRequestHandler<GetBasketByUsernameQuery, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketRepository;

        public GetBasketByUsernameHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        public async Task<ShoppingCartResponse> Handle(GetBasketByUsernameQuery request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _basketRepository.GetBasket(request.Username);
            var shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(shoppingCart);
            return shoppingCartResponse;
        }
    }
}
