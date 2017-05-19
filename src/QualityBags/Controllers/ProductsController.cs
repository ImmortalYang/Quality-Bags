using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityBags.Data;
using QualityBags.Models;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace QualityBags.Controllers
{
    [AllowAnonymous]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostEnv;

        public ProductsController(ApplicationDbContext context, IHostingEnvironment hEnv)
        {
            _context = context;
            _hostEnv = hEnv;  
        }

        // GET: Products
        public async Task<IActionResult> Index(
            string searchString, 
            string sortBy,
            string order, 
            decimal? minPrice, 
            decimal? maxPrice, 
            int? pageIndex, 
            int? pageSize
            )
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsNoTracking();
            //Apply filters
            //Filter: search string
            if(!string.IsNullOrWhiteSpace(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString));
                ViewBag.searchString = searchString;
            }
            //filter: sort and order
            if (order == "asc")
            {
                switch (sortBy)
                {
                    case "product name":
                        products = products.OrderBy(p => p.ProductName);
                        break;
                    case "category name":
                        products = products.OrderBy(p => p.Category.CategoryName);
                        break;
                    case "unit price":
                        products = products.OrderBy(p => p.UnitPrice);
                        break;
                    default:
                        break;
                }
            }
            else if(order == "dsc")
            {
                switch (sortBy)
                {
                    case "product name":
                        products = products.OrderByDescending(p => p.ProductName);
                        break;
                    case "category name":
                        products = products.OrderByDescending(p => p.Category.CategoryName);
                        break;
                    case "unit price":
                        products = products.OrderByDescending(p => p.UnitPrice);
                        break;
                    default:
                        break;
                }
            }
            ViewBag.order = new SelectList(new string[] { "asc", "dsc" }, order);
            ViewBag.sortBy = new SelectList(new string[] { "", "product name", "category name" , "unit price" }, sortBy);
            //filter: price range
            if(minPrice != null || maxPrice != null)
            {
                minPrice = minPrice ?? 0;
                products = products.Where(p => p.UnitPrice >= minPrice);
                if(maxPrice != null)
                {
                    products = products.Where(p => p.UnitPrice <= maxPrice);
                }
            }
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;
            ViewBag.pageSize = pageSize.GetValueOrDefault(3);
            var productsList = await PaginatedList<Product>.CreateAsync(
                    products, pageIndex??1, ViewBag.pageSize
                );
            

            if (User.IsInRole("Admin"))
            {
                return View("AdminIndex", productsList);
            }
            else
            {
                return View(productsList);
            }
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create(int? categoryId = null)
        {
            PopulateCategoriesAndSuppliersDropDownList(categoryId);
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("CategoryID,Description,ProductName,SupplierID,UnitPrice")] Product product, 
            IList<IFormFile> imgFiles)
        {
            
            product.ImagePath = await GetPathOfFile(imgFiles);

            try { 
                if (ModelState.IsValid)
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                //Log the error
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " + "see your system administrator.");

            }
            PopulateCategoriesAndSuppliersDropDownList(product);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            PopulateCategoriesAndSuppliersDropDownList(product);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPost(int? id, 
            IList<IFormFile> imgFiles)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var productToUpdate = await _context.Products
                .SingleOrDefaultAsync(p => p.ID == id);
            //if admin didn't upload any new file and old file already exists
            //then do not update image path
            if (!(imgFiles.Count < 1 && productToUpdate.ImagePath != ""))
            {
                //Delete old image
                System.IO.File.Delete(_hostEnv.WebRootPath + productToUpdate.ImagePath);
                productToUpdate.ImagePath = await GetPathOfFile(imgFiles);
            } 
            
            if(await TryUpdateModelAsync(
                productToUpdate, "", 
                p => p.CategoryID, p => p.SupplierID,  
                p => p.ProductName, p => p.Description, P => P.UnitPrice)
                )
            {
                try
                {
                    await _context.SaveChangesAsync(); //Update Database
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            
            PopulateCategoriesAndSuppliersDropDownList(productToUpdate);
            return View(productToUpdate);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. " +
                "Please delete the cart items and order details that are related to this product first. " + 
                "Try again, and if the problem persists " +
                "see your system administrator.";
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(m => m.ID == id);
            var imgPath = _hostEnv.WebRootPath + product.ImagePath;
            
            _context.Products.Remove(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            //Delete product image
            System.IO.File.Delete(imgPath);
            return RedirectToAction("Index");
        }

        // List of products under certain category
        // For Ajax call
        // GET: Products/List?categoryId=1
        public async Task<IActionResult> List(int? categoryId)
        {
            if(categoryId == null)
            {
                return NotFound();
            }
            var products = await _context.Products
                .Where(p => p.CategoryID == categoryId)
                .AsNoTracking()
                .ToListAsync();
            return PartialView("_List", products);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }

        /// <summary>
        /// Populate the data for categories and suppliers selection using an instance of product
        /// </summary>
        /// <param name="product">The current product item being operated</param>
        private void PopulateCategoriesAndSuppliersDropDownList(Product product = null)
        { 
            ViewData["CategoryID"] = new SelectList(_context.Categories.AsNoTracking(), 
                "ID", "CategoryName", product?.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers.AsNoTracking(), 
                "ID", "SupplierName", product?.SupplierID);
        }

        /// <summary>
        /// Populate the data for categories and suppliers selection using given category and supplier id
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="supplierId"></param>
        private void PopulateCategoriesAndSuppliersDropDownList(int? categoryId = null, int? supplierId = null)
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories.AsNoTracking(),
                "ID", "CategoryName", categoryId);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers.AsNoTracking(),
                "ID", "SupplierName", supplierId);
        }

        /// <summary>
        /// Save the files to directory and return the path of the file
        /// </summary>
        /// <param name="_files"></param>
        /// <returns></returns>
        private async Task<string> GetPathOfFile(IList<IFormFile> _files)
        {
            string relativeName = "";
            string fileName = "";
            if (_files.Count < 1)
            {
                relativeName = "/images/ProductImages/Default.jpg";
            }
            else
            {
                foreach (IFormFile file in _files)
                {
                    fileName = ContentDispositionHeaderValue
                        .Parse(file.ContentDisposition)
                        .FileName.Trim('"');
                    relativeName = "/Images/ProductImages/"
                        + DateTime.Now.ToString("ddMMyyyy-HHmmssffffff")
                        + fileName;
                    using (FileStream fs = System.IO.File.Create(_hostEnv.WebRootPath + relativeName))
                    {
                        await file.CopyToAsync(fs);
                        await fs.FlushAsync();
                    }

                }
            }
            return relativeName;
        }

    }
}
