using fitcensys.Extensions;
using fitcensys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitcensys.Controllers
{
    public class TrainerAvailabilitiesController : Controller
    {
        private readonly AppDbContext _context;

        public TrainerAvailabilitiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TrainerAvailabilities
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.TrainerAvailabilities.Include(t => t.Trainer);
            return View(await appDbContext.ToListAsync());
        }

        // GET: TrainerAvailabilities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerAvailability = await _context.TrainerAvailabilities
                .Include(t => t.Trainer)
                .FirstOrDefaultAsync(m => m.TrainerAvailabilityID == id);
            if (trainerAvailability == null)
            {
                return NotFound();
            }

            return View(trainerAvailability);
        }

        // GET: TrainerAvailabilities/Create
        public IActionResult Create()
        {
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerID", "FullName");
            return View();
        }

        // POST: TrainerAvailabilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainerAvailabilityID,TrainerID,Day,StartTime,EndTime")] TrainerAvailability trainerAvailability)
        {
            if (trainerAvailability.EndTime <= trainerAvailability.StartTime)
            {
                ModelState.AddModelError("EndTime", "Bitiş saati, başlangıç saatinden sonra olmalıdır.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(trainerAvailability);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var gunler = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>()
                .Select(d => new { ID = (int)d, Name = d.ToTurkishName() })
                .ToList();
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerID", "FullName", trainerAvailability.TrainerID);
            return View(trainerAvailability);
        }

        // GET: TrainerAvailabilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerAvailability = await _context.TrainerAvailabilities.FindAsync(id);
            if (trainerAvailability == null)
            {
                return NotFound();
            }
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerID", "FullName", trainerAvailability.TrainerID);
            return View(trainerAvailability);
        }

        // POST: TrainerAvailabilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainerAvailabilityID,TrainerID,Day,StartTime,EndTime")] TrainerAvailability trainerAvailability)
        {
            if (trainerAvailability.EndTime <= trainerAvailability.StartTime)
            {
                ModelState.AddModelError("EndTime", "Bitiş saati, başlangıç saatinden sonra olmalıdır.");
            }
            if (id != trainerAvailability.TrainerAvailabilityID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainerAvailability);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerAvailabilityExists(trainerAvailability.TrainerAvailabilityID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerID", "FullName", trainerAvailability.TrainerID);
            return View(trainerAvailability);
        }

        // GET: TrainerAvailabilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerAvailability = await _context.TrainerAvailabilities
                .Include(t => t.Trainer)
                .FirstOrDefaultAsync(m => m.TrainerAvailabilityID == id);
            if (trainerAvailability == null)
            {
                return NotFound();
            }

            return View(trainerAvailability);
        }

        // POST: TrainerAvailabilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainerAvailability = await _context.TrainerAvailabilities.FindAsync(id);
            if (trainerAvailability != null)
            {
                _context.TrainerAvailabilities.Remove(trainerAvailability);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerAvailabilityExists(int id)
        {
            return _context.TrainerAvailabilities.Any(e => e.TrainerAvailabilityID == id);
        }
    }
}
