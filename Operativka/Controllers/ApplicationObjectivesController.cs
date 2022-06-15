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
using static Operativka.Areas.Identity.Models.Enums;

namespace Operativka.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "Moderator")]
        // GET: ApplicationObjectives/Create
        public IActionResult Create(int documentID)
        {
            ApplicationObjective objective = new()
            {
                ApplicationDocumentId = documentID
            };
            ViewBag.Types = Enum.GetValues(typeof(ApplicationObjectiveTypes)).Cast<ApplicationObjectiveTypes>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            return View(objective);
        }

        // POST: ApplicationObjectives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Create([Bind("Id,Type,PlannedDate,ExecutionDate,IsExecuted,ApplicationDocumentId")] ApplicationObjective applicationObjective)
        {
            if (applicationObjective is null)
            {
                return View(nameof(Create));
            }
            applicationObjective.ApplicationDocument = await _context
                .ApplicationDocuments
                .FirstOrDefaultAsync(x => x.Id == applicationObjective.ApplicationDocumentId);
            ModelState.Clear();
            TryValidateModel(applicationObjective);
            if (ModelState.IsValid)
            {
                _context.Add(applicationObjective);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), "ApplicationDocuments", new { id = applicationObjective.ApplicationDocumentId });
            }
            ViewBag.Types = Enum.GetValues(typeof(ApplicationObjectiveTypes)).Cast<ApplicationObjectiveTypes>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            return View(applicationObjective);
        }

        [Authorize(Roles = "Moderator")]
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
            ViewBag.Types = Enum.GetValues(typeof(ApplicationObjectiveTypes)).Cast<ApplicationObjectiveTypes>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();

            return View(applicationObjective);
        }

        // POST: ApplicationObjectives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,PlannedDate,ExecutionDate,IsExecuted,ApplicationDocumentId")] ApplicationObjective applicationObjective)
        {
            if (id != applicationObjective.Id)
            {
                return NotFound();
            }
            applicationObjective.ApplicationDocument = await _context
               .ApplicationDocuments
               .FirstOrDefaultAsync(x => x.Id == applicationObjective.ApplicationDocumentId);
            ModelState.Clear();
            TryValidateModel(applicationObjective);
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
                return RedirectToAction(nameof(Edit), "ApplicationDocuments", new { id = applicationObjective.ApplicationDocumentId });
            }
            ViewBag.Types =  Enum.GetValues(typeof(ApplicationObjectiveTypes)).Cast<ApplicationObjectiveTypes>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList();
          
            return View(applicationObjective);
        }
        [Authorize(Roles = "Moderator")]
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
        [Authorize(Roles = "Moderator")]
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
            return RedirectToAction(nameof(Edit), "ApplicationDocuments", new { id = applicationObjective.ApplicationDocumentId });
        }

        private bool ApplicationObjectiveExists(int id)
        {
          return (_context.ApplicationObjectives?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
