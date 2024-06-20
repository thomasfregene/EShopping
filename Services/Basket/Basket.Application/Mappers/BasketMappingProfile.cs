using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using EventBus.Messges.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Mapper
{
    public class BasketMappingProfile : Profile
    {
        public BasketMappingProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartResponse>().ReverseMap();
            CreateMap<ShoppingCartItems, ShoppingCartItemResponse>().ReverseMap();
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
            CreateMap<BasketCheckoutV2, BasketCheckoutEventV2>().ReverseMap();
        }
    }
}
