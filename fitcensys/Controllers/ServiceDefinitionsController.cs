using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fitcensys.Models;
using fitcensys.Models;

namespace fitcensys.Controllers
{
    public class ServiceDefinitionsController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceDefinitionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ServiceDefinitions
        public async Task<IActionResult> Index()
        {
            return View(await _context.ServiceDefinitions.ToListAsync());
        }

        // GET: ServiceDefinitions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceDefinition = await _context.ServiceDefinitions
                .FirstOrDefaultAsync(m => m.ServiceDefinitionID == id);
            if (serviceDefinition == null)
            {
                return NotFound();
            }

            return View(serviceDefinition);
        }

        // GET: ServiceDefinitions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceDefinitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceDefinitionID,Name,Description")] ServiceDefinition serviceDefinition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceDefinition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceDefinition);
        }

        // GET: ServiceDefinitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceDefinition = await _context.ServiceDefinitions.FindAsync(id);
            if (serviceDefinition == null)
            {
                return NotFound();
            }
            return View(serviceDefinition);
        }

        // POST: ServiceDefinitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceDefinitionID,Name,Description")] ServiceDefinition serviceDefinition)
        {
            if (id != serviceDefinition.ServiceDefinitionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceDefinition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceDefinitionExists(serviceDefinition.ServiceDefinitionID))
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
            return View(serviceDefinition);
        }

        // GET: ServiceDefinitions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceDefinition = await _context.ServiceDefinitions
                .FirstOrDefaultAsync(m => m.ServiceDefinitionID == id);
            if (serviceDefinition == null)
            {
                return NotFound();
            }

            return View(serviceDefinition);
        }

        // POST: ServiceDefinitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceDefinition = await _context.ServiceDefinitions.FindAsync(id);
            if (serviceDefinition != null)
            {
                _context.ServiceDefinitions.Remove(serviceDefinition);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceDefinitionExists(int id)
        {
            return _context.ServiceDefinitions.Any(e => e.ServiceDefinitionID == id);
        }
    }
}
