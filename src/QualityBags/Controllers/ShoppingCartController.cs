using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QualityBags.Data;
using QualityBags.Models;
using Microsoft.EntityFrameworkCore;

namespace QualityBags.Controllers
{
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            // Retrieve the product from the database
            var productToAdd = await _context.Products
                .SingleOrDefaultAsync(p => p.ID == id);
            if(productToAdd == null)
            {
                return NotFound();
            }
            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(HttpContext);
            await cart.AddToCart(productToAdd, _context);
            // Go back to the main products page for more shopping
            return RedirectToAction("Index", "Products");
        }

        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int itemCount = await cart.RemoveFromCart(id, _context);
            return Redirect(Request.Headers["Referer"].ToString());
        }

    }
}