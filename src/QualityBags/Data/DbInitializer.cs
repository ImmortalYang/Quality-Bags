using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QualityBags.Data;
using QualityBags.Models;

namespace QualityBags.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            //Look for any Products
            if (context.Products.Any())
            {
                return; //DB has been seeded
            }

            var categories = new Category[]
            {
                new Category {CategoryName="Women's Bags", Description="Bags designed for women" }
            };
            foreach(Category category in categories)
            {
                context.Categories.Add(category);
            }
            context.SaveChanges();

            var suppliers = new Supplier[]
            {
                new Supplier {SupplierName="EcoBags NZ", ContactNumber=092799919, EmailAddress="sales@ragbag.co.nz" }
            };
            foreach(Supplier supplier in suppliers)
            {
                context.Suppliers.Add(supplier);
            }
            context.SaveChanges();

            var products = new Product[]
            {
                new Product {CategoryID=1, SupplierID=1, ProductName="Lv", UnitPrice=1200M, Description="Lv bag", ImagePath="" },
                new Product {CategoryID=1, SupplierID=1, ProductName="gucci", UnitPrice=9800M, Description="gucci bag", ImagePath="" }
            };
            foreach(Product product in products)
            {
                context.Products.Add(product);
            }
            context.SaveChanges();
        }
    }
}
