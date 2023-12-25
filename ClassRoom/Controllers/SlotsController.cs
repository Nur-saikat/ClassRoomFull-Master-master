using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassRoom.Areas.Identity.Data;
using ClassRoom.Models.DataCreate;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Drawing;
using classroombooking.DataCreate;

namespace ClassRoom.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SlotsController : Controller
    {
        private readonly Databasecon _context;

        public SlotsController(Databasecon context)
        {
            _context = context;
        }

        // GET: Slods
        public async Task<IActionResult> Index()
        {
              return _context.Slots != null ? 
                          View(await _context.Slots.ToListAsync()) :
                          Problem("Entity set 'Databasecon.Slods'  is null.");
        }

        // GET: Slods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Slots == null)
            {
                return NotFound();
            }

            var slod = await _context.Slots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slod == null)
            {
                return NotFound();
            }

            return View(slod);
        }

        // GET: Slods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Slods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,EndTime")] Slot slot)
        {
            if (ModelState.IsValid)
            {
                bool isDuplicate = _context.Slots.Any(p => p.StartTime.TimeOfDay >= slot.StartTime.TimeOfDay);
                
                if (isDuplicate)
                {
                    ViewBag.Status = "A Class time Upkeep already .";
                    return View(slot);
                }


                _context.Add(slot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(slot);
        }

        // GET: Slods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Slots == null)
            {
                return NotFound();
            }

            var slod = await _context.Slots.FindAsync(id);
            if (slod == null)
            {
                return NotFound();
            }
            return View(slod);
        }
        [Authorize(Roles = "Admin")]
        // POST: Slods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,EndTime")] Slot slot)
        {
            if (id != slot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool isDuplicate = _context.Slots.Any(p => p.StartTime.TimeOfDay == slot.StartTime.TimeOfDay);

                    if (isDuplicate)
                    {
                        ViewBag.Status = "A Class time Upkeep already .";
                        return View(slot);
                    }
                    _context.Update(slot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlotExists(slot.Id))
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
            return View(slot);
        }

        // GET: Slods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Slots == null)
            {
                return NotFound();
            }

            var slot = await _context.Slots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (slot == null)
            {
                return NotFound();
            }

            return View(slot);
        }
        [Authorize(Roles = "Admin")]
        // POST: Slods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Slots == null)
            {
                return Problem("Entity set 'Databasecon.Slods'  is null.");
            }
            var slot = await _context.Slots.FindAsync(id);
            if (slot != null)
            {
                var AllSlot = await _context.Slots.Where(m => m.Id == slot.Id).ToListAsync();
                _context.Slots.RemoveRange(slot);
                _context.Slots.Remove(slot);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SlotExists(int id)
        {
          return (_context.Slots?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
