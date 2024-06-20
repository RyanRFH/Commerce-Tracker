using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_backend.Models;

namespace commerce_tracker_v2.Models
{
    public class BasketItem
    {
        public string BasketItemId { get; set; } = Guid.NewGuid().ToString();
        public string BasketId { get; set; }
        public Basket Basket { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}