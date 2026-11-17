using System.Diagnostics;
using Inventory_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Inventory_Management.Data;

namespace Inventory_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string searchString, string category, string sortOrder)
        {
            List<Product> products = _context.Products.ToList();
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category).ToList();
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (sortOrder == "name")
                {
                products = products.OrderBy(p => p.Name).ToList();
            }
            else if (sortOrder == "price")
            {
                products = products.OrderBy(p => p.Price).ToList();
            }
            else if (sortOrder == "quantity")
            {
                products = products.OrderBy(p => p.Quantity).ToList();
            }
                int totalQuantity = 0;

            foreach (var product in products)
            {
                totalQuantity += product.Quantity;
            }

            ViewBag.TotalProducts = products.Count;
            ViewBag.TotalQuantity = totalQuantity;
            ViewBag.LowStockProducts = products.Count(p => p.Quantity < 10);
            ViewBag.Categories = _context.Products.Select(p => p.Category).Distinct().ToList();
            ViewBag.SearchString = searchString;
            ViewBag.SelectedCategory = category;
            ViewBag.SortOrder = sortOrder;
            return View(products);
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }


            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult EditProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _context.Products.Update(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]

        public IActionResult DeleteProductConfirmed(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
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
