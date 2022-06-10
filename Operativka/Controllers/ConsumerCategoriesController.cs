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
    public class ConsumerCategoriesController : Controller
    {
        private readonly OperativkaContext _context;

        public ConsumerCategoriesController(OperativkaContext context)
        {
            _context = context;
        }

        // GET: ConsumerCategories
        public async Task<IActionResult> Index()
        {
              return _context.ConsumerCategories != null ? 
                          View(await _context.ConsumerCategories.ToListAsync()) :
                          Problem("Entity set 'OperativkaContext.ConsumerCategories'  is null.");
        }

        // GET: ConsumerCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ConsumerCategories == null)
            {
                return NotFound();
            }

            var consumerCategorie = await _context.ConsumerCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consumerCategorie == null)
            {
                return NotFound();
            }

            return View(consumerCategorie);
        }

        [Authorize(Roles = "Moderator")]
        // GET: ConsumerCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConsumerCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] ConsumerCategorie consumerCategorie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consumerCategorie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(consumerCategorie);
        }

        // GET: ConsumerCategories/Edit/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ConsumerCategories == null)
            {
                return NotFound();
            }

            var consumerCategorie = await _context.ConsumerCategories.FindAsync(id);
            if (consumerCategorie == null)
            {
                return NotFound();
            }
            return View(consumerCategorie);
        }

        // POST: ConsumerCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] ConsumerCategorie consumerCategorie)
        {
            if (id != consumerCategorie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consumerCategorie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsumerCategorieExists(consumerCategorie.Id))
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
            return View(consumerCategorie);
        }

        // GET: ConsumerCategories/Delete/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ConsumerCategories == null)
            {
                return NotFound();
            }

            var consumerCategorie = await _context.ConsumerCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consumerCategorie == null)
            {
                return NotFound();
            }

            return View(consumerCategorie);
        }

        // POST: ConsumerCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ConsumerCategories == null)
            {
                return Problem("Entity set 'OperativkaContext.ConsumerCategories'  is null.");
            }
            var consumerCategorie = await _context.ConsumerCategories.FindAsync(id);
            if (consumerCategorie != null)
            {
                _context.ConsumerCategories.Remove(consumerCategorie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsumerCategorieExists(int id)
        {
          return (_context.ConsumerCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
