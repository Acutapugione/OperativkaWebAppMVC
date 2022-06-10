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
    public class ConsumersController : Controller
    {
        private readonly OperativkaContext _context;

        public ConsumersController(OperativkaContext context)
        {
            _context = context;
        }

        // GET: Consumers
        public async Task<IActionResult> Index()
        {
            var operativkaContext = _context.Consumers.Include(c => c.District);
            return View(await operativkaContext.ToListAsync());
        }

        // GET: Consumers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Consumers == null)
            {
                return NotFound();
            }

            var consumer = await _context.Consumers
                .Include(c => c.District)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consumer == null)
            {
                return NotFound();
            }

            return View(consumer);
        }

        // GET: Consumers/Create
        public IActionResult Create()
        {
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Id");
            return View();
        }

        // POST: Consumers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Patronymic,Address,PhoneNumber,PersonalAccountCode,DistrictId")] Consumer consumer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consumer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name", consumer.DistrictId);
            return View(consumer);
        }

        // GET: Consumers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Consumers == null)
            {
                return NotFound();
            }

            var consumer = await _context.Consumers.FindAsync(id);
            if (consumer == null)
            {
                return NotFound();
            }
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name", consumer.DistrictId);
            return View(consumer);
        }

        // POST: Consumers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Patronymic,Address,PhoneNumber,PersonalAccountCode,DistrictId")] Consumer consumer)
        {
            if (id != consumer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consumer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsumerExists(consumer.Id))
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
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name", consumer.DistrictId);
            return View(consumer);
        }

        // GET: Consumers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Consumers == null)
            {
                return NotFound();
            }

            var consumer = await _context.Consumers
                .Include(c => c.District)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consumer == null)
            {
                return NotFound();
            }

            return View(consumer);
        }

        // POST: Consumers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Consumers == null)
            {
                return Problem("Entity set 'OperativkaContext.Consumer'  is null.");
            }
            var consumer = await _context.Consumers.FindAsync(id);
            if (consumer != null)
            {
                _context.Consumers.Remove(consumer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsumerExists(int id)
        {
          return (_context.Consumers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
