using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fitcensys.Models;
using fitcensys.Models;
using Microsoft.AspNetCore.Authorization;

namespace fitcensys.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrainerServicesController : Controller
    {
        private readonly AppDbContext _context;

        public TrainerServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TrainerServices
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.TrainerServices.Include(t => t.ServiceDefinition).Include(t => t.Trainer);
            return View(await appDbContext.ToListAsync());
        }

        // GET: TrainerServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerService = await _context.TrainerServices
                .Include(t => t.ServiceDefinition)
                .Include(t => t.Trainer)
                .FirstOrDefaultAsync(m => m.TrainerServiceID == id);
            if (trainerService == null)
            {
                return NotFound();
            }

            return View(trainerService);
        }

        // GET: TrainerServices/Create
        public IActionResult Create(int? trainerId)
        {
            // Eğer trainerId geldiyse dropdown'da onu seçili getir, gelmediyse boş getir.
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerID", "FullName", trainerId);

            // Hizmet listesini yükle
            ViewData["ServiceDefinitionID"] = new SelectList(_context.ServiceDefinitions, "ServiceDefinitionID", "Name");
            return View();
        }

        // POST: TrainerServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainerServiceID,TrainerID,ServiceDefinitionID")] TrainerService trainerService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainerService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceDefinitionID"] = new SelectList(_context.ServiceDefinitions, "ServiceDefinitionID", "Name", trainerService.ServiceDefinitionID);
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerID", "FullName", trainerService.TrainerID);
            return View(trainerService);
        }

        // GET: TrainerServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerService = await _context.TrainerServices.FindAsync(id);
            if (trainerService == null)
            {
                return NotFound();
            }
            ViewData["ServiceDefinitionID"] = new SelectList(_context.ServiceDefinitions, "ServiceDefinitionID", "Name", trainerService.ServiceDefinitionID);
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerID", "FullName", trainerService.TrainerID);
            return View(trainerService);
        }

        // POST: TrainerServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainerServiceID,TrainerID,ServiceDefinitionID")] TrainerService trainerService)
        {
            if (id != trainerService.TrainerServiceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainerService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerServiceExists(trainerService.TrainerServiceID))
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
            ViewData["ServiceDefinitionID"] = new SelectList(_context.ServiceDefinitions, "ServiceDefinitionID", "Name", trainerService.ServiceDefinitionID);
            ViewData["TrainerID"] = new SelectList(_context.Trainers, "TrainerID", "FullName", trainerService.TrainerID);
            return View(trainerService);
        }

        // GET: TrainerServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainerService = await _context.TrainerServices
                .Include(t => t.ServiceDefinition)
                .Include(t => t.Trainer)
                .FirstOrDefaultAsync(m => m.TrainerServiceID == id);
            if (trainerService == null)
            {
                return NotFound();
            }

            return View(trainerService);
        }

        // POST: TrainerServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainerService = await _context.TrainerServices.FindAsync(id);
            if (trainerService != null)
            {
                _context.TrainerServices.Remove(trainerService);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerServiceExists(int id)
        {
            return _context.TrainerServices.Any(e => e.TrainerServiceID == id);
        }
    }
}
