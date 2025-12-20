using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using fitcensys.Models;

namespace fitcensys.Controllers
{
    [Authorize] // Sadece giriş yapanlar girebilir
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: Profil Sayfası (Verileri Göster)
        public async Task<IActionResult> Index()
        {
            // Giriş yapan kullanıcıyı bul
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            return View(user);
        }

        // POST: Profil Güncelleme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ApplicationUser model)
        {
            // Giriş yapan gerçek kullanıcıyı veritabanından çekelim
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            // Formdan gelen verileri, gerçek kullanıcıya aktaralım
            // Not: Email ve Username'i değiştirtmiyoruz, güvenlik için.
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.BirthDate = model.BirthDate;
            user.Height = model.Height;
            user.Weight = model.Weight;

            // Telefonu da ekleyelim (IdentityUser'dan gelir)
            user.PhoneNumber = model.PhoneNumber;

            // Veritabanını güncelle
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("Index", model);
        }
    }
}