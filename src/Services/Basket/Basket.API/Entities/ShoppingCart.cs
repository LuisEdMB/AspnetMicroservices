using System.Collections.Generic;

namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string  UserName { get; set; }

        public List<ShoppingCartItem> Items { get; set; }

        public ShoppingCart()
        {
            Items = new List<ShoppingCartItem>();
        }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice
        {
            get
            {
                var totalPrice = 0m;
                Items?.ForEach(g => {
                    totalPrice += g.Price * g.Quantity;
                });
                return totalPrice;
            }
        }
    }
}
