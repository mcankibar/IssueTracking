using IssueTracker.Areas.Identity.Data;
using IssueTracker.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly DBContextSample _dBContextSample;
        private readonly UserManager<SampleUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<SampleUser> userManager, RoleManager<IdentityRole> roleManager, DBContextSample dBContextSample)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dBContextSample = dBContextSample;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
           
            return View(users);
        }


        [HttpPost]
        public async Task<IActionResult> EditRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Failed to remove existing roles.");
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(role))
            {
                result = await _userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Failed to add new role.");
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string userId)
        {
            // Guid'e çevirme 
            var userIdGuid = new Guid(userId);

            var user = await _userManager.FindByIdAsync(userId);
           
            user.isDeleted = true;
            user.UserName = user.UserName + "[Deleted]";
           _dBContextSample.SaveChanges();
            if (user.isDeleted)
            {
                return RedirectToAction("Index");
            }
            else
            {
                
                return RedirectToAction("Error");
            }
        }










    }
}
