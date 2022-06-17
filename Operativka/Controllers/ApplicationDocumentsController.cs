using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        public async Task<IActionResult> Index(ApplicationDocumentViewModel model, string sortOrder, string? filterJsoned)
        {
            return View(await GetViewFilledViewModel(model, filterJsoned, sortOrder));

            var operativkaContext = _context.ApplicationDocuments
                .Include(a => a.ApplicationObjectives)
                .Include(a => a.Consumer)
                .ThenInclude(a => a.District);
            return View(await operativkaContext.ToListAsync());
        }

        private async Task<ApplicationDocumentViewModel> GetViewFilledViewModel(ApplicationDocumentViewModel model, string? filterJsoned, string sortOrder)
        {
            IQueryable<string> districtQuery =
               from appdoc in _context.ApplicationDocuments
               orderby appdoc.Consumer.District.Name
               select appdoc.Consumer.District.Name;

            var appdocs = from appdoc in _context.ApplicationDocuments
                          .Include(a => a.Consumer)
                          .ThenInclude(a => a.District)
                          .Include(a => a.ApplicationObjectives)
                          select appdoc;
            if (filterJsoned != null)
            {
                model.CurrentFilter = JsonSerializer.Deserialize<Dictionary<string, string>>(filterJsoned);
            }
            else
            {
                filterJsoned = model.CurrentFilter != null ? JsonSerializer.Serialize(model.CurrentFilter) : null;
            }

            ApplyFilters(ref appdocs, model);

            ApplySort(sortOrder, ref appdocs);

            int pageSize = 10;

            var currFilter = new Dictionary<string, string>
            {
                { "District", model.SelectedDistrict },
                { "PersonalAccountCode" , model.InsertedPersonalAccountCode },
            };

            var viewModel = new ApplicationDocumentViewModel
            {
                Districts = new SelectList(await districtQuery.Distinct().ToListAsync(), model.SelectedDistrict),
                SelectedDistrict = model.SelectedDistrict,
                InsertedPersonalAccountCode = model.InsertedPersonalAccountCode,

                CurrentFilter = currFilter,
                pageNumber = model.pageNumber,
                Documents = await PaginatedList<ApplicationDocument>.CreateAsync(appdocs.AsNoTracking(), model.pageNumber ?? 1, pageSize)

            };
            return viewModel;
        }

        private void ApplySort(string sortOrder, ref IQueryable<ApplicationDocument> appdocs)
        {
            ViewData["FullNameParam"] = String.IsNullOrEmpty(sortOrder) ? "full_name_desc" : "";
            ViewData["PersonalAccountCodeParam"] = sortOrder == "ls" ? "ls_desc" : "ls";
            ViewData["DistrictParam"] = sortOrder == "district" ? "district_desc" : "district";
            ViewData["AddressParam"] = sortOrder == "address" ? "address_desc" : "address";
            ViewData["PhoneNumberParam"] = sortOrder == "phone_num" ? "phone_num_desc" : "phone_num";
            
            ViewData["ExecutionDateParam"] = sortOrder == "execution_date" ? "execution_date_desc" : "execution_date";
            ViewData["PlannedDateParam"] = sortOrder == "planned_date" ? "planned_date_desc" : "planned_date";
            ViewData["OuterAppNumParam"] = sortOrder == "outer_app_num" ? "outer_app_num_desc" : "outer_app_num";
            
            appdocs = sortOrder switch
            {
                "ls_desc" => appdocs.OrderByDescending(s => s.Consumer.PersonalAccountCode),
                "ls" => appdocs.OrderBy(s => s.Consumer.PersonalAccountCode),

                "district_desc" => appdocs.OrderByDescending(s => s.Consumer.District.Name),
                "district" => appdocs.OrderBy(s => s.Consumer.District.Name),

                "address_desc" => appdocs.OrderByDescending(s => s.Consumer.Address),
                "address" => appdocs.OrderBy(s => s.Consumer.Address),

                "phone_num_desc" => appdocs.OrderByDescending(s => s.Consumer.PhoneNumber),
                "phone_num" => appdocs.OrderBy(s => s.Consumer.PhoneNumber),

                "execution_date_desc" => appdocs.OrderByDescending(s => s.ApplicationObjectives.Max(obj => obj.ExecutionDate)),
                "execution_date" => appdocs.OrderBy(s => s.ApplicationObjectives.Max(obj => obj.ExecutionDate)),

                "planned_date_desc" => appdocs.OrderByDescending(s => s.ApplicationObjectives.Min(obj => obj.PlannedDate)),
                "planned_date" => appdocs.OrderBy(s => s.ApplicationObjectives.Min(obj => obj.PlannedDate)),

                "outer_app_num_desc" => appdocs.OrderByDescending(s => s.OuterAppNum),
                "outer_app_num" => appdocs.OrderBy(s => s.OuterAppNum),

                "full_name_desc" => appdocs.OrderByDescending(s => s.Consumer.LastName + s.Consumer.FirstName + s.Consumer.Patronymic),
                _ => appdocs.OrderBy(s => s.Consumer.LastName + s.Consumer.FirstName + s.Consumer.Patronymic),
            };
        }

        private void ApplyFilters(ref IQueryable<ApplicationDocument> appdocs, ApplicationDocumentViewModel model)
        {
            if (model.CurrentFilter != null)
            {
                if (model.CurrentFilter.ContainsKey("PersonalAccountCode"))
                {
                    if (!string.IsNullOrEmpty(model.CurrentFilter["PersonalAccountCode"]))
                    {
                        appdocs = appdocs
                            .Where(d => d.Consumer.PersonalAccountCode.ToString()!.Contains(model.CurrentFilter["PersonalAccountCode"]));
                        model.InsertedPersonalAccountCode = model.CurrentFilter["PersonalAccountCode"];
                    }
                }
                if (model.CurrentFilter.ContainsKey("District"))
                {
                    if (!string.IsNullOrEmpty(model.CurrentFilter["District"]))
                    {
                        appdocs = appdocs
                            .Where(d => d.Consumer.District.Name!.Contains(model.CurrentFilter["District"]));
                        model.SelectedDistrict = model.CurrentFilter["District"];
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(model.SelectedDistrict))
                {
                    appdocs = appdocs
                        .Where(d => d.Consumer.District.Name!.Contains(model.SelectedDistrict));
                }
                if (!string.IsNullOrEmpty(model.InsertedPersonalAccountCode))
                {
                    appdocs = appdocs
                        .Where(d => d.Consumer.PersonalAccountCode.ToString()!.Contains(model.InsertedPersonalAccountCode));
                }
            }
        }

        // GET: ApplicationDocuments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ApplicationDocuments == null)
            {
                return NotFound();
            }

            var applicationDocument = await _context.ApplicationDocuments
                .Include(a => a.ApplicationObjectives)
                .Include(a => a.Consumer)
                .ThenInclude(a => a.District)
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
            applicationDocument.Consumer = await _context.Consumers.Include(x => x.District).FirstAsync(x => x.Id == applicationDocument.ConsumerId);
            ModelState.Clear();
            TryValidateModel(applicationDocument);
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

            var applicationDocument = await _context
                .ApplicationDocuments
                .Include(x => x.ApplicationObjectives)
                .Include(x => x.Consumer).ThenInclude(x => x.District)
                .FirstAsync(x => x.Id == id);
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
            applicationDocument.Consumer = await _context.Consumers.Include(x => x.District).FirstAsync(x => x.Id == applicationDocument.ConsumerId);
            ModelState.Clear();
            TryValidateModel(applicationDocument);
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
                .Include(a => a.ApplicationObjectives)
                .Include(a => a.Consumer)
                .ThenInclude(a => a.District)
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
            var applicationDocument = await _context.ApplicationDocuments
                .Include( a => a.ApplicationObjectives)
                .Include( a => a.Consumer)
                .ThenInclude( a => a.District)
                .FirstOrDefaultAsync( x => x.Id == id);
            if (applicationDocument != null)
            {
                _context.ApplicationObjectives.RemoveRange(applicationDocument.ApplicationObjectives);
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
