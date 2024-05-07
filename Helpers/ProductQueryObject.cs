using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace commerce_tracker_v2.Helpers
{
    public class ProductQueryObject
    {
        public string? ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? SortBy { get; set; }
        public bool IsDecsending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}