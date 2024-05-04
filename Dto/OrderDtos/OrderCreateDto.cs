using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using dotnet_backend.Models;

namespace commerce_tracker_v2.Dto
{
    public class OrderCreateDto
    {
        [Required]
        public string UserId { get; set; }
        public List<string> ProductsIds { get; set; }
    }
}