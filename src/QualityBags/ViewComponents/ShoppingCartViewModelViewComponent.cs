using Microsoft.AspNetCore.Mvc;
using QualityBags.Data;
using QualityBags.Models;
using QualityBags.Models.ShoppingCartViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBags.ViewComponents
{
    public class ShoppingCartViewModelViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartViewModelViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await GetCurrentCartViewModel());
        }

        public async Task<ShoppingCartViewModel> GetCurrentCartViewModel()
        {
            var cart = ShoppingCart.GetCart(HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = await cart.GetCartItems(_context),
                CartTotal = await cart.GetGrandTotal(_context)
            };
            return viewModel;
        }

    }
}
