using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fitcensys.Models;

namespace fitcensys.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext _context;

        public AppointmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            // Giriş yapan kullanıcıyı alalım (Opsiyonel: Sadece kendi randevularını görsün istersek burayı filtreleriz)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Eğer admin ise hepsini görsün, değilse sadece kendininkini (Bu filtreyi şimdilik kapalı tutuyorum, test için hepsini gör)
            var appDbContext = _context.Appointments
                .Include(a => a.GymService).ThenInclude(gs => gs.ServiceDefinition) // Hizmet adını görmek için zincirleme include
                .Include(a => a.Member)
                .Include(a => a.Trainer);

            return View(await appDbContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.GymService).ThenInclude(gs => gs.ServiceDefinition)
                .Include(a => a.Member)
                .Include(a => a.Trainer)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            // DÜZELTME: Dropdown'da ID yerine Hizmet Adı görünsün.
            // GymServices tablosunu ServiceDefinition ile birleştirip çekiyoruz.
            var services = _context.GymServices.Include(s => s.ServiceDefinition);

            // Value: GymServiceID, Text: ServiceDefinition.Name
            ViewData["GymServiceID"] = new SelectList(services, "GymServiceID", "ServiceDefinition.Name");

            // Trainer için Adını gösterelim
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerID", "FullName");

            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentID,TrainerID,GymServiceID,AppointmentDate,StartTime")] Appointment appointment)
        {
            // 1. Giriş Yapan Kullanıcıyı Ata
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            appointment.MemberID = userId;

            // 2. Seçilen Hizmeti Getir
            var service = await _context.GymServices
                .Include(s => s.Gym)
                .Include(s => s.ServiceDefinition)
                .FirstOrDefaultAsync(s => s.GymServiceID == appointment.GymServiceID);

            if (service == null)
            {
                ModelState.AddModelError("", "Seçilen hizmet bulunamadı.");
                // Listeleri tekrar doldur
                ReloadSelectLists(appointment); // Aşağıda bu metodu tanımladım, kod tekrarını önler
                return View(appointment);
            }

            // 3. Otomatik Atamalar
            appointment.PriceSnapshot = service.Price;
            appointment.Status = AppointmentStatus.Pending;
            appointment.CreatedDate = DateTime.Now;

            // DateTime ile TimeSpan'i toplayıp atıyoruz
            // ÖNEMLİ: StartTime formdan sadece "Saat" olarak geliyor, Tarih ile birleştiriyoruz.
            DateTime startDateTime = appointment.AppointmentDate.Date + appointment.StartTime;
            appointment.AppointmentDate = startDateTime; // Tarih ve Saati birleştirdik (İstersen ayrı da tutabilirsin)

            // EndTime Hesaplama
            appointment.EndTime = appointment.StartTime.Add(service.Duration);

            // 4. VALIDATION (Mantık Kontrolleri)

            // A) Salon Açık mı?
            var dayOfWeek = appointment.AppointmentDate.DayOfWeek;
            var workingHours = await _context.GymWorkingHours
                .FirstOrDefaultAsync(w => w.GymID == service.GymID && w.Day == dayOfWeek);

            if (workingHours != null)
            {
                if (appointment.StartTime < workingHours.OpeningTime || appointment.EndTime > workingHours.ClosingTime)
                {
                    ModelState.AddModelError("", $"Salon seçilen günde {workingHours.OpeningTime} - {workingHours.ClosingTime} saatleri arasında hizmet vermektedir.");
                }
            }

            // C) Eğitmen O Gün/Saatte Çalışıyor mu? (Vardiya Kontrolü)
            var trainerShift = await _context.TrainerAvailabilities
                .FirstOrDefaultAsync(t => t.TrainerID == appointment.TrainerID && t.Day == dayOfWeek);

            if (trainerShift != null)
            {
                // Hoca o gün çalışıyor ama seçilen saat vardiyası dışında mı?
                if (appointment.StartTime < trainerShift.StartTime || appointment.EndTime > trainerShift.EndTime)
                {
                    ModelState.AddModelError("", $"Seçilen eğitmen {dayOfWeek} günleri sadece {trainerShift.StartTime} - {trainerShift.EndTime} saatleri arasında çalışmaktadır.");
                }
            }
            else
            {
                // Hoca o gün hiç çalışmıyorsa (Vardiya kaydı yoksa) randevu almayı engellemek istersen:
                ModelState.AddModelError("", $"Eğitmen {dayOfWeek} günleri çalışmamaktadır.");
            }

            // B) Eğitmen Müsait mi?
            bool isConflict = await _context.Appointments.AnyAsync(a =>
                a.TrainerID == appointment.TrainerID &&
                a.AppointmentDate.Date == appointment.AppointmentDate.Date && // Aynı gün
                a.Status != AppointmentStatus.Cancelled &&
                (
                    (appointment.StartTime >= a.StartTime && appointment.StartTime < a.EndTime) ||
                    (appointment.EndTime > a.StartTime && appointment.EndTime <= a.EndTime) ||
                    (appointment.StartTime <= a.StartTime && appointment.EndTime >= a.EndTime)
                )
            );

            if (isConflict)
            {
                ModelState.AddModelError("", "Seçilen saatte eğitmenin başka bir randevusu mevcut.");
            }

            // --- KRİTİK DÜZELTME BURASI ---
            // Backend tarafında hesapladığımız veya boş gelebilecek navigation property hatalarını siliyoruz.
            ModelState.Remove("MemberID");
            ModelState.Remove("Member");
            ModelState.Remove("Trainer");
            ModelState.Remove("GymService");
            ModelState.Remove("EndTime");
            ModelState.Remove("Status");
            ModelState.Remove("PriceSnapshot");

            // --- Kayıt İşlemi ---
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Hata varsa listeleri tekrar doldur
            ReloadSelectLists(appointment);
            return View(appointment);
        }

        // Kod tekrarını önlemek için yardımcı metot (Class'ın içine, en alta ekleyebilirsin)
        private void ReloadSelectLists(Appointment appointment)
        {
            var serviceList = _context.GymServices
                .Include(s => s.ServiceDefinition)
                .Select(s => new { Id = s.GymServiceID, DisplayText = s.ServiceDefinition.Name + " (" + s.Price + " TL)" })
                .ToList();

            var trainerList = _context.Trainers
                .Select(t => new { Id = t.TrainerID, Name = t.FirstName + " " + t.LastName })
                .ToList();

            ViewData["GymServiceID"] = new SelectList(serviceList, "Id", "DisplayText", appointment.GymServiceID);
            ViewData["TrainerID"] = new SelectList(trainerList, "Id", "Name", appointment.TrainerID);
        }

        // EDIT ve DELETE metotları şimdilik standart kalabilir, Create en önemlisiydi.
        // Vaktimiz kalırsa onları da Create mantığına benzetiriz.
        // ... (Geri kalanı Scaffolding'den gelen kodlar kalabilir) ...

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            ViewData["GymServiceID"] = new SelectList(_context.GymServices, "GymServiceID", "GymServiceID", appointment.GymServiceID);
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerID", "FirstName", appointment.TrainerID);
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

       
        

    }
}