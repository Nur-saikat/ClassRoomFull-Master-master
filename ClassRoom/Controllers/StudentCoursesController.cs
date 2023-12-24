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
using DocumentFormat.OpenXml.Office2010.Excel;

namespace ClassRoom.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class StudentCoursesController : Controller
    {
        private readonly Databasecon _context;

        public StudentCoursesController(Databasecon context)
        {
            _context = context;
        }

        // GET: StudentCourses
        public async Task<IActionResult> Index()
        {
            var databasecon = _context.StudentCourse.Include(s => s.Courses).Include(s => s.Sessions).Include(s => s.Students);
            return View(await databasecon.ToListAsync());
        }

        // GET: StudentCourses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StudentCourse == null)
            {
                return NotFound();
            }

            var studentCourse = await _context.StudentCourse
                .Include(s => s.Courses)
                .Include(s => s.Students)
                .Include(s => s.Sessions)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentCourse == null)
            {
                return NotFound();
            }

            return View(studentCourse);
        }

        // GET: StudentCourses/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName");
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name");
            return View();
        }

        // POST: StudentCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,CourseId,SessionId")] StudentCourse studentCourse)
        {
            if (ModelState.IsValid)
            {

                bool isDuplicate = _context.StudentCourse.Any(p => p.CourseId == studentCourse.CourseId && p.SessionId==studentCourse.SessionId);

                if (isDuplicate)
                {
                    ViewBag.Status = "A course with this name already exists.";
                    ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", studentCourse.CourseId);
                    ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", studentCourse.StudentId);
                    ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name", studentCourse.SessionId);
                   
                    return View(studentCourse);
                }
                _context.Add(studentCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", studentCourse.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", studentCourse.StudentId);
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name", studentCourse.SessionId);
            return View(studentCourse);
        }

        // GET: StudentCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StudentCourse == null)
            {
                return NotFound();
            }

            var studentCourse = await _context.StudentCourse.FindAsync(id);
            if (studentCourse == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", studentCourse.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", studentCourse.StudentId);
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name", studentCourse.SessionId);
            return View(studentCourse);
        }

        [Authorize(Roles = "Admin")]
        // POST: StudentCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,CourseId,SessionId")] StudentCourse studentCourse)
        {
            if (id != studentCourse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentCourseExists(studentCourse.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", studentCourse.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", studentCourse.StudentId);
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name", studentCourse.SessionId);
            return View(studentCourse);
        }

        // GET: StudentCourses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StudentCourse == null)
            {
                return NotFound();
            }

            var studentCourse = await _context.StudentCourse
                .Include(s => s.Courses)
                .Include(s => s.Students)
                .Include(s => s.Sessions)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentCourse == null)
            {
                return NotFound();
            }

            return View(studentCourse);
        }

        [Authorize(Roles = "Admin")]
        // POST: StudentCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StudentCourse == null)
            {
                return Problem("Entity set 'Databasecon.StudentCourse'  is null.");
            }
            var studentCourse = await _context.StudentCourse.FindAsync(id);
            if (studentCourse != null)
            {
                _context.StudentCourse.Remove(studentCourse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentCourseExists(int id)
        {
            return (_context.StudentCourse?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> GetBySession(int studentId, int sessionId)
        {
            var course = await _context.StudentCourse
                .Include(c => c.Courses)
                .Where(c => c.StudentId == studentId && c.SessionId == sessionId)
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
