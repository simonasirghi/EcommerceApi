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
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        
        public async Task<IActionResult> Index(string buyerID)
        {


            var cart = await _context.Carts.Where(c => c.BuyerToker == buyerID).ToListAsync();


            return Ok(cart);
        }

        [HttpPost]
        [Route("/create")]
        public async Task<IActionResult> Create(CartDTO cartDTO)
        {
            Cart cart = new Cart();
            cart.BuyerToker = cartDTO.BuyerToker;
            cart.Quantity = cartDTO.Quantity;
            cart.ProductId = cartDTO.ProductId;

            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

            return Ok(cart);
        }

        

    }
}
