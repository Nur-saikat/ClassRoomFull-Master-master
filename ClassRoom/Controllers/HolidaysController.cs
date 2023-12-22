using ClassRoom.Areas.Identity.Data;
using ClassRoom.Models.DataCreate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassRoom.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class HolidaysController : Controller
    {

        private readonly Databasecon _context;

        public HolidaysController(Databasecon context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return _context.Hdays != null ?
                        View(await _context.Hdays.ToListAsync()) :
                        Problem("Entity set 'Databasecon.holidays'  is null.");
        }

        // GET: Sessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hdays == null)
            {
                return NotFound();
            }

            var hday = await _context.Hdays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hday == null)
            {
                return NotFound();
            }

            return View(hday);
        }

        // GET: Sessions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Start_Date,End_Date")] Hday hday)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hday);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Index);
        }

        // GET: Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hdays == null)
            {
                return NotFound();
            }

            var hday = await _context.Hdays.FindAsync(id);
            if (hday == null)
            {
                return NotFound();
            }
            return View(hday);
        }
        [Authorize(Roles = "Admin")]
        // POST: Sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Start_Date,End_Date")] Hday hday)
        {
            if (id != hday.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hday);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!hdayExists(hday.Id))
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
            return View(hday);
        }

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hdays == null)
            {
                return NotFound();
            }

            var hday = await _context.Hdays
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hday == null)
            {
                return NotFound();
            }

            return View(hday);
        }
        [Authorize(Roles = "Admin")]
        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hdays == null)
            {
                return Problem("Entity set 'Databasecon.Hdays'  is null.");
            }
            var hday = await _context.Hdays.FindAsync(id);
            if (hday != null)
            {
                _context.Hdays.Remove(hday);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool hdayExists(int id)
        {
            return (_context.Session?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
