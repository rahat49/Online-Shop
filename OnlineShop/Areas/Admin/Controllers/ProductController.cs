using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Data.Migrations;
using OnlineShop.Models;
using System.IO;



namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Products.Include(p => p.ProductTypes).Include(s=>s.SpecialTag).ToList());
        }
        //Post Index action method
        [HttpPost]
        public IActionResult Index(decimal? lowAmount,decimal? largeamount)
        {
            var products=_context.Products.Include(p => p.ProductTypes).Include(s => s.SpecialTag).Where(c=>c.Price>=lowAmount && c.Price<=largeamount).ToList();
            if(lowAmount== null || largeamount== null)
            {
                products = _context.Products.Include(p => p.ProductTypes).Include(s => s.SpecialTag).ToList();
            }
            return View(products);
        }

        public IActionResult Create()
        {
            ViewData["ProTId"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["STId"] = new SelectList(_context.SpecialTags.ToList(), "Id", "SpecialTagName");
            return View();
        }
        //Create post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products products, IFormFile image)
        {
  
            if (image != null && image.Length > 0)
              {
               // Save the uploaded image to disk
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                   await image.CopyToAsync(stream);
                  products.Image = "Images/" + image.FileName;
                }
                if (image==null)
                {
                    products.Image = "Image/noimage.png";
                }
                 _context.Products.Add(products);
                 await _context.SaveChangesAsync();
                 return RedirectToAction("Index");
             }

            return View(products);
        }
        //Edit 
        public IActionResult Edit(int? id)
        {
            ViewData["ProTId"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["STId"] = new SelectList(_context.SpecialTags.ToList(), "Id", "SpecialTagName");
            if (id == null)
            {
                return NotFound();

            }
            var product = _context.Products.Include(c=>c.ProductTypes).Include(c=>c.SpecialTag)
                .FirstOrDefault(c=>c.Id==id);
            if (product== null)
            {
                return NotFound();
            }
            return View(product);
        }
       // edit post method
        [HttpPost]
        public async Task<IActionResult> Edit(Products products, IFormFile image)
        {
           if(ModelState.IsValid)
           {
                if (image != null)
                {
                    var im = Path.Combine(_env.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(im, FileMode.Create));

                    //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", image.FileName);
                    //using (var stream = new FileStream(filePath, FileMode.Create))
                    //await image.CopyToAsync(stream);
                    products.Image = "Images/" + image.FileName;   
                }
                if (image == null)
                {
                    products.Image = "Image/noimage.png";
                }
                _context.Products.Update(products);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
              }

            return View(products);
        }
        //Details
       
        public IActionResult Detail(int ? id)
        {
            ViewData["ProTId"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["STId"] = new SelectList(_context.SpecialTags.ToList(), "Id", "SpecialTagName");
            if (id==null)
            {
                return NotFound();
            }
            var product = _context.Products.Include(c=>c.ProductTypes).Include(c=>c.SpecialTag)
               .FirstOrDefault(c=>c.Id==id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //Delete
        public IActionResult Delete(int? id)
        {

            ViewData["ProTId"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["STId"] = new SelectList(_context.SpecialTags.ToList(), "Id", "SpecialTagName");
            if (id == null)
            {
                return NotFound();

            }
            var product = _context.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag)
                 .Where(c => c.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        //Delet post method
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
           return RedirectToAction("Index");
        }


    }
}
