using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using commerce_tracker_v2.Models;

namespace dotnet_backend.Models
{
    [Table("Products")]
    public class Product
    {
        public string ProductId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public List<Order> Orders { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}