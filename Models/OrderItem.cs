using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_backend.Models;

namespace commerce_tracker_v2.Models
{
    public class OrderItem
    {
        public string OrderItemId { get; set; } = Guid.NewGuid().ToString();

        public string OrderId { get; set; }
        public Order Order { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }


    }
}