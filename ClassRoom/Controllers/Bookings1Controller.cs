using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassRoom.Areas.Identity.Data;
using classroombooking.DataCreate;

namespace ClassRoom.Controllers
{
    public class Bookings1Controller : Controller
    {
        private readonly Databasecon _context;

        public Bookings1Controller(Databasecon context)
        {
            _context = context;
        }

        // GET: Bookings1
        public async Task<IActionResult> Index()
        {
            var databasecon = _context.Bookings.Include(b => b.Course).Include(b => b.Lecturers).Include(b => b.Rooms).Include(b => b.Sessions).Include(b => b.Slods);
            return View(await databasecon.ToListAsync());
        }

        // GET: Bookings1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Course)
                .Include(b => b.Lecturers)
                .Include(b => b.Rooms)
                .Include(b => b.Sessions)
                .Include(b => b.Slods)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings1/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name");
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name");
            ViewData["SlodId"] = new SelectList(_context.Slods, "Id", "Name");
            return View();
        }

        // POST: Bookings1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SlodId,SessionId,LecturerId,RoomId,CourseId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", booking.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", booking.LecturerId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name", booking.SessionId);
            ViewData["SlodId"] = new SelectList(_context.Slods, "Id", "Name", booking.SlodId);
            return View(booking);
        }

        // GET: Bookings1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", booking.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", booking.LecturerId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name", booking.SessionId);
            ViewData["SlodId"] = new SelectList(_context.Slods, "Id", "Name", booking.SlodId);
            return View(booking);
        }

        // POST: Bookings1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SlodId,SessionId,LecturerId,RoomId,CourseId")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", booking.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", booking.LecturerId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
            ViewData["SessionId"] = new SelectList(_context.Session, "Id", "Name", booking.SessionId);
            ViewData["SlodId"] = new SelectList(_context.Slods, "Id", "Name", booking.SlodId);
            return View(booking);
        }

        // GET: Bookings1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Course)
                .Include(b => b.Lecturers)
                .Include(b => b.Rooms)
                .Include(b => b.Sessions)
                .Include(b => b.Slods)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bookings == null)
            {
                return Problem("Entity set 'Databasecon.Bookings'  is null.");
            }
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
          return (_context.Bookings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
