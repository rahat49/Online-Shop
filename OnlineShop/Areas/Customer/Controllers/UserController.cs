using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        UserManager<IdentityUser> _userManager;
        ApplicationDbContext _context;
        public UserController(UserManager<IdentityUser>userManager, ApplicationDbContext context)
        { 
            _userManager= userManager;
            _context=context;
        }
        public IActionResult Index()
        {
            return View(_context.ApplicationUsers.ToList());
        }
        public async Task<IActionResult>Create()
        {
            return View();  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if(ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    var isSaveRole = await _userManager.AddToRoleAsync(user, "User");//make user role auto
                    TempData["Save"] = "User has been created successfully";
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
          
            return View();
        }
        //Edit 
        public async Task<IActionResult> Edit(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (id == null)
            {
                return NotFound();

            }
            return View(user);
        }
        // edit post method
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var userInfo = _context.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);

            if(userInfo == null)
            {
                return NotFound();
            }
            userInfo.FirstName = user.FirstName;
            userInfo.LastName = user.LastName;
            var result=await _userManager.UpdateAsync(userInfo);
            if (result.Succeeded)
            {
                TempData["Save"] = "User has been update successfully";
                return RedirectToAction("Index");
            }

            return View();
        }
        //Details
        public async Task<IActionResult> Detail(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (id == null)
            {
                return NotFound();

            }
            return View(user);
        }
        //Details
        public async Task<IActionResult> Delete(string id)
        {
           
            if (id == null)
            {
                return NotFound();

            }
            var user = _context.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            var userInfo = _context.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            userInfo.LockoutEnd= DateTime.Now.AddYears(100);
            int rowAffected = _context.SaveChanges();
            if(rowAffected> 0)
            {
                TempData["Save"] = "User has been Lockout successfully";
                return RedirectToAction("Index");
            }
            return View(userInfo);
        }
        public async Task<IActionResult>Active(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if(user == null )
            {
                return NotFound();
            }
            

            return View(user);
        }
        [HttpPost]

        public async Task<IActionResult> Active(ApplicationUser user)
        {
            var userInfo = _context.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            userInfo.LockoutEnd = DateTime.Now.AddDays(-1);
            int rowAffected = _context.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["Save"] = "User has been active successfully";
                return RedirectToAction("Index");
            }
            return View(userInfo);
        }

        public async Task<IActionResult> PermanentDelete(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }


            return View(user);
        }
        [HttpPost]

        public async Task<IActionResult> PermanentDelete(ApplicationUser user)
        {
            var userInfo = _context.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            _context.ApplicationUsers.Remove(userInfo);
            int rowAffected = _context.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["Save"] = "User has been delete parmanently successfully";
                return RedirectToAction("Index");
            }
            return View(userInfo);
        }
    }
}
