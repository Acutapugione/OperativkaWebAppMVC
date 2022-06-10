using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Operativka.Data;
using Operativka.Models;

namespace Operativka.Controllers
{
    [Authorize]
    public class PlanningIndicatorsController : Controller
    {
        private readonly OperativkaContext _context;

        public PlanningIndicatorsController(OperativkaContext context)
        {
            _context = context;
        }

        // GET: PlanningIndicators
        public async Task<IActionResult> Index()
        {
              return _context.PlanningIndicators != null ? 
                          View(await _context.PlanningIndicators.ToListAsync()) :
                          Problem("Entity set 'OperativkaContext.PlanningIndicators'  is null.");
        }

        // GET: PlanningIndicators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PlanningIndicators == null)
            {
                return NotFound();
            }

            var planningIndicator = await _context.PlanningIndicators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planningIndicator == null)
            {
                return NotFound();
            }

            return View(planningIndicator);
        }

        // GET: PlanningIndicators/Create
        [Authorize(Roles = "Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlanningIndicators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Name,Name_Fact")] PlanningIndicator planningIndicator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(planningIndicator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(planningIndicator);
        }

        // GET: PlanningIndicators/Edit/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PlanningIndicators == null)
            {
                return NotFound();
            }

            var planningIndicator = await _context.PlanningIndicators.FindAsync(id);
            if (planningIndicator == null)
            {
                return NotFound();
            }
            return View(planningIndicator);
        }

        // POST: PlanningIndicators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Name_Fact")] PlanningIndicator planningIndicator)
        {
            if (id != planningIndicator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(planningIndicator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanningIndicatorExists(planningIndicator.Id))
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
            return View(planningIndicator);
        }

        // GET: PlanningIndicators/Delete/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PlanningIndicators == null)
            {
                return NotFound();
            }

            var planningIndicator = await _context.PlanningIndicators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (planningIndicator == null)
            {
                return NotFound();
            }

            return View(planningIndicator);
        }

        // POST: PlanningIndicators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PlanningIndicators == null)
            {
                return Problem("Entity set 'OperativkaContext.PlanningIndicators'  is null.");
            }
            var planningIndicator = await _context.PlanningIndicators.FindAsync(id);
            if (planningIndicator != null)
            {
                _context.PlanningIndicators.Remove(planningIndicator);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanningIndicatorExists(int id)
        {
          return (_context.PlanningIndicators?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
