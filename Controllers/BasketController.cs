using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commerce_tracker_v2.Data;
using commerce_tracker_v2.Models;
using dotnet_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace commerce_tracker_v2.Controllers
{
    [Route("api/basket")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly DataContext _context;

        public BasketController(DataContext context)
        {
            _context = context;
        }

        //TEST GET BASKET
        [HttpGet]
        // [Authorize]
        public async Task<IActionResult> GetBasket(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var basket = await _context.Baskets.Include(b => b.BasketItems).ThenInclude(i => i.Product).FirstOrDefaultAsync(b => b.UserId.Equals(userId));

            if (basket == null)
            {
                return NotFound("Basket not found");
            }

            return Ok(basket);

        }

        [HttpPost]
        // [Authorize]
        public async Task<IActionResult> CreateBasket(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var basket = new Basket
            {
                UserId = userId
            };

            _context.Baskets.Add(basket);

            await _context.SaveChangesAsync();

            return Ok(basket);

        }

        [HttpPost]
        [Route("additem")]
        public async Task<IActionResult> AddItemToBasket(string userId, string productId, int quantity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var basket = await _context.Baskets.Include(b => b.BasketItems).ThenInclude(i => i.Product).FirstOrDefaultAsync(b => b.UserId == userId);

            if (basket == null)
            {
                var newBasket = new Basket
                {
                    UserId = userId
                };
                _context.Baskets.Add(newBasket);
                await _context.SaveChangesAsync();
            }

            basket = await _context.Baskets.Include(b => b.BasketItems).ThenInclude(i => i.Product).FirstOrDefaultAsync(b => b.UserId == userId);


            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            var basketItem = new BasketItem
            {
                BasketItemId = basket.BasketId,
                Product = product,
                Quantity = quantity
            };

            basket.BasketItems.Add(basketItem);

            await _context.SaveChangesAsync();

            return Ok(basket);

        }


    }
}