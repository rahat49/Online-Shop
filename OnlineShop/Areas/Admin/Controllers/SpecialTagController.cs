using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpecialTagController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View(_context.SpecialTags.ToList());
        }

        //Create Get

        public IActionResult Create()
        {
            return View();
        }
        //Create post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTag specialTag)
        {
            if (ModelState.IsValid)
            {
                _context.SpecialTags.Add(specialTag);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(specialTag);
        }

        //Edit 
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var specialTag = _context.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }
        //edit post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SpecialTag specialTag)
        {
            if (ModelState.IsValid)
            {
                _context.SpecialTags.Update(specialTag);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(specialTag);
        }
        //Details
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var specialTag = _context.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(SpecialTag specialTag)
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
            var specialTag = _context.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }
        //edit post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, SpecialTag specialTag)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != specialTag.Id)
            {
                return NotFound();
            }
            var st = _context.SpecialTags.Find(id);
            if (st == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Remove(st);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(st);
        }

    }
}
