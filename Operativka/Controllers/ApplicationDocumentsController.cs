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
    public class ApplicationDocumentsController : Controller
    {
        private readonly OperativkaContext _context;

        public ApplicationDocumentsController(OperativkaContext context)
        {
            _context = context;
        }

        // GET: ApplicationDocuments
        public async Task<IActionResult> Index()
        {
            var operativkaContext = _context.ApplicationDocuments.Include(a => a.Consumer);
            return View(await operativkaContext.ToListAsync());
        }

        // GET: ApplicationDocuments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ApplicationDocuments == null)
            {
                return NotFound();
            }

            var applicationDocument = await _context.ApplicationDocuments
                .Include(a => a.Consumer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationDocument == null)
            {
                return NotFound();
            }

            return View(applicationDocument);
        }

        // GET: ApplicationDocuments/Create
        [Authorize(Roles = "Moderator")]
        public IActionResult Create()
        {
            ViewData["ConsumerId"] = new SelectList(_context.Consumers, "Id", "FullName");
            return View();
        }

        // POST: ApplicationDocuments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Create([Bind("Id,ConsumerId,OuterAppNum,IsFromOuterApp")] ApplicationDocument applicationDocument)
        {
            if (ModelState.IsValid)
            {
                _context.Add(applicationDocument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConsumerId"] = new SelectList(_context.Consumers, "Id", "FullName", applicationDocument.ConsumerId);
            return View(applicationDocument);
        }

        // GET: ApplicationDocuments/Edit/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ApplicationDocuments == null)
            {
                return NotFound();
            }

            var applicationDocument = await _context.ApplicationDocuments.FindAsync(id);
            if (applicationDocument == null)
            {
                return NotFound();
            }
            ViewData["ConsumerId"] = new SelectList(_context.Consumers, "Id", "FullName", applicationDocument.ConsumerId);
            return View(applicationDocument);
        }

        // POST: ApplicationDocuments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ConsumerId,OuterAppNum,IsFromOuterApp")] ApplicationDocument applicationDocument)
        {
            if (id != applicationDocument.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationDocument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationDocumentExists(applicationDocument.Id))
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
            ViewData["ConsumerId"] = new SelectList(_context.Consumers, "Id", "FullName", applicationDocument.ConsumerId);
            return View(applicationDocument);
        }

        // GET: ApplicationDocuments/Delete/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ApplicationDocuments == null)
            {
                return NotFound();
            }

            var applicationDocument = await _context.ApplicationDocuments
                .Include(a => a.Consumer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationDocument == null)
            {
                return NotFound();
            }

            return View(applicationDocument);
        }

        // POST: ApplicationDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ApplicationDocuments == null)
            {
                return Problem("Entity set 'OperativkaContext.ApplicationDocument'  is null.");
            }
            var applicationDocument = await _context.ApplicationDocuments.FindAsync(id);
            if (applicationDocument != null)
            {
                _context.ApplicationDocuments.Remove(applicationDocument);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationDocumentExists(int id)
        {
          return (_context.ApplicationDocuments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
