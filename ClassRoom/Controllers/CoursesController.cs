using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassRoom.Areas.Identity.Data;
using classroombooking.DataCreate;
using Microsoft.AspNetCore.Authorization;
using ClassRoom.Models.DataCreate;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ClassRoom.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class CoursesController : Controller
    {
        private readonly Databasecon _context;

        public CoursesController(Databasecon context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var databasecon = _context.Courses.Include(c => c.Lecturers);
            return View(await databasecon.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Lecturers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Code,Credits,LecturerId")] Course course)
        {
            if (ModelState.IsValid)
            {
                ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", course.LecturerId);
                bool isDuplicate = _context.Courses.Any(p => p.Name == course.Name);

                if (isDuplicate)
                {
                    ModelState.AddModelError("Name", "A course with this name already exists.");
                    return View(course);
                }
                bool isDuplicateCode = _context.Courses.Any(p => p.Code == course.Code);

                if (isDuplicateCode)
                {
                    ModelState.AddModelError("Code", "A course with this Code already exists.");
                    return View(course);
                }
             

                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", course.LecturerId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", course.LecturerId);
            return View(course);
        }
        [Authorize(Roles = "Admin")]
        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Code,Credits,LecturerId")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", course.LecturerId);
                bool isDuplicate = _context.Courses.Any(p => p.Name == course.Name);

                if (isDuplicate)
                {
                    ModelState.AddModelError("Name", "A course with this name already exists.");
                    return View(course);
                }
                bool isDuplicateCode = _context.Courses.Any(p => p.Code == course.Code);

                if (isDuplicateCode)
                {
                    ModelState.AddModelError("Code", "A course with this Code already exists.");
                    return View(course);
                }


               
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", course.LecturerId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Lecturers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
        [Authorize(Roles = "Admin")]
        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'Databasecon.Courses'  is null.");
            }
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
