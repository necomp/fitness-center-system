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
    public class GymServicesController : Controller
    {
        private readonly AppDbContext _context;

        public GymServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: GymServices
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.GymServices.Include(g => g.Gym).Include(g => g.ServiceDefinition);
            return View(await appDbContext.ToListAsync());
        }

        // GET: GymServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymService = await _context.GymServices
                .Include(g => g.Gym)
                .Include(g => g.ServiceDefinition)
                .FirstOrDefaultAsync(m => m.GymServiceID == id);
            if (gymService == null)
            {
                return NotFound();
            }

            return View(gymService);
        }

        // GET: GymServices/Create
        public IActionResult Create(int? gymId)
        {
            ViewData["GymID"] = new SelectList(_context.Gyms, "GymID", "Name", gymId);
            // Hizmet tanımlarını da çekelim (Pilates, Boks vb.)
            ViewData["ServiceDefinitionID"] = new SelectList(_context.ServiceDefinitions, "ServiceDefinitionID", "Name");
            return View();
        }

        // POST: GymServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GymServiceID,GymID,ServiceDefinitionID,Price,Duration,Capacity")] GymService gymService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gymService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GymID"] = new SelectList(_context.Gyms, "GymID", "Name", gymService.GymID);
            ViewData["ServiceDefinitionID"] = new SelectList(_context.ServiceDefinitions, "ServiceDefinitionID", "Name", gymService.ServiceDefinitionID);
            return View(gymService);
        }

        // GET: GymServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymService = await _context.GymServices.FindAsync(id);
            if (gymService == null)
            {
                return NotFound();
            }
            ViewData["GymID"] = new SelectList(_context.Gyms, "GymID", "Name", gymService.GymID);
            ViewData["ServiceDefinitionID"] = new SelectList(_context.ServiceDefinitions, "ServiceDefinitionID", "Name", gymService.ServiceDefinitionID);
            return View(gymService);
        }

        // POST: GymServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GymServiceID,GymID,ServiceDefinitionID,Price,Duration,Capacity")] GymService gymService)
        {
            if (id != gymService.GymServiceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymServiceExists(gymService.GymServiceID))
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
            ViewData["GymID"] = new SelectList(_context.Gyms, "GymID", "Name", gymService.GymID);
            ViewData["ServiceDefinitionID"] = new SelectList(_context.ServiceDefinitions, "ServiceDefinitionID", "Name", gymService.ServiceDefinitionID);
            return View(gymService);
        }

        // GET: GymServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymService = await _context.GymServices
                .Include(g => g.Gym)
                .Include(g => g.ServiceDefinition)
                .FirstOrDefaultAsync(m => m.GymServiceID == id);
            if (gymService == null)
            {
                return NotFound();
            }

            return View(gymService);
        }

        // POST: GymServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymService = await _context.GymServices.FindAsync(id);
            if (gymService != null)
            {
                _context.GymServices.Remove(gymService);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymServiceExists(int id)
        {
            return _context.GymServices.Any(e => e.GymServiceID == id);
        }
    }
}
