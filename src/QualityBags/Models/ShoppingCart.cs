using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QualityBags.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace QualityBags.Models
{
    public class ShoppingCart
    {
        /// <summary>
        /// A unique Id of the shopping cart
        /// </summary>
        public string ShoppingCartId { get; set; }
        public const string CartSessionKey = "cartId";
        /// <summary>
        /// Represents the current GST rate
        /// </summary>
        public static readonly decimal GSTRate = 0.15M;

        /// <summary>
        /// Get the shopping cart from the current HTTP context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ShoppingCart GetCart(HttpContext context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        /// <summary>
        /// Add a product to shopping cart and save to database asynchronously.
        /// If the product already exists in the shopping cart, simply increase the count by 1.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="appContext"></param>
        public async Task AddToCart(Product product, ApplicationDbContext appContext)
        {
            var cartItem = await appContext.CartItems
                .SingleOrDefaultAsync(c => c.CartID == ShoppingCartId && c.Product.ID == product.ID);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    Product = product,
                    CartID = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                appContext.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }
            await appContext.SaveChangesAsync();
        }

        /// <summary>
        /// Remove a product from the shopping cart asynchronously.
        /// If product count is 0, remove the item from the shopping cart.
        /// </summary>
        /// <param name="productId">The id of the product</param>
        /// <param name="appContext"></param>
        /// <returns>The count of products remained in this item</returns>
        public async Task<int> RemoveFromCart(int productId, ApplicationDbContext appContext)
        {
            var cartItem = await appContext.CartItems
                .SingleOrDefaultAsync(cart => cart.CartID == ShoppingCartId && cart.Product.ID == productId);
            int itemCount = 0;
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    appContext.CartItems.Remove(cartItem);
                }
                await appContext.SaveChangesAsync();
            }
            return itemCount;
        }

        /// <summary>
        /// Remove all the cart items within the shopping cart in the current session asynchronously.
        /// </summary>
        /// <param name="appContext"></param>
        public async void EmptyCart(ApplicationDbContext appContext)
        {
            var cartItems = appContext.CartItems
                .Where(cart => cart.CartID == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                appContext.CartItems.Remove(cartItem);
            }
            await appContext.SaveChangesAsync();
        }

        /// <summary>
        /// Get a list of all the cart items within the shopping cart asynchronously
        /// </summary>
        /// <param name="appContext"></param>
        /// <returns></returns>
        public async Task<List<CartItem>> GetCartItems(ApplicationDbContext appContext)
        {
            List<CartItem> cartItems = await appContext
                .CartItems.Include(i => i.Product)
                .Where(cartItem => cartItem.CartID == ShoppingCartId)
                .ToListAsync();
            return cartItems;
        }

        /// <summary>
        /// Get the count of products within the shopping cart.
        /// </summary>
        /// <param name="appContext"></param>
        /// <returns></returns>
        public async Task<int> GetCount(ApplicationDbContext appContext)
        {
            int? count = await appContext.CartItems
                .Where(c => c.CartID == ShoppingCartId)
                .Select(c => (int?)c.Count)
                .SumAsync();
            /*
                (from cartItems in appContext.CartItems
                 where cartItems.CartID == ShoppingCartId
                 select (int?)cartItems.Count)
                 .Sum();
                 */
            return count ?? 0;
        }

        /// <summary>
        /// Get the grand total of the shopping cart.
        /// </summary>
        /// <param name="appContext"></param>
        /// <returns></returns>
        public async Task<decimal> GetGrandTotal(ApplicationDbContext appContext)
        {
            decimal? total = await (from cartItem in appContext.CartItems
                              where cartItem.CartID == ShoppingCartId
                              select (int?)cartItem.Count * cartItem.Product.UnitPrice)
                              .SumAsync();
            return total ?? decimal.Zero;
        }

        /// <summary>
        /// Get GST amount of the shopping cart asynchronously.
        /// </summary>
        /// <param name="appContext"></param>
        /// <returns></returns>
        public async Task<decimal> GetGSTAmount(ApplicationDbContext appContext)
        {
            return await GetGrandTotal(appContext) * GSTRate/(1+GSTRate);
        }

        /// <summary>
        /// Get the subtotal of the shopping cart asynchronously.
        /// </summary>
        /// <param name="appContext"></param>
        /// <returns></returns>
        public async Task<decimal> GetSubTotal(ApplicationDbContext appContext)
        {
            return await GetGrandTotal(appContext) / (1+GSTRate);
        }

        /// <summary>
        /// Get the unique id of the shopping cart in the current session
        /// </summary>
        /// <param name="context">Current HTTP context</param>
        /// <returns>Shopping cart id stored in the current session</returns>
        public string GetCartId(HttpContext context)
        {
            if (context.Session.GetString(CartSessionKey) == null)
            {
                Guid tempCartId = Guid.NewGuid();
                context.Session.SetString(CartSessionKey, tempCartId.ToString());
            }
            return context.Session.GetString(CartSessionKey).ToString();
        }

    }
}
