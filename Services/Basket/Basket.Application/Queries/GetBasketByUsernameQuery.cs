using Basket.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Queries
{
    public class GetBasketByUsernameQuery : IRequest<ShoppingCartResponse>
    {
        public string Username { get; set; }
        public GetBasketByUsernameQuery(string username)
        {
            Username = username;
        }
    }
}
