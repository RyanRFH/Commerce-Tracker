using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commerce_tracker_v2.Data;
using commerce_tracker_v2.Dto;
using commerce_tracker_v2.Models;
using dotnet_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace commerce_tracker_v2.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderCreateDto request)
        {
            var user = await _context.Users
                .Where(u => u.Id == request.UserId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found");
            }

            var requestProductIds = request.ProductsIds;

            var allProductIds = _context.Products.Select(p => p.ProductId).ToList();

            List<Product> requestProducts = new List<Product>();

            foreach (string Id in requestProductIds)
            {
                if (!allProductIds.Contains(Id))
                {
                    return NotFound("Product Id not found in database: " + Id);
                }

                var currentProduct = _context.Products.FirstOrDefault(p => p.ProductId == Id);
                requestProducts.Add(currentProduct);

            }

            //Create order from request data
            var newOrder = new Order
            {
                UserId = request.UserId,
                Products = requestProducts
            };

            _context.Orders.Add(newOrder);

            await _context.SaveChangesAsync();

            return Ok(newOrder.ToString());

        }

    }
}