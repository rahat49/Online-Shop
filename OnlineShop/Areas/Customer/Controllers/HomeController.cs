using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;
using X.PagedList;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context; 
        }
        public IActionResult Index(int? page)
        {
            return View(_context.Products.Include(c=>c.ProductTypes).Include(c=>c.SpecialTag).ToList().ToPagedList(page??1,5)); 
        }
        //DETAIL
        public IActionResult Detail(int? id)
        {
            ViewData["ProTId"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["STId"] = new SelectList(_context.SpecialTags.ToList(), "Id", "SpecialTagName");
            if (id==null)
            {
                return NotFound();
            }
            var product = _context.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if(product == null) 
            {
                return NotFound();

            }
            return View(product);
        }
        [HttpPost]
        [ActionName("Detail")]
        public IActionResult ProductDetail(int? id)
        {
            List<Products> products = new List<Products>();
            if (id == null)
            {
                return NotFound();
            }
            var product = _context.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();

            }
            products = HttpContext.Session.Get<List<Products>>("products");
            if(products==null)
            {
                products = new List<Products>();
            }
            products.Add(product);
            HttpContext.Session.Set("products", products);
            return View(product);
        }
        
        [HttpPost]
        public IActionResult Remove(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if(products!=null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if(product!= null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction("Index");
        }
        //Get remove action method
        [ActionName("Remove")]
        public IActionResult RemoveToCart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction("Cart");
        }

        //Get product cart action method

        public IActionResult Cart()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if(products==null )
            {
                products = new List<Products>();
                
            }
            return View(products);
        }
    }
}
