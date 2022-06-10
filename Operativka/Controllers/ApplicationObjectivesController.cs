using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Operativka.Data;
using Operativka.Models;

namespace Operativka.Controllers
{
    public class ApplicationObjectivesController : Controller
    {
        private readonly OperativkaContext _context;

        public ApplicationObjectivesController(OperativkaContext context)
        {
            _context = context;
        }

        // GET: ApplicationObjectives
        public async Task<IActionResult> Index()
        {
            var operativkaContext = _context.ApplicationObjectives.Include(a => a.ApplicationDocument);
            return View(await operativkaContext.ToListAsync());
        }

        // GET: ApplicationObjectives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ApplicationObjectives == null)
            {
                return NotFound();
            }

            var applicationObjective = await _context.ApplicationObjectives
                .Include(a => a.ApplicationDocument)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationObjective == null)
            {
                return NotFound();
            }

            return View(applicationObjective);
        }

        // GET: ApplicationObjectives/Create
        public IActionResult Create()
        {
            ViewData["ApplicationDocumentId"] = new SelectList(_context.ApplicationDocuments, "Id", "Id");
            return View();
        }

        // POST: ApplicationObjectives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,PlannedDate,ExecutionDate,IsExecuted,ApplicationDocumentId")] ApplicationObjective applicationObjective)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationObjective);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationDocumentId"] = new SelectList(_context.ApplicationDocuments, "Id", "Id", applicationObjective.ApplicationDocumentId);
            return View(applicationObjective);
        }

        // GET: ApplicationObjectives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ApplicationObjectives == null)
            {
                return NotFound();
            }

            var applicationObjective = await _context.ApplicationObjectives.FindAsync(id);
            if (applicationObjective == null)
            {
                return NotFound();
            }
            ViewData["ApplicationDocumentId"] = new SelectList(_context.ApplicationDocuments, "Id", "Id", applicationObjective.ApplicationDocumentId);
            return View(applicationObjective);
        }

        // POST: ApplicationObjectives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,PlannedDate,ExecutionDate,IsExecuted,ApplicationDocumentId")] ApplicationObjective applicationObjective)
        {
            if (id != applicationObjective.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationObjective);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationObjectiveExists(applicationObjective.Id))
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
            ViewData["ApplicationDocumentId"] = new SelectList(_context.ApplicationDocuments, "Id", "Id", applicationObjective.ApplicationDocumentId);
            return View(applicationObjective);
        }

        // GET: ApplicationObjectives/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ApplicationObjectives == null)
            {
                return NotFound();
            }

            var applicationObjective = await _context.ApplicationObjectives
                .Include(a => a.ApplicationDocument)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationObjective == null)
            {
                return NotFound();
            }

            return View(applicationObjective);
        }

        // POST: ApplicationObjectives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ApplicationObjectives == null)
            {
                return Problem("Entity set 'OperativkaContext.ApplicationObjective'  is null.");
            }
            var applicationObjective = await _context.ApplicationObjectives.FindAsync(id);
            if (applicationObjective != null)
            {
                _context.ApplicationObjectives.Remove(applicationObjective);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationObjectiveExists(int id)
        {
          return (_context.ApplicationObjectives?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
