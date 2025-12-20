using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using fitcensys.Models;

namespace fitcensys.Controllers
{
    // Bu bir API Controller'dır (View döndürmez, Veri döndürür)
    [Route("api/[controller]")]
    [ApiController]
    public class GymsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GymsApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/GymsApi
        // Tüm salonları, çalışma saatleriyle ve hoca sayılarıyla getirir
        [HttpGet]
        public async Task<IActionResult> GetGyms()
        {
            // LINQ SORGU ÖRNEĞİ (Hocanın istediği teknik kısım burası)
            // Tüm veriyi çekmek yerine sadece lazım olanları seçiyoruz (Select)
            var gyms = await _context.Gyms
                .Select(g => new
                {
                    Id = g.GymID,
                    SalonAdi = g.Name,
                    Adres = g.Address,
                    EgitmenSayisi = g.Trainers.Count(), // Count sorgusu
                    HizmetSayisi = g.GymServices.Count(),
                    // Çalışma saatlerini de alalım
                    CalismaSaatleri = g.WorkingHours.Select(w => new
                    {
                        Gun = w.Day.ToString(),
                        Acilis = w.OpeningTime,
                        Kapanis = w.ClosingTime
                    }).ToList()
                })
                .ToListAsync();

            return Ok(gyms); // Veriyi JSON formatında fırlatır
        }

        // GET: api/GymsApi/5
        // Tek bir salonun detayını getirir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGym(int id)
        {
            var gym = await _context.Gyms
                .Where(g => g.GymID == id)
                .Select(g => new
                {
                    g.Name,
                    g.Address,
                    Egitmenler = g.Trainers.Select(t => t.FirstName + " " + t.LastName).ToList()
                })
                .FirstOrDefaultAsync();

            if (gym == null)
            {
                return NotFound("Salon bulunamadı.");
            }

            return Ok(gym);
        }
    }
}