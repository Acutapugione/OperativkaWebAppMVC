using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestApp.Models;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class HistoriesController : Controller
    {
        private readonly MyContext _context;

        public HistoriesController(MyContext context)
        {
            _context = context;
        }

        // GET: Histories
        public async Task<IActionResult> Index()
        {
            var myContext = _context.History.Include(h => h.User);
            return View(await myContext.ToListAsync());
        }

        // GET: Histories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.History == null)
            {
                return NotFound();
            }

            var history = await _context.History
                .Include(h => h.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (history == null)
            {
                return NotFound();
            }

            return View(history);
        }

        // GET: Histories/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Set<User>(), "ID", "ID");
            return View();
        }

        // POST: Histories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Year,Month,Day,UserID")] History history)
        {
            if (ModelState.IsValid)
            {
                _context.Add(history);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Set<User>(), "ID", "ID", history.UserID);
            return View(history);
        }

        // GET: Histories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.History == null)
            {
                return NotFound();
            }

            var history = await _context.History.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Set<User>(), "ID", "ID", history.UserID);
            return View(history);
        }

        // POST: Histories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Year,Month,Day,UserID")] History history)
        {
            if (id != history.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(history);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoryExists(history.ID))
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
            ViewData["UserID"] = new SelectList(_context.Set<User>(), "ID", "ID", history.UserID);
            return View(history);
        }

        // GET: Histories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.History == null)
            {
                return NotFound();
            }

            var history = await _context.History
                .Include(h => h.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (history == null)
            {
                return NotFound();
            }

            return View(history);
        }

        // POST: Histories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.History == null)
            {
                return Problem("Entity set 'MyContext.History'  is null.");
            }
            var history = await _context.History.FindAsync(id);
            if (history != null)
            {
                _context.History.Remove(history);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoryExists(int id)
        {
          return (_context.History?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
