using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commerce_tracker_v2.Data;
using commerce_tracker_v2.Dto;
using commerce_tracker_v2.Helpers;
using dotnet_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace commerce_tracker_v2.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct(ProductCreateDto request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Create product from request data
            var newProduct = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Quantity = request.Quantity,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return Ok(newProduct);
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryObject query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = _context.Products.Include(p => p.Orders).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.ProductId))
            {
                products = products.Where(p => p.ProductId.Equals(query.ProductId));
            }

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                products = products.Where(p => p.Name.Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                products = products.Where(p => p.Description.Contains(query.Description));
            }

            if (query.Quantity.HasValue)
            {
                products = products.Where(p => p.Quantity == query.Quantity);
            }

            if (query.Price.HasValue)
            {
                products = products.Where(p => p.Price == query.Price);
            }

            int skipNumber = 0;
            if (query.PageNumber > 0)
            {
                skipNumber = (query.PageNumber - 1) * query.PageSize;
            }

            var queriedProducts = await products.Skip(skipNumber).Take(query.PageSize).ToListAsync();

            return Ok(queriedProducts);

        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductCreateDto request, string Id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == Id);

            if (product == null)
            {
                return NotFound("Product not found: " + Id);
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Quantity = request.Quantity;
            product.Price = request.Price;
            product.ImageUrl = request.ImageUrl;

            await _context.SaveChangesAsync();

            return Ok(product);
        }


        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct([FromBody] string id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound("Product does not exist: " + id);
            }

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return Ok(product);
        }
    }



}