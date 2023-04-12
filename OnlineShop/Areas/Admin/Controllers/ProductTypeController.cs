using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Admin")]
    public class ProductTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductTypeController(ApplicationDbContext context)
        {
            _context= context;
        }
        public IActionResult Index()
        {

            return View(_context.ProductTypes.ToList());
        }

        //Create Get
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        //Create post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _context.ProductTypes.Add(productTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productTypes);
        }
        [Authorize]
        //Edit 
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var productType = _context.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }
        //edit post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes productTypes)
        {
            if (ModelState.IsValid)
            {
                _context.ProductTypes.Update(productTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productTypes);
        }
        //Details
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var productType = _context.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ProductTypes productTypes)
        {
            return RedirectToAction(nameof(Index));

        }
        //Delete
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var productType = _context.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }
        //Delet post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, ProductTypes productTypes)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != productTypes.Id)
            {
                return NotFound();
            }
            var productType = _context.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Remove(productType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }
    }
 }

