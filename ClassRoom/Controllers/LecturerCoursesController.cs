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

namespace ClassRoom.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class LecturerCoursesController : Controller
    {
        private readonly Databasecon _context;

        public LecturerCoursesController(Databasecon context)
        {
            _context = context;
        }

        // GET: LecturerCourses
        public async Task<IActionResult> Index()
        {
            var databasecon = _context.LecturerCourses.Include(l => l.Courses).Include(l => l.Lecturers).Include(s => s.Sessions);
            return View(await databasecon.ToListAsync());
        }

        // GET: LecturerCourses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LecturerCourses == null)
            {
                return NotFound();
            }

            var lecturerCourse = await _context.LecturerCourses
                .Include(l => l.Courses)
                .Include(l => l.Lecturers)
                .Include(s => s.Sessions)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecturerCourse == null)
            {
                return NotFound();
            }

            return View(lecturerCourse);
        }

        // GET: LecturerCourses/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName");
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name");
            return View();
        }

        // POST: LecturerCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LecturerId,CourseId,SessionId")] LecturerCourse lecturerCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lecturerCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", lecturerCourse.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", lecturerCourse.LecturerId);
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name", lecturerCourse.SessionId);
            return View(lecturerCourse);
        }

        // GET: LecturerCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LecturerCourses == null)
            {
                return NotFound();
            }

            var lecturerCourse = await _context.LecturerCourses.FindAsync(id);
            if (lecturerCourse == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", lecturerCourse.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", lecturerCourse.LecturerId);
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name", lecturerCourse.SessionId);
            return View(lecturerCourse);
        }

        [Authorize(Roles = "Admin")]
        // POST: LecturerCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LecturerId,CourseId,SessionId")] LecturerCourse lecturerCourse)
        {
            if (id != lecturerCourse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lecturerCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LecturerCourseExists(lecturerCourse.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", lecturerCourse.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", lecturerCourse.LecturerId);
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name", lecturerCourse.SessionId);
            return View(lecturerCourse);
        }

        // GET: LecturerCourses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LecturerCourses == null)
            {
                return NotFound();
            }

            var lecturerCourse = await _context.LecturerCourses
                .Include(l => l.Courses)
                .Include(l => l.Lecturers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecturerCourse == null)
            {
                return NotFound();
            }

            return View(lecturerCourse);
        }

        [Authorize(Roles = "Admin")]
        // POST: LecturerCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LecturerCourses == null)
            {
                return Problem("Entity set 'Databasecon.LecturerCourses'  is null.");
            }
            var lecturerCourse = await _context.LecturerCourses.FindAsync(id);
            if (lecturerCourse != null)
            {
                _context.LecturerCourses.Remove(lecturerCourse);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LecturerCourseExists(int id)
        {
          return (_context.LecturerCourses?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> GetBySession(int lecturerid, int sessionId)
        {
            var course = await _context.LecturerCourses
                .Include(c => c.Courses)
                .Where(c => c.LecturerId == lecturerid && c.SessionId == sessionId)
            .Select(c => new
            {
                c.Courses.Id,
                c.Courses.Code,
                c.Courses.Name
            })
                .ToListAsync();

            return Json(course);
        }
    }
}
