using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commerce_tracker_v2.Data;
using commerce_tracker_v2.Dto;
using commerce_tracker_v2.Helpers;
using commerce_tracker_v2.Models;
using dotnet_backend.Models;
using Microsoft.AspNetCore.Authorization;
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
                return NotFound("User not found:" + request.UserId);
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

            return Ok(newOrder);

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrders([FromQuery] OrderQueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ChangeTracker.LazyLoadingEnabled = false;

            //Using AsNoTracking so that products in the order dont include their respective orders list too, makes data hard to read/understand
            var orders = _context.Orders.AsNoTracking().Include(p => p.Products).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.OrderId))
            {
                orders = orders.Where(o => o.OrderId.Equals(query.OrderId));
            }

            if (!string.IsNullOrWhiteSpace(query.UserId))
            {
                orders = orders.Where(o => o.UserId.Equals(query.UserId));
            }

            int skipNumber = 0;
            if (query.PageNumber > 0)
            {
                skipNumber = (query.PageNumber - 1) * query.PageSize;
            }

            var queriedOrders = await orders.Skip(skipNumber).Take(query.PageSize).ToListAsync();

            return Ok(queriedOrders);

        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder([FromBody] string id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _context.Orders.FirstOrDefaultAsync(p => p.OrderId == id);

            if (order == null)
            {
                return NotFound("Order does not exist: " + id);
            }

            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();

            return Ok(order);
        }


        // [HttpPut]
        // public async Task<IActionResult> UpdateOrder([FromBody] List<string> productIds, string orderId)
        // {

        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);

        //     if (order == null)
        //     {
        //         return NotFound("Order not found: " + orderId);
        //     }

        //     var allProductIds = _context.Products.Select(p => p.ProductId).ToList();

        //     List<Product> requestProducts = new List<Product>();

        //     foreach (string Id in productIds)
        //     {
        //         if (!allProductIds.Contains(Id))
        //         {
        //             return NotFound("Product Id not found in database: " + Id);
        //         }

        //         var currentProduct = _context.Products.FirstOrDefault(p => p.ProductId == Id);
        //         requestProducts.Add(currentProduct);
        //     }

        //     order.Products = requestProducts;

        //     await _context.SaveChangesAsync();

        //     return Ok(order);
        // }



    }
}