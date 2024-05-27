﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Responses
{
    public class ShoppingCartResponse
    {
        public string Username { get; set; }
        public List<ShoppingCartItemResponse> Items { get; set; }
        public ShoppingCartResponse()
        {
            
        }
        public ShoppingCartResponse(string username)
        {
            Username = username;
        }

        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                foreach (var item in Items)
                {
                    totalPrice+= item.Price*item.Quantity;
                }
                return totalPrice;
            }
        }
    }
}
