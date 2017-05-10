using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityBags.Data;
using QualityBags.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;

namespace QualityBags.Controllers
{
    [Authorize(Roles = ("Admin,Member"))]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;   
        }

        // GET: Orders
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var orders = _context.Orders
                .Include(o => o.ApplicationUser)
                .AsNoTracking();
            return View(await orders.ToListAsync());
        }

        // GET: Orders/Create
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Create()
        {
            var order = new Order();
            var user = await _userManager.GetUserAsync(User);
            order.ApplicationUser = user;
            order.Address = user.Address;
            order.PhoneNumber = user.PhoneNumber;

            ShoppingCart cart = ShoppingCart.GetCart(HttpContext);
            order.GrandTotal = await cart.GetGrandTotal(_context);
            order.GST = await cart.GetGSTAmount(_context);
            order.Subtotal = await cart.GetSubTotal(_context);
            return View(order);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Create([Bind("PhoneNumber,Address,City,State,Country,PostalCode")] Order order)
        {
            var user = await _userManager.GetUserAsync(User);
            order.ApplicationUser = user;
            if (ModelState.IsValid)
            {
                ShoppingCart cart = ShoppingCart.GetCart(HttpContext);
                order.GrandTotal = await cart.GetGrandTotal(_context);
                order.GST = await cart.GetGSTAmount(_context);
                order.Subtotal = await cart.GetSubTotal(_context);
                List<CartItem> items = await cart.GetCartItems(_context);
                List<OrderDetail> details = new List<OrderDetail>();
                foreach (CartItem item in items)
                {
                    OrderDetail detail = CreateOrderDetailFromCartItem(item);
                    detail.Order = order;
                    details.Add(detail);
                    _context.Add(detail);
                }
                order.OrderDetails = details;
                order.OrderDate = DateTime.Now;
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Purchased", new RouteValueDictionary(
                new { action = "Purchased", id = order.ID }));

            }
            return View(order);
        }

        /// <summary>
        /// Create an order detail record from a given cart item.
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        private OrderDetail CreateOrderDetailFromCartItem(CartItem cartItem)
        {
            OrderDetail detail = new OrderDetail();
            detail.Quantity = cartItem.Count;
            detail.Product = cartItem.Product;
            detail.UnitPrice = cartItem.Product.UnitPrice;
            return detail;
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Purchased(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context
                .Orders.Include(o => o.ApplicationUser)
                .AsNoTracking()
                .SingleOrDefaultAsync(o => o.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            var details = await _context.OrderDetails
                .Where(detail => detail.Order.ID == order.ID)
                .Include(detail => detail.Product)
                .ToListAsync();

            order.OrderDetails = details;
            ShoppingCart.GetCart(HttpContext).EmptyCart(_context);
            return View(order);
        }


        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.ApplicationUser)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }
            order.OrderDetails = await _context.OrderDetails
                .Where(detail => detail.Order.ID == order.ID)
                .Include(detail => detail.Product)
                .ToListAsync();
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders
                .SingleOrDefaultAsync(o => o.ID == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
