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

namespace ClassRoom.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class StudentsController : Controller
    {
        private readonly Databasecon _context;

        public StudentsController(Databasecon context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var databasecon = _context.Student.Include(s => s.Department);
            return View(await databasecon.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.Id == id);

            ViewBag.Courses =
            await _context.StudentCourse
               .Include(s => s.Courses)
               .Where(m => m.StudentId == id).ToListAsync();

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentId,FirstName,LastName,Address,Number,DateOfBirth,DepartmentId")] Student student)
        {
            if (ModelState.IsValid)
            {
                bool isDuplicate = _context.Student.Any(p => p.StudentId == student.StudentId);
                ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", student.DepartmentId);
                if (isDuplicate)
                {
                    ModelState.AddModelError("StudentId", "A Student with this Id already exists.");
                    return View(student);
                }
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", student.DepartmentId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", student.DepartmentId);
            return View(student);
        }

        [Authorize(Roles = "Admin")]
        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,FirstName,LastName,Address,Number,DateOfBirth,DepartmentId")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                bool isDuplicate = _context.Student.Any(p => p.StudentId == student.StudentId);
                ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", student.DepartmentId);
                if (isDuplicate)
                {
                    ModelState.AddModelError("StudentId", "A Student with this Id already exists.");
                    return View(student);
                }
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", student.DepartmentId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [Authorize(Roles = "Admin")]
        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Student == null)
            {
                return Problem("Entity set 'Databasecon.Student'  is null.");
            }
            var student = await _context.Student.FindAsync(id);
            if (student != null)
            {
                var studentCourses = await _context.StudentCourse.Where(m => m.StudentId == student.Id).ToListAsync();
                _context.StudentCourse.RemoveRange(studentCourses);
                _context.Student.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
          return (_context.Student?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
