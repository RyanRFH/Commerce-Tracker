using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commerce_tracker_v2.Data;
using commerce_tracker_v2.Dto;
using commerce_tracker_v2.Dto.OrderDtos;
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
        [Route("create")]
        public async Task<IActionResult> CreateOrder(OrderCreateDto request)
        {
            //Create order from request data
            var user = await _context.Users
                .Where(u => u.Id == request.UserId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found:" + request.UserId);
            }


            var newOrder = new Order
            {
                UserId = request.UserId,
            };

            // List<string> requestProductIds = request.ProductIds;

            var allProductIds = _context.Products.Select(p => p.ProductId).ToList();

            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (OrderItemCreateDto orderItemDto in request.OrderItemCreateDtos)
            {
                if (!allProductIds.Contains(orderItemDto.ProductId))
                {
                    return NotFound("Product Id not found in database: " + orderItemDto.ProductId);
                }

                var currentProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == orderItemDto.ProductId);

                if (currentProduct == null)
                {
                    return NotFound("Product Id not found in database: " + orderItemDto.ProductId);
                }

                var currentOrderItem = new OrderItem
                {
                    OrderId = newOrder.OrderId,
                    ProductId = currentProduct.ProductId,
                    Quantity = orderItemDto.Quantity
                };

                await _context.OrderItems.AddAsync(currentOrderItem);
                orderItems.Add(currentOrderItem);
            }

            await _context.Orders.AddAsync(newOrder);

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
            var orders = _context.Orders.AsNoTracking().Include(p => p.OrderItems).ThenInclude(oi => oi.Product).AsQueryable();

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

        [HttpPost]
        [Route("createfrombasket")]
        public async Task<IActionResult> CreateOrderFromBasket(string userId)
        {

            //Create order from users basket
            var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found:" + userId);
            }

            var newOrder = new Order
            {
                UserId = userId
            };

            var orderItems = new List<OrderItem>();

            var basket = await _context.Baskets.Include(b => b.BasketItems).FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null)
            {
                return NotFound("Users basket not found");
            }

            foreach (var basketItem in basket.BasketItems)
            {
                var newOrderItem = new OrderItem
                {
                    OrderId = newOrder.OrderId,
                    ProductId = basketItem.ProductId,
                    Quantity = basketItem.Quantity
                };

                orderItems.Add(newOrderItem);
                await _context.OrderItems.AddAsync(newOrderItem);
            }
            newOrder.OrderItems = orderItems;

            await _context.Orders.AddAsync(newOrder);

            await _context.SaveChangesAsync();

            return Ok(newOrder);

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