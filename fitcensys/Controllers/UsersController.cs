using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using fitcensys.Models;

namespace fitcensys.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece Admin girebilir!
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: /Users
        public async Task<IActionResult> Index()
        {
            // Veritabanındaki tüm kullanıcıları listeye çevirip getir
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
    }
}