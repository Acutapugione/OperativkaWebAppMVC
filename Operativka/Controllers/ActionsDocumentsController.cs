using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Operativka.Data;
using Operativka.Models;

namespace Operativka.Controllers
{
    [Authorize]
    public class ActionsDocumentsController : Controller
    {
        private const string RequiredFieldNames = "Id,Date,SettlementId,ConsumerCategorieId,PlanningIndicatorId,Plan_Count,Fact_Count";
        private readonly OperativkaContext _context;

        public ActionsDocumentsController(OperativkaContext context)
        {
            _context = context;
        }

        //GET: ActionsDocuments
        public async Task<IActionResult> Index(ActionsDocumentViewModel model, string sortOrder, string? filterJsoned)
        {
            IQueryable<string> districtQuery =
                from doc in _context.ActionsDocuments
                orderby doc.Settlement.District.Name
                select doc.Settlement.District.Name;

            IQueryable<string> categorieQuery = 
                from doc in _context.ActionsDocuments
                orderby doc.ConsumerCategorie.Name
                select doc.ConsumerCategorie.Name;

            IQueryable<string> planIndicatorQuery =
                from doc in _context.ActionsDocuments
                orderby doc.PlanningIndicator.Name
                select doc.PlanningIndicator.Name;

            IQueryable<string> settlementQuery =
                from doc in _context.ActionsDocuments
                orderby doc.Settlement.Name
                select doc.Settlement.Name;

            var documents = from d in _context.ActionsDocuments
                           .Include(a => a.ConsumerCategorie)
                           .Include(a => a.PlanningIndicator)
                           .Include(a => a.Settlement)
                            select d;

            
            if (filterJsoned != null)
            {
                model.CurrentFilter = JsonSerializer.Deserialize<Dictionary<string, string>>(filterJsoned);
            }
            else
            {
                filterJsoned = model.CurrentFilter!=null ? JsonSerializer.Serialize(model.CurrentFilter) : null;
            }
            ApplyFilters(ref documents, ref settlementQuery, model);
            ApplySort(sortOrder, ref documents);
            int pageSize = 10;

            var currFilter = new Dictionary<string, string>
            {
                { "ConsumerCategorie", model.SelectedConsumerCategorie },
                { "PlanningIndicator", model.SelectedPlanningIndicator },
                { "District", model.SelectedDistrict },
                { "Settlement", model.SelectedSettlement }
            };

            var DocumentsViewModel = new ActionsDocumentViewModel
            {
                Districts = new SelectList(await districtQuery.Distinct().ToListAsync(), model.SelectedDistrict),
                Settlements = new SelectList(await settlementQuery.Distinct().ToListAsync(), model.SelectedSettlement),
                PlanningIndicators = new SelectList(await planIndicatorQuery.Distinct().ToListAsync(), model.SelectedPlanningIndicator),
                ConsumerCategories = new SelectList(await categorieQuery.Distinct().ToListAsync(), model.SelectedConsumerCategorie),
                
                SelectedConsumerCategorie = model.SelectedConsumerCategorie,
                SelectedPlanningIndicator = model.SelectedPlanningIndicator,
                SelectedSettlement = model.SelectedSettlement,
                SelectedDistrict = model.SelectedDistrict,

                CurrentFilter = currFilter,
                pageNumber = model.pageNumber,
                Documents = await PaginatedList<ActionsDocument>.CreateAsync(documents.AsNoTracking(), model.pageNumber ?? 1, pageSize)

            };

            //ViewData["ConsumerCategorie"] = new SelectList(await categorieQuery.Distinct().ToListAsync(), "", "", DocumentsViewModel.SelectedConsumerCategorie);
            //ViewData["PlanningIndicator"] = new SelectList(await planIndicatorQuery.Distinct().ToListAsync(), "", "", DocumentsViewModel.SelectedPlanningIndicator);
            //ViewData["District"] = new SelectList(await districtQuery.Distinct().ToListAsync(), "", "", DocumentsViewModel.SelectedDistrict);
            //ViewData["Settlement"] = new SelectList(await settlementQuery.Distinct().ToListAsync(), "", "", DocumentsViewModel.SelectedSettlement);
            //ViewData["CurrentFilter"] = currFilter;

            return View(DocumentsViewModel);
        }

        public void ApplyFilters(ref IQueryable<ActionsDocument> documents, ref IQueryable<string> settlementQuery, ActionsDocumentViewModel model)
        {
            if (model.CurrentFilter!=null)
            {
                if (model.CurrentFilter.ContainsKey("District"))
                {
                    if (!string.IsNullOrEmpty(model.CurrentFilter["District"]))
                    {
                        settlementQuery = from doc in _context.ActionsDocuments
                                          orderby doc.Settlement.Name
                                          where doc.Settlement.District.Name == model.CurrentFilter["District"]
                                          select doc.Settlement.Name;
                        documents = documents
                            .Where(d => d.Settlement.District.Name!.Contains(model.CurrentFilter["District"]));
                        model.SelectedDistrict = model.CurrentFilter["District"];
                    }
                }

                if (model.CurrentFilter.ContainsKey("Settlement"))
                {
                    if (!string.IsNullOrEmpty(model.CurrentFilter["Settlement"]))
                    {
                        documents = documents
                            .Where(s => s.Settlement.Name!.Contains(model.CurrentFilter["Settlement"]));
                        model.SelectedSettlement = model.CurrentFilter["Settlement"];
                    }
                }
                if (model.CurrentFilter.ContainsKey("PlanningIndicator"))
                {
                    if (!string.IsNullOrEmpty(model.CurrentFilter["PlanningIndicator"]))
                    {
                        documents = documents
                            .Where(s => s.PlanningIndicator.Name!.Contains(model.CurrentFilter["PlanningIndicator"]));
                        model.SelectedPlanningIndicator = model.CurrentFilter["PlanningIndicator"];
                    }
                }
                if (model.CurrentFilter.ContainsKey("ConsumerCategorie"))
                {
                    if (!string.IsNullOrEmpty(model.CurrentFilter["ConsumerCategorie"]))
                    {
                        documents = documents
                            .Where(s => s.ConsumerCategorie.Name!.Contains(model.CurrentFilter["ConsumerCategorie"]));
                        model.SelectedConsumerCategorie = model.CurrentFilter["ConsumerCategorie"];
                    }
                }
                }
            else
            {
                if (!string.IsNullOrEmpty(model.SelectedDistrict))
                {
                    settlementQuery = from doc in _context.ActionsDocuments
                                      orderby doc.Settlement.Name
                                      where doc.Settlement.District.Name == model.SelectedDistrict
                                      select doc.Settlement.Name;
                    documents = documents
                        .Where(d => d.Settlement.District.Name!.Contains(model.SelectedDistrict));
                }
                if (!string.IsNullOrEmpty(model.SelectedSettlement))
                {
                    documents = documents
                        .Where(s => s.Settlement.Name!.Contains(model.SelectedSettlement));
                }
                if (!string.IsNullOrEmpty(model.SelectedPlanningIndicator))
                {
                    documents = documents
                        .Where(s => s.PlanningIndicator.Name!.Contains(model.SelectedPlanningIndicator));
                }
                if (!string.IsNullOrEmpty(model.SelectedConsumerCategorie))
                {
                    documents = documents
                        .Where(s => s.ConsumerCategorie.Name!.Contains(model.SelectedConsumerCategorie));
                }
            }
            
        }

        public void ApplySort(string sortOrder, ref IQueryable<ActionsDocument> documents)
        {
            ViewData["SettlementSortParm"] = String.IsNullOrEmpty(sortOrder) ? "settlement_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["ConsumerCategorieSortParm"] = sortOrder == "cons_categ" ? "cons_categ_desc" : "cons_categ";
            ViewData["PlanningIndicatorSortParm"] = sortOrder == "planning_indicator" ? "planning_indicator_desc" : "planning_indicator";
            ViewData["ActualPercentSortParm"] = sortOrder == "percentage" ? "percentage_desc" : "percentage";
            documents = sortOrder switch
            {
                "cons_categ_desc" => documents.OrderByDescending(s => s.ConsumerCategorie.Name),
                "cons_categ" => documents.OrderBy(s => s.ConsumerCategorie.Name),
                "planning_indicator_desc" => documents.OrderByDescending(s => s.PlanningIndicator.Name),
                "planning_indicator" => documents.OrderBy(s => s.PlanningIndicator.Name),
                "percentage_desc" => documents.OrderByDescending(s => Convert.ToDouble(s.Fact_Count) / Convert.ToDouble(s.Plan_Count) * 1000),
                "percentage" => documents.OrderBy(s => Convert.ToDouble(s.Fact_Count) / Convert.ToDouble(s.Plan_Count) * 1000),
                "date" => documents.OrderBy(s => s.Date),
                "date_desc" => documents.OrderByDescending(s => s.Date),
                "settlement_desc" => documents.OrderByDescending(s => s.Settlement.Name),
                _ => documents.OrderBy(s => s.Settlement.Name),
            };
        }

        // GET: ActionsDocuments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ActionsDocuments == null)
            {
                return NotFound();
            }

            var actionsDocument = await _context.ActionsDocuments
                .Include(a => a.ConsumerCategorie)
                .Include(a => a.PlanningIndicator)
                .Include(a => a.Settlement)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actionsDocument == null)
            {
                return NotFound();
            }

            return View(actionsDocument);
        }

        [Authorize(Roles = "Moderator")]
        // GET: ActionsDocuments/Create
        public async Task<IActionResult> Create()
        {
            var categorieQuery =
                from doc in _context.ConsumerCategories
                orderby doc.Name
                select doc;

            var planIndicatorQuery =
                from doc in _context.PlanningIndicators
                orderby doc.Name
                select doc;

            var settlementQuery =
                from doc in _context.Settlements
                orderby doc.Name
                select doc;

            ViewData["Settlements"] = new SelectList(await settlementQuery.Distinct().ToListAsync(), "Id", "Name");
            ViewData["PlanningIndicators"] = new SelectList(await planIndicatorQuery.Distinct().ToListAsync(), "Id", "Name");
            ViewData["ConsumerCategories"] = new SelectList(await categorieQuery.Distinct().ToListAsync(), "Id", "Name");
           
            return View();
        }

        // POST: ActionsDocuments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Create([Bind(RequiredFieldNames)] ActionsDocument actionsDocument)
        {
            await SetFields(actionsDocument);
            ModelState.Clear();
            TryValidateModel(actionsDocument);
            if (!ModelState.IsValid)
            {
                var categorieQuery =
                    from doc in _context.ConsumerCategories
                    orderby doc.Name
                    select doc;

                var planIndicatorQuery =
                    from doc in _context.PlanningIndicators
                    orderby doc.Name
                    select doc;

                var settlementQuery =
                    from doc in _context.Settlements
                    orderby doc.Name
                    select doc;
                ViewData["Settlements"] = new SelectList(await settlementQuery.Distinct().ToListAsync(), "Id", "Name");
                ViewData["PlanningIndicators"] = new SelectList(await planIndicatorQuery.Distinct().ToListAsync(), "Id", "Name");
                ViewData["ConsumerCategories"] = new SelectList(await categorieQuery.Distinct().ToListAsync(), "Id", "Name");
                return View();
            }
            _context.Add(actionsDocument);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private static bool CheckFields(ActionsDocument actionsDocument)
        {
            if (actionsDocument is null)
            {
                return false;
            }
            if (actionsDocument.PlanningIndicator == null
                || actionsDocument.ConsumerCategorie == null
                || actionsDocument.Settlement == null)
            {

                return false;
            }
            return true;
        }

        private async Task SetFields(ActionsDocument actionsDocument)
        {
            actionsDocument.PlanningIndicator = await _context.PlanningIndicators.FirstAsync(x => x.Id == actionsDocument.PlanningIndicatorId);
            actionsDocument.ConsumerCategorie = await _context.ConsumerCategories.FirstAsync(x => x.Id == actionsDocument.ConsumerCategorieId);
            actionsDocument.Settlement = await _context.Settlements.FirstAsync(x => x.Id == actionsDocument.SettlementId);
        }

        [Authorize(Roles = "Moderator")]
        // GET: ActionsDocuments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActionsDocuments == null)
            {
                return NotFound();
            }

            var actionsDocument = await _context.ActionsDocuments.FindAsync(id);
            if (actionsDocument == null)
            {
                return NotFound();
            }

            var categorieQuery =
                from doc in _context.ConsumerCategories
                orderby doc.Name
                select doc;
            var planIndicatorQuery =
                from doc in _context.PlanningIndicators
                orderby doc.Name
                select doc;
            var settlementQuery =
                from doc in _context.Settlements
                orderby doc.Name
                select doc;

            ViewData["Settlements"] = new SelectList(await settlementQuery.Distinct().ToListAsync(), "Id", "Name", actionsDocument.Settlement);
            ViewData["PlanningIndicators"] = new SelectList(await planIndicatorQuery.Distinct().ToListAsync(), "Id", "Name", actionsDocument.PlanningIndicator);
            ViewData["ConsumerCategories"] = new SelectList(await categorieQuery.Distinct().ToListAsync(), "Id", "Name", actionsDocument.ConsumerCategorie);

            return View(actionsDocument);
        }

        // POST: ActionsDocuments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(int id, [Bind(RequiredFieldNames)] ActionsDocument actionsDocument)
        {
            if (id != actionsDocument.Id)
            {
                return NotFound();
            }
            await SetFields(actionsDocument);
            ModelState.Clear();
            TryValidateModel(actionsDocument);
            if (!ModelState.IsValid)
            {
                var categorieQuery =
                     from doc in _context.ConsumerCategories
                     orderby doc.Name
                     select doc;
                var planIndicatorQuery =
                    from doc in _context.PlanningIndicators
                    orderby doc.Name
                    select doc;
                var settlementQuery =
                    from doc in _context.Settlements
                    orderby doc.Name
                    select doc;
                ViewData["Settlements"] = new SelectList(await settlementQuery.Distinct().ToListAsync(), "Id", "Name", actionsDocument.Settlement);
                ViewData["PlanningIndicators"] = new SelectList(await planIndicatorQuery.Distinct().ToListAsync(), "Id", "Name", actionsDocument.PlanningIndicator);
                ViewData["ConsumerCategories"] = new SelectList(await categorieQuery.Distinct().ToListAsync(), "Id", "Name", actionsDocument.ConsumerCategorie);
                return View(actionsDocument);
            }
            try
            {
                _context.Update(actionsDocument);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActionsDocumentExists(actionsDocument.Id))
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

        [Authorize(Roles = "Moderator")]
        // GET: ActionsDocuments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActionsDocuments == null)
            {
                return NotFound();
            }

            var actionsDocument = await _context.ActionsDocuments
                .Include(a => a.ConsumerCategorie)
                .Include(a => a.PlanningIndicator)
                .Include(a => a.Settlement)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (actionsDocument == null)
            {
                return NotFound();
            }

            return View(actionsDocument);
        }

        // POST: ActionsDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActionsDocuments == null)
            {
                return Problem("Entity set 'OperativkaContext.ActionsDocuments'  is null.");
            }
            var actionsDocument = await _context.ActionsDocuments.FindAsync(id);
            if (actionsDocument != null)
            {
                _context.ActionsDocuments.Remove(actionsDocument);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActionsDocumentExists(int id)
        {
            return (_context.ActionsDocuments?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: ActionsDocuments
        [HttpPost, ActionName("Filter")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Filter(int FDistrict, int FSettlement)
        {
            ViewData["ConsumerCategorieId"] = new SelectList(_context.ConsumerCategories, "Id", "Name");
            ViewData["PlanningIndicatorId"] = new SelectList(_context.PlanningIndicators, "Id", "Name");
            ViewData["SettlementId"] = new SelectList(_context.Settlements.Where(x=> FDistrict > 0 ? x.DistrictId == FDistrict : x.Id > 0), "Id", "Name", FSettlement);
            ViewData["DistrictId"] = new SelectList(_context.Districts, "Id", "Name", FDistrict);
            var operativkaContext = _context.ActionsDocuments.Include(a => a.ConsumerCategorie).Include(a => a.PlanningIndicator).Include(a => a.Settlement);
            return View("Index",
                await operativkaContext
                .Where(x => FDistrict > 0 ? x.Settlement.DistrictId == FDistrict : x.Id > 0)
                .Where(x => FSettlement > 0 ? x.SettlementId == FSettlement :x.Id>0).ToListAsync()) ;
        }
    }
}
