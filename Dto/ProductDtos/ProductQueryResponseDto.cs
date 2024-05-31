using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using dotnet_backend.Models;

namespace commerce_tracker_v2.Dto.ProductDtos
{
    public class ProductQueryResponseDto
    {
        public List<Product> ProductsList { get; set; } = new List<Product>();
        public int PageNumber { get; set; }

        public int TotalProductCount { get; set; }
    }
}