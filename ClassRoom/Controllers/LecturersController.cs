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
using classroombooking.DataCreate;
using NuGet.Protocol;

namespace ClassRoom.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class LecturersController : Controller
    {
        private readonly Databasecon _context;

        public LecturersController(Databasecon context)
        {
            _context = context;
        }

        // GET: Lecturers
        public async Task<IActionResult> Index()
        {
              return _context.Lecturers != null ? 
                          View(await _context.Lecturers.ToListAsync()) :
                          Problem("Entity set 'Databasecon.Lecturers'  is null.");
        }

        // GET: Lecturers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lecturers == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers
                .FirstOrDefaultAsync(m => m.Id == id);

            ViewBag.Courses =
            await _context.LecturerCourses
               .Include(s => s.Courses)
               .Where(m => m.LecturerId == id).ToListAsync();

            var sessions = await _context.Session.ToListAsync();

            ViewBag.Sessions = new SelectList(sessions, "Id", "Name");

            if (lecturer == null)
            {
                return NotFound();
            }

            return View(lecturer);
        }

        // GET: Lecturers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lecturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LecturerId,FirstName,LastName,Number")] Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                bool isDuplicate = _context.Lecturers.Any(p => p.LecturerId == lecturer.LecturerId);
                
                if (isDuplicate)
                {
                    ModelState.AddModelError("LecturerId", "A Lecturer with this Id already exists.");
                    return View(lecturer);
                }
                _context.Add(lecturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lecturer);
        }

        // GET: Lecturers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lecturers == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }
            return View(lecturer);
        }

        [Authorize(Roles = "Admin")]
        // POST: Lecturers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LecturerId,FirstName,LastName,Number")] Lecturer lecturer)
        {
            if (id != lecturer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lecturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LecturerExists(lecturer.Id))
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
            return View(lecturer);
        }

        // GET: Lecturers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lecturers == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecturer == null)
            {
                return NotFound();
            }

            return View(lecturer);
        }

        [Authorize(Roles = "Admin")]
        // POST: Lecturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lecturers == null)
            {
                return Problem("Entity set 'Databasecon.Lecturers'  is null.");
            }
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer != null)
            {
                var lecturersCourse = await _context.LecturerCourses
                    .Where(m => m.LecturerId == lecturer.Id).ToListAsync();
                var lecturersrouting = await _context.Routines.Where(m => m.LecturerId == lecturer.Id).ToListAsync();
                
                _context.LecturerCourses.RemoveRange(lecturersCourse);
                _context.Lecturers.Remove(lecturer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LecturerExists(int id)
        {
          return (_context.Lecturers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
