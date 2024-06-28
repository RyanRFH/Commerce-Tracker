using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_backend.Models;

namespace commerce_tracker_v2.Dto.OrderDtos
{
    public class OrderItemCreateDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}