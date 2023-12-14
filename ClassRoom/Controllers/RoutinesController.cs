using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassRoom.Areas.Identity.Data;
using ClassRoom.Models.Room_Booking;

namespace ClassRoom.Controllers
{
    public class RoutinesController : Controller
    {
        private readonly Databasecon _context;

        public RoutinesController(Databasecon context)
        {
            _context = context;
        }

        // GET: Routines
        public async Task<IActionResult> Index()
        {
            var databasecon = _context.Routines.Include(r => r.Course).Include(r => r.Lecturers).Include(r => r.Sessions);
            return View(await databasecon.ToListAsync());
        }

        // GET: Routines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Routines == null)
            {
                return NotFound();
            }

            var routine = await _context.Routines
                .Include(r => r.Course)
                .Include(r => r.Lecturers)
                .Include(r => r.Sessions)
                .Include(r => r.Bookings)
                    .ThenInclude(b => b.Rooms)
                .Include(r => r.Bookings)
                    .ThenInclude(b => b.Slots)
                .FirstOrDefaultAsync(m => m.Id == id);

            return View(routine);
        }

        // GET: Routines/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName");            
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name");

            ViewData["Rooms"] = new SelectList(_context.Rooms, "Id", "Name");
            ViewData["Slots"] = new SelectList(_context.Slots, "Id", "Name");

            return View();
        }

        // POST: Routines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SessionId,LecturerId,CourseId")] Routine routine, int? RoomId, int? SlotId)
        {
            if (ModelState.IsValid)
            {

                _context.Add(routine);

                await _context.SaveChangesAsync();


                var session = _context
                   .Session
                   .FirstOrDefault(s => s.Id == routine.SessionId);
                var routing = _context
                   .Rooms
                   
                   .FirstOrDefault(s => s.Id == routine.SessionId);


                DateTime SessionStartDate = session.Session_Start_Date;
                DateTime SessionEndDate = session.Session_End_Date;

              

                while (SessionStartDate.Date <= SessionEndDate.Date)
                {
                    SessionStartDate = SessionStartDate.AddDays(7);
                    
                    var newData = new Booking()
                    {
                       Class_Date = SessionStartDate,
                       RoomId = RoomId,
                       SlotId = SlotId,
                       RoutineId = routine.Id
                     

                    };

                    _context.Add(newData);
                   
                }
                await _context.SaveChangesAsync();
                

                return RedirectToAction(nameof(Index));
            }

            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", routine.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", routine.LecturerId);
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name", routine.SessionId);

            ViewData["Rooms"] = new SelectList(_context.Rooms, "Id", "Name", RoomId);
            ViewData["Slots"] = new SelectList(_context.Slots, "Id", "Name", SlotId);

            return View(routine);
        }

        // GET: Routines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Routines == null)
            {
                return NotFound();
            }

            var routine = await _context.Routines.FindAsync(id);
            if (routine == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", routine.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", routine.LecturerId);

            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name", routine.SessionId);

            return View(routine);
        }

        // POST: Routines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SessionId,LecturerId,CourseId")] Routine routine)
        {
            if (id != routine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(routine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoutineExists(routine.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", routine.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", routine.LecturerId);

            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name", routine.SessionId);

            return View(routine);
        }

        // GET: Routines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Routines == null)
            {
                return NotFound();
            }

            var routine = await _context.Routines
                .Include(r => r.Course)
                .Include(r => r.Lecturers)

                .Include(r => r.Sessions)

                .FirstOrDefaultAsync(m => m.Id == id);
            if (routine == null)
            {
                return NotFound();
            }

            return View(routine);
        }

        // POST: Routines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Routines == null)
            {
                return Problem("Entity set 'Databasecon.Routines'  is null.");
            }
            var routine = await _context.Routines.FindAsync(id);
            if (routine != null)
            {
                _context.Routines.Remove(routine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoutineExists(int id)
        {
            return (_context.Routines?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
