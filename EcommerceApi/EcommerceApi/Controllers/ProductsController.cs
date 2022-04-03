using EcommerceApi.Data;
using EcommerceApi.DTO;
using EcommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : Controller
    {

        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] ProductDTO productDTO)
        {
            try
            {
                Product product = new Product();

                product.Name = productDTO.Name;
                product.Price = productDTO.Price;
                product.Description = productDTO.Description;
                product.Quantity = productDTO.Quantity;

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return Ok(product);
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        
        public async Task<IActionResult> Index(int page=1, int perPage=2)
        {
            var products = await _context.Products.OrderByDescending(p => p.Id)
                .Skip((page-1)*perPage).Take(perPage)
                .AsNoTracking().ToListAsync();

            return Ok(products);
        }
    }
}
