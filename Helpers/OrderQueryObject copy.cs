using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace commerce_tracker_v2.Helpers
{
    public class OrderQueryObject
    {
        public string? OrderId { get; set; }
        public string? UserId { get; set; }
        public string? SortBy { get; set; }
        public bool IsDecsending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}