using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Operativka.Data;
using Operativka.Models;

namespace Operativka.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {

        private readonly OperativkaContext _context;

        public ReportsController(OperativkaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ActionsDocumentsZvit()
        {
            IQueryable<District> districtQuery =
               from doc in _context.ActionsDocuments
               orderby doc.Settlement.District.Name
               select doc.Settlement.District;

            IQueryable<ConsumerCategorie> categorieQuery =
                from doc in _context.ActionsDocuments
                orderby doc.ConsumerCategorie.Name
                select doc.ConsumerCategorie;

            IQueryable<PlanningIndicator> planIndicatorQuery =
                from doc in _context.ActionsDocuments
                orderby doc.PlanningIndicator.Name
                select doc.PlanningIndicator;

            IQueryable<Settlement> settlementQuery =
                from doc in _context.ActionsDocuments
                orderby doc.Settlement.Name
                select doc.Settlement;

            var documents = (from d in _context.ActionsDocuments
                           .Include(a => a.ConsumerCategorie)
                           .Include(a => a.PlanningIndicator)
                           .Include(a => a.Settlement)
                           select d);

            var ReportViewModel = new Report
            {
                Documents = await documents.Distinct().ToListAsync(),
                Districts = await districtQuery.Distinct().ToListAsync(),
                Settlements = await settlementQuery.Distinct().ToListAsync(),
                PlanningIndicators = await planIndicatorQuery.Distinct().ToListAsync(),
                ConsumerCategories = await categorieQuery.Distinct().ToListAsync(),
            };
            return View(ReportViewModel);
        }
    }
}
