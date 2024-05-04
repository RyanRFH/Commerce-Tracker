using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using commerce_tracker_v2.Models;

namespace dotnet_backend.Models
{
    [Table("Orders")]
    public class Order
    {
        public string OrderId { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public User User { get; set; }
        // public List<OrderProduct> OrderProduct { get; set; }
        public List<Product> Products { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}