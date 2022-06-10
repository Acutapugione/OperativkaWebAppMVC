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
    public class SettlementsController : Controller
    {
        private readonly OperativkaContext _context;

        public SettlementsController(OperativkaContext context)
        {
            _context = context;
        }

        // GET: Settlements
        public async Task<IActionResult> Index()
        {
            var operativkaContext = _context.Settlements.Include(s => s.District);
            return View(await operativkaContext.ToListAsync());
        }

        // GET: Settlements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Settlements == null)
            {
                return NotFound();
            }

            var settlement = await _context.Settlements
                .Include(s => s.District)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (settlement == null)
            {
                return NotFound();
            }

            return View(settlement);
        }

        // GET: Settlements/Create
        [Authorize(Roles = "Moderator")]
        public IActionResult Create()
        {
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name");
            return View();
        }

        // POST: Settlements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Name,DistrictId")] Settlement settlement)
        {
            settlement.District = await _context.Districts.FirstOrDefaultAsync(x => x.Id == settlement.DistrictId);
            if (settlement.District!=null)
            {
                _context.Add(settlement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name", settlement.DistrictId);
            return View(settlement);
        }

        // GET: Settlements/Edit/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Settlements == null)
            {
                return NotFound();
            }

            var settlement = await _context.Settlements.FindAsync(id);
            if (settlement == null)
            {
                return NotFound();
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name", settlement.DistrictId);
            return View(settlement);
        }

        // POST: Settlements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DistrictId")] Settlement settlement)
        {
            if (id != settlement.Id)
            {
                return NotFound();
            }

            settlement.District = await _context.Districts.FirstOrDefaultAsync(x => x.Id == settlement.DistrictId);
            if (settlement.District != null)
            {
                try
                {
                    _context.Update(settlement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettlementExists(settlement.Id))
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
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name", settlement.DistrictId);
            return View(settlement);
        }

        // GET: Settlements/Delete/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Settlements == null)
            {
                return NotFound();
            }

            var settlement = await _context.Settlements
                .Include(s => s.District)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (settlement == null)
            {
                return NotFound();
            }

            return View(settlement);
        }

        // POST: Settlements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Settlements == null)
            {
                return Problem("Entity set 'OperativkaContext.Settlements'  is null.");
            }
            var settlement = await _context.Settlements.FindAsync(id);
            if (settlement != null)
            {
                _context.Settlements.Remove(settlement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SettlementExists(int id)
        {
          return (_context.Settlements?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
