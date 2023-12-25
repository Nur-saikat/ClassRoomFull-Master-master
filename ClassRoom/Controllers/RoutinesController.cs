using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassRoom.Areas.Identity.Data;
using ClassRoom.Models.Room_Booking;
using classroombooking.DataCreate;
using Microsoft.Extensions.Primitives;
using ClassRoom.Models.DataCreate;
using Microsoft.AspNetCore.Authorization;

namespace ClassRoom.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
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
            var databasecon = _context.Routines.Include(r => r.Course!).Include(r => r.Lecturers!).Include(r => r.Sessions!);
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
            StringValues days;

            Request.Form.TryGetValue("days[]", out days);

            if (days.Count == 0)
                ViewBag.Status = "Please select at least one day";

            var sessionCourse = await _context.Routines
                .Where(s => s.CourseId == routine.CourseId && s.LecturerId == routine.LecturerId && s.SessionId == routine.SessionId).ToListAsync();

            var isDuplicate = sessionCourse.Any();

            if (isDuplicate)
                ViewBag.Status = "Please change your Routine(Room Alrady Upkeep)";
            

            if (ModelState.IsValid && days.Count > 0 && !isDuplicate)
            {
                _context.Add(routine);

                await _context.SaveChangesAsync();


                var session = _context
                   .Session
                   .FirstOrDefault(s => s.Id == routine.SessionId);

                var routing = _context
                   .Rooms
                   .FirstOrDefault(s => s.Id == routine.SessionId);

                DateTime startDate = session.Session_Start_Date.Date;
                DateTime endDate = session.Session_End_Date.Date;

                var holidays = await _context.Hdays.Where(h =>
                        (h.Start_Date >= startDate && h.Start_Date <= endDate) ||
                        (h.End_Date >= startDate && h.End_Date <= endDate))
                        .ToListAsync();

                while (startDate <= endDate)
                {
                    if (
                        !days.Contains(startDate.DayOfWeek.GetHashCode().ToString()) ||
                        isInHolidays(startDate, holidays)
                       )
                    {
                        startDate = startDate.AddDays(1);
                        continue;
                    }

                    _context.Add(new Booking()
                    {
                        Class_Date = startDate,
                        RoomId = RoomId,
                        SlotId = SlotId,
                        RoutineId = routine.Id
                    });

                    startDate = startDate.AddDays(1);
                }

                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", routine.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", routine.LecturerId);
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name", routine.SessionId);

            ViewData["Rooms"] = new SelectList(_context.Rooms, "Id", "Name", RoomId);
            ViewData["Slots"] = new SelectList(_context.Slots, "Id", "Name", SlotId);

            bool isInHolidays(DateTime date, List<Hday> holidayList)
            {
                var isHoliday = false;

                foreach (var hd in holidayList)
                {
                    if (date >= hd.Start_Date && date <= hd.End_Date)
                        isHoliday = true;
                }

                return isHoliday;
            }

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
                var booking = await _context.Bookings.Where(m => m.RoutineId == routine.Id).ToListAsync();
                _context.Bookings.RemoveRange(booking);
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
