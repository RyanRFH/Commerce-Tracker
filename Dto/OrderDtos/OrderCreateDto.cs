using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using commerce_tracker_v2.Dto.OrderDtos;
using commerce_tracker_v2.Models;
using dotnet_backend.Models;

namespace commerce_tracker_v2.Dto
{
    public class OrderCreateDto
    {
        [Required]
        public string UserId { get; set; }
        public List<OrderItemCreateDto> OrderItemCreateDtos { get; set; }

    }
}