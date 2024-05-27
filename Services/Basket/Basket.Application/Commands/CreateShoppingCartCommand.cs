using Basket.Application.Responses;
using Basket.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Commands
{
    public class CreateShoppingCartCommand : IRequest<ShoppingCartResponse>
    {
        public string Username { get; set; }
        public List<ShoppingCartItems> Items { get; set; }

        public CreateShoppingCartCommand(string username, List<ShoppingCartItems> items)
        {
            Username = username;
            Items = items;
        }
    }
}
