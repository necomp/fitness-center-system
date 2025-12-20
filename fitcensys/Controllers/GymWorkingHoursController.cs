using fitcensys.Migrations;
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
    public class GymWorkingHoursController : Controller
    {
        private readonly AppDbContext _context;

        public GymWorkingHoursController(AppDbContext context)
        {
            _context = context;
        }

        // GET: GymWorkingHours
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.GymWorkingHours.Include(g => g.Gym);
            return View(await appDbContext.ToListAsync());
        }

        // GET: GymWorkingHours/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymWorkingHour = await _context.GymWorkingHours
                .Include(g => g.Gym)
                .FirstOrDefaultAsync(m => m.GymWorkingHourID == id);
            if (gymWorkingHour == null)
            {
                return NotFound();
            }

            return View(gymWorkingHour);
        }

        // GET: GymWorkingHours/Create
        public IActionResult Create()
        {
            ViewData["GymID"] = new SelectList(_context.Gyms, "GymID", "Name");
            return View();
        }

        // POST: GymWorkingHours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GymWorkingHourID,GymID,Day,OpeningTime,ClosingTime")] GymWorkingHour gymWorkingHour)
        {
            if (gymWorkingHour.ClosingTime <= gymWorkingHour.OpeningTime)
            {
                ModelState.AddModelError("ClosingTime", "Kapanış saati, açılış saatinden sonra olmalıdır.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(gymWorkingHour);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GymID"] = new SelectList(_context.Gyms, "GymID", "Name", gymWorkingHour.GymID);
            return View(gymWorkingHour);
        }

        // GET: GymWorkingHours/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymWorkingHour = await _context.GymWorkingHours.FindAsync(id);
            if (gymWorkingHour == null)
            {
                return NotFound();
            }
            ViewData["GymID"] = new SelectList(_context.Gyms, "GymID", "Name", gymWorkingHour.GymID);
            return View(gymWorkingHour);
        }

        // POST: GymWorkingHours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GymWorkingHourID,GymID,Day,OpeningTime,ClosingTime")] GymWorkingHour gymWorkingHour)
        {
            if (gymWorkingHour.ClosingTime <= gymWorkingHour.OpeningTime)
            {
                ModelState.AddModelError("ClosingTime", "Kapanış saati, açılış saatinden sonra olmalıdır.");
            }
            if (id != gymWorkingHour.GymWorkingHourID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymWorkingHour);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymWorkingHourExists(gymWorkingHour.GymWorkingHourID))
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
            ViewData["GymID"] = new SelectList(_context.Gyms, "GymID", "Name", gymWorkingHour.GymID);
            return View(gymWorkingHour);
        }

        // GET: GymWorkingHours/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymWorkingHour = await _context.GymWorkingHours
                .Include(g => g.Gym)
                .FirstOrDefaultAsync(m => m.GymWorkingHourID == id);
            if (gymWorkingHour == null)
            {
                return NotFound();
            }

            return View(gymWorkingHour);
        }

        // POST: GymWorkingHours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymWorkingHour = await _context.GymWorkingHours.FindAsync(id);
            if (gymWorkingHour != null)
            {
                _context.GymWorkingHours.Remove(gymWorkingHour);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymWorkingHourExists(int id)
        {
            return _context.GymWorkingHours.Any(e => e.GymWorkingHourID == id);
        }
    }
}
