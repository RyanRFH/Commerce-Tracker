using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace commerce_tracker_v2.Dto.BasketDtos
{
    public class AddItemToBasketDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}