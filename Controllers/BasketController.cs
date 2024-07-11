using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commerce_tracker_v2.Data;
using commerce_tracker_v2.Dto.BasketDtos;
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

        [HttpGet]
        // [Authorize]
        public async Task<IActionResult> GetBasket(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var basket = await _context.Baskets.AsNoTracking().Include(b => b.BasketItems).ThenInclude(i => i.Product).FirstOrDefaultAsync(b => b.UserId.Equals(userId));

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
        public async Task<IActionResult> AddItemToBasket(AddItemToBasketDto newBasketItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == newBasketItem.UserId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var basket = await _context.Baskets.Include(b => b.BasketItems).ThenInclude(i => i.Product).FirstOrDefaultAsync(b => b.UserId == newBasketItem.UserId);

            if (basket == null)
            {
                var newBasket = new Basket
                {
                    UserId = newBasketItem.UserId
                };
                _context.Baskets.Add(newBasket);
                await _context.SaveChangesAsync();
                basket = newBasket;

            }


            // basket = await _context.Baskets.Include(b => b.BasketItems).ThenInclude(i => i.Product).FirstOrDefaultAsync(b => b.UserId == userId);


            if (basket == null)
            {
                return NotFound("Basket not found");
            }


            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == newBasketItem.ProductId);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            var basketItem = new BasketItem
            {
                BasketId = basket.BasketId,
                Product = product,
                Quantity = newBasketItem.Quantity
            };

            basket.BasketItems.Add(basketItem);

            await _context.SaveChangesAsync();

            return Ok(basket);

        }

        [HttpDelete]
        [Route("removeitem")]
        public async Task<IActionResult> RemoveItemFromBasket(string basketItemId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var basketItem = await _context.BasketItems.FirstOrDefaultAsync(bi => bi.BasketItemId == basketItemId);

            if (basketItem == null)
            {
                return NotFound("Basket item not found");
            }

            _context.BasketItems.Remove(basketItem);

            await _context.SaveChangesAsync();

            return Ok(basketItem);


        }

        [HttpDelete]
        [Route("clearbasket")]
        public async Task<IActionResult> ClearBasket(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var basket = await _context.Baskets.FirstOrDefaultAsync(b => b.User.Id == userId);

            if (basket == null)
            {
                return NotFound("User basket not found");
            }

            var basketItemIds = await _context.BasketItems.Where(bi => bi.BasketId == basket.BasketId).ToListAsync();

            if (basketItemIds.Any())
            {
                // Remove all basket items
                _context.BasketItems.RemoveRange(basketItemIds);
            }

            await _context.SaveChangesAsync();

            return Ok(userId);
        }

        [HttpPut]
        [Route("basketitem/updatequantity")]
        public async Task<IActionResult> UpdateBasketItemQuantity(string basketItemId, int quantity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var basketItem = await _context.BasketItems.FirstOrDefaultAsync(bi => bi.BasketItemId == basketItemId);

            if (quantity < 0)
            {
                return BadRequest("New basket item quantity must be more than 0");
            }

            if (basketItem == null)
            {
                return NotFound("Basket item not found");
            }

            basketItem.Quantity = quantity;

            await _context.SaveChangesAsync();

            return Ok(basketItem);






        }
    }
}