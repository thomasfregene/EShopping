using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Core.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppingCartItems> Items { get; set; } = new List<ShoppingCartItems>();
        public ShoppingCart()
        {
            
        }

        public ShoppingCart(string username)
        {
            UserName = username;
        }
    }
}
