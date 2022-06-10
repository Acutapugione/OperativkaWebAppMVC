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
using static Operativka.Areas.Identity.Models.Enums;

namespace Operativka.Controllers
{
    [Authorize(Roles = "Moderator")]
    public class ConsumersController : Controller
    {
        public const string RequiredFieldNames = "Id,FirstName,LastName,Patronymic,Address,PhoneNumber,PersonalAccountCode,DistrictId";

        private readonly OperativkaContext _context;

        public ConsumersController(OperativkaContext context)
        {
            _context = context;
        }

        // GET: Consumers
        public async Task<IActionResult> Index(ConsumerViewModel model, string sortOrder, string? filterJsoned)
        {
            return View(await GetViewFilledViewModel(model, filterJsoned, sortOrder));
        }

        private async Task<ConsumerViewModel> GetViewFilledViewModel(ConsumerViewModel model, string? filterJsoned, string sortOrder)
        {
            IQueryable<string> districtQuery =
                from consumer in _context.Consumers
                orderby consumer.District.Name
                select consumer.District.Name;

            var consumers = from con in _context.Consumers
                            .Include(a => a.District)
                            select con;

            if (filterJsoned != null)
            {
                model.CurrentFilter = JsonSerializer.Deserialize<Dictionary<string, string>>(filterJsoned);
            }
            else
            {
                filterJsoned = model.CurrentFilter != null ? JsonSerializer.Serialize(model.CurrentFilter) : null;
            }

            ApplyFilters(ref consumers, model);

            ApplySort(sortOrder, ref consumers);

            int pageSize = 10;

            var currFilter = new Dictionary<string, string>
            {
                { "District", model.SelectedDistrict },
            };

            var ConsumerViewModel = new ConsumerViewModel
            {
                Districts = new SelectList(await districtQuery.Distinct().ToListAsync(), model.SelectedDistrict),
                SelectedDistrict = model.SelectedDistrict,
                InsertedPersonalAccountCode = model.InsertedPersonalAccountCode,

                CurrentFilter = currFilter,
                pageNumber = model.pageNumber,
                Consumers = await PaginatedList<Consumer>.CreateAsync(consumers.AsNoTracking(), model.pageNumber ?? 1, pageSize)
            };
            return ConsumerViewModel;
        }

        private void ApplySort(string sortOrder, ref IQueryable<Consumer> consumers)
        {
            ViewData["FullNameParam"] = String.IsNullOrEmpty(sortOrder) ? "full_name_desc" : "";
            ViewData["PersonalAccountCodeParam"] = sortOrder == "LS" ? "LS_desc" : "LS";
            ViewData["DistrictParam"] = sortOrder == "district" ? "district_desc" : "district";
            ViewData["AddressParam"] = sortOrder == "address" ? "address_desc" : "address";
            ViewData["PhoneNumberParam"] = sortOrder == "phone_num" ? "phone_num_desc" : "phone_num";
            consumers = sortOrder switch
            {
                "LS_desc" => consumers.OrderByDescending(s => s.PersonalAccountCode),
                "LS" => consumers.OrderBy(s => s.PersonalAccountCode),

                "district_desc" => consumers.OrderByDescending(s => s.District.Name),
                "district" => consumers.OrderBy(s => s.District.Name),

                "address_desc" => consumers.OrderByDescending(s => s.Address),
                "address" => consumers.OrderBy(s => s.Address),

                "phone_num_desc" => consumers.OrderByDescending(s => s.PhoneNumber),
                "phone_num" => consumers.OrderBy(s => s.PhoneNumber),

                "full_name_desc" => consumers.OrderByDescending(s => s.LastName + s.FirstName + s.Patronymic),
                _ => consumers.OrderBy(s => s.LastName + s.FirstName + s.Patronymic),
            };
        }

        private void ApplyFilters(ref IQueryable<Consumer> consumers, ConsumerViewModel model)
        {
            if (model.CurrentFilter != null)
            {
                if (model.CurrentFilter.ContainsKey("PersonalAccountCode"))
                {
                    if (!string.IsNullOrEmpty(model.CurrentFilter["PersonalAccountCode"]))
                    {
                        consumers = consumers
                            .Where(d => d.PersonalAccountCode.ToString()!.Contains(model.CurrentFilter["PersonalAccountCode"]));
                        model.InsertedPersonalAccountCode = model.CurrentFilter["PersonalAccountCode"];
                    }
                }
                if (model.CurrentFilter.ContainsKey("District"))
                {
                    if (!string.IsNullOrEmpty(model.CurrentFilter["District"]))
                    {
                        consumers = consumers
                            .Where(d => d.District.Name!.Contains(model.CurrentFilter["District"]));
                        model.SelectedDistrict = model.CurrentFilter["District"];
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(model.SelectedDistrict))
                {
                    consumers = consumers
                        .Where(d => d.District.Name!.Contains(model.SelectedDistrict));
                }
                if (!string.IsNullOrEmpty(model.InsertedPersonalAccountCode))
                {
                    consumers = consumers
                        .Where(d => d.PersonalAccountCode.ToString()!.Contains(model.InsertedPersonalAccountCode));
                }
            }
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
        public async Task<IActionResult> Create()
        {
            await FillViewDataAsync();
            return View();
        }

        // POST: Consumers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(RequiredFieldNames)] Consumer consumer)
        {
            await SetFields(consumer);
            if (CheckFields(consumer))
            {
                _context.Add(consumer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await FillViewDataAsync(consumer);
            return View();
        }

        private async Task FillViewDataAsync(Consumer? consumer = null)
        {
            var districtQuery =
                 from district in _context.Districts
                 orderby district.Name
                 select district;

            ViewData["Districts"] = new SelectList(await districtQuery.Distinct().ToListAsync(), "Id", "Name", consumer != null ? consumer.District : null);
        }

        private bool CheckFields(Consumer consumer)
        {
            if (consumer is null) return false;
            if (consumer.District is null) return false;
            return true;
        }

        private async Task SetFields(Consumer consumer)
        {
            consumer.District = await _context.Districts.FirstAsync(x => x.Id == consumer.DistrictId);
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
            await FillViewDataAsync(consumer);
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
            await SetFields(consumer);
            if (CheckFields(consumer))
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

            await FillViewDataAsync(consumer);
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
