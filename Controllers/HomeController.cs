using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Products.Models;

namespace Products.Controllers
{
    public class HomeController : Controller
    {
        private ProductsContext db;

        public HomeController(ProductsContext context)
        {
            db = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            List<Product> allProducts = db.Products
            .Include(p => p.Associations)
            .ThenInclude(sub => sub.Catagory)
            .ToList();
            ViewBag.products = allProducts;
            return View("Index");
        }

        [HttpGet("/details/product/{productId}")]
        public IActionResult Details(int productId)
        {
            Product product = db.Products
            .FirstOrDefault(p => p.ProductId == productId);
            
            ViewBag.ProductInCategories = db.Associations
            .Include(p=>p.Catagory)
            .Where(p=>p.ProductId == productId);

            ViewBag.ProductCategories = db.Categories
            .ToList()
            .Except(db.Categories
            .Where(c=>c.Associations
            .Any(a=>a.ProductId == productId)).ToList());
            return View("ProductDetails", product);
        }

        [HttpGet("/details/category/{categoryId}")]
        public IActionResult CategoryDetails(int categoryId)
        {
            Category category = db.Categories
            .FirstOrDefault(p => p.CatagoryId == categoryId);

            ViewBag.ProductInCategories = db.Associations
            .Include(p=>p.Product)
            .Where(p=>p.CatagoryId == categoryId);

            ViewBag.ProductCategories = db.Products
            .ToList()
            .Except(db.Products
            .Where(c=>c.Associations
            .Any(a=>a.CatagoryId == categoryId)).ToList());

            return View("CategoryDetails", category);
        }

        [HttpGet("/categories")]
        public IActionResult Categories()
        {
            List<Category> allCategories = db.Categories
            .Include(p => p.Associations)
            .ThenInclude(sub => sub.Catagory)
            .ToList();
            ViewBag.categories = allCategories;
            return View("Categories");
        }

        [HttpPost("/add/product")]
        public IActionResult AddProduct(Product newProduct)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(newProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<Product> allProducts = db.Products
            .Include(p => p.Associations)
            .ThenInclude(sub => sub.Catagory)
            .ToList();
            ViewBag.products = allProducts;
            return View("Index");
        }

        [HttpPost("/add/category")]
        public IActionResult AddCategory(Category newCategory)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(newCategory);
                db.SaveChanges();
                return RedirectToAction("Categories");
            }
            return View("Categories");
        }

        [HttpPost("/relate/product/{productId}")]
        public IActionResult RelateCategory(int productId, Association newAssociation)
        {
            newAssociation.ProductId = productId;
            db.Associations.Add(newAssociation);
            db.SaveChanges();
            return RedirectToAction("Details", new {productId = productId});
        }

        [HttpPost("/relate/category/{categoryId}")]
        public IActionResult RelateProduct(int categoryId, Association newAssociation)
        {
            newAssociation.CatagoryId = categoryId;
            db.Associations.Add(newAssociation);
            db.SaveChanges();
            return RedirectToAction("CategoryDetails", new {categoryId = categoryId});
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
