using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<IdentityUser> _userManager;
        ApplicationDbContext _context;

        public RoleController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context,
        UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _context = context;
            _userManager= userManager;


        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles;
            return View();
        }
        //Create role get method
        public IActionResult Create()
        {

            return View();
        }
        //Create post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name)
        {
            IdentityRole role = new IdentityRole();
            role.Name = name;
            var isExist = await _roleManager.RoleExistsAsync(role.Name);
            if (isExist)
            {
                ViewBag.msg = "This role is already exist";
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                TempData["Save"] = "Role has been saved successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        //Edit role get method
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();

            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View(role);
        }
        //Create post method
        [HttpPost]

        public async Task<IActionResult> Edit(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();

            }
            role.Name = name;
            var isExist = await _roleManager.RoleExistsAsync(role.Name);
            if (isExist)
            {
                ViewBag.msg = "This role is already exist";
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["Save"] = "Role has been update successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        //Detail  role get method
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();

            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View(role);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();

            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Save"] = "Role has been deleted successfully";
                return RedirectToAction("Index");
            }

            return View(role);
        }
        public async Task<IActionResult> Assign(string id)
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers.Where(f=>f.LockoutEnd<DateTime.Now||f.LockoutEnd==null), "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_context.Roles.ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Assign(RolesUserVm rolesUser)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(c => c.Id == rolesUser.UserId);
            var isCheckedRoleAssign= await _userManager.IsInRoleAsync(user, rolesUser.RoleId);
            if(isCheckedRoleAssign )
            {
                ViewBag.msg= "This User already Assigned this role.";
                return View();
            }
            
            var role = await _userManager.AddToRoleAsync(user,rolesUser.RoleId);
            if(role == null)
            {
                TempData["Save"] = "User Role Assigned.";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult AssignUserRole()
        {
            //join querey to see the who is assign for which role
            var result = from ur in _context.UserRoles
                         join r in _context.Roles on ur.RoleId equals r.Id
                         join a in _context.ApplicationUsers on ur.UserId equals a.Id
                         select new UserRoleMapping()
                         {
                             UserId = ur.UserId,
                             RoleId = ur.RoleId,
                             UserName = a.UserName,
                             RoleName = r.Name
                         };
            ViewBag.UserRoles = result;
            return View();
        }
    }
}
