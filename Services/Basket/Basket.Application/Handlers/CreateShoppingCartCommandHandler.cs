﻿using Basket.Application.Commands;
using Basket.Application.Mapper;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Handlers
{
    public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketRepository;

        public CreateShoppingCartCommandHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            //TODO: Call Discount service and apply coupons
            try
            {
                var shoppingCart = await _basketRepository.UpdateBasket(new ShoppingCart
                {
                    UserName = request.Username,
                    Items = request.Items
                });
                var shoppingCartResponse = BasketMapper.Mapper.Map<ShoppingCartResponse>(shoppingCart);
                return shoppingCartResponse;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
