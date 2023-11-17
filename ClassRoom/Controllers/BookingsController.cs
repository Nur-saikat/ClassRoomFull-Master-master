using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassRoom.Areas.Identity.Data;
using classroombooking.DataCreate;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using ClassRoom.Models;

namespace ClassRoom.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class BookingsController : Controller
    {
        private readonly Databasecon _context;

        public BookingsController(Databasecon context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(FilterClassRoutine filter = null)
        {
            filter = filter ?? new FilterClassRoutine();
            ViewData["Filter"] = filter;
            ViewData["FilterErrorMsg"] = "";

            var bookingsData = await _context.Bookings.Include(b => b.Course).Include(b => b.Lecturers).Include(b => b.Rooms).ToListAsync();
            if (filter.StartDate != null && filter.EndDate != null && filter.StartDate < filter.EndDate)
            {
                bookingsData = bookingsData.Where(d => d.StartDate >= filter.StartDate && d.EndDate <= filter.EndDate).ToList();
            }
            else if (filter.StartDate != null && filter.EndDate != null && filter.StartDate > filter.EndDate)
            {
                ViewData["FilterErrorMsg"] = "Start date of filter can not be less than end date!";
            }

            return View(bookingsData);
        }

        // GET: Bookings/Details/5
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,EndDate,Finish,LecturerId,RoomId,CourseId")] Booking booking)
        {

            DateTime StartDate = booking.StartDate;
            DateTime EndDate = booking.EndDate;
            DateTime DateTime = booking.Finish;
            int DayInterval = 7;
            if (ModelState.IsValid)
            {
                

                if (StartDate < DateTime.Now || StartDate > DateTime)
                {
                    ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", booking.LecturerId);
                    ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
                    ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", booking.CourseId);
                    if (StartDate < DateTime.Now)
                        ViewBag.Status = "The Start Date & Time cannot be in the past.";
                    else
                        ViewBag.Status = "The Start Date & Time cannot be later than the End Date & Time.";
                    return View(booking);
                }

                ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", booking.LecturerId);
                ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
                ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", booking.CourseId);
                bool isDuplicate = _context.Bookings.Any(p =>
                  p.RoomId == booking.RoomId &&
                  (
                      (booking.StartDate >= p.StartDate && booking.StartDate < p.EndDate) ||
                      (booking.EndDate > p.StartDate && booking.EndDate <= p.EndDate)
                  )
);

                if (isDuplicate)
                {
                    ViewBag.Status = "The Booking Could not be created.Ther were no available Room";
                    return View(booking);
                }



                while (StartDate <= DateTime)
                {
                    var newData = new Booking()
                    {
                        StartDate = StartDate,
                        EndDate = EndDate,
                        Finish = DateTime,
                        Course = booking.Course,
                        CourseId = booking.CourseId,
                        //Id = routing.Id,
                        LecturerId = booking.LecturerId,
                        Lecturers = booking.Lecturers,
                        Rooms = booking.Rooms,
                        RoomId = booking.RoomId,
                    };



                    _context.Add(newData);

                    _context.SaveChanges();
                    StartDate = StartDate.AddDays(DayInterval);
                    EndDate = EndDate.AddDays(DayInterval);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", booking.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", booking.LecturerId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
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
            return View(booking);
        }

        [Authorize(Roles = "Admin")]
        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,EndDate,Finish,LecturerId,RoomId,CourseId")] Booking booking)
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
            return View(booking);
        }

        // GET: Bookings/Delete/5
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [Authorize(Roles = "Admin")]
        // POST: Bookings/Delete/5
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



        public IActionResult Room_Booking()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Room_Booking(int id, [Bind("Id,StartDate,EndDate,LecturerId,RoomId,CourseId")] Booking booking)
        {
            DateTime StartDate = booking.StartDate;
            DateTime EndDate = booking.EndDate;
            

            if (ModelState.IsValid)
            {
                int RoomId = +1;

                if (StartDate < DateTime.Now || StartDate > EndDate)
                {
                    ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", booking.LecturerId);
                    ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
                    ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", booking.CourseId);
                    if (StartDate < DateTime.Now)
                        ViewBag.Status = "The Start Date & Time cannot be in the past.";
                    else
                        ViewBag.Status = "The Booking Could not be created.Ther were no available Room";

                    return View(booking);
                }

                ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", booking.LecturerId);
                ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
                ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", booking.CourseId);
                bool isDuplicate = _context.Bookings.Any(p => p.RoomId == booking.RoomId &&
                                                              ((p.StartDate <= booking.StartDate && p.EndDate >= booking.StartDate) ||
                                                              (p.StartDate <= booking.EndDate && p.EndDate >= booking.EndDate) ||
                                                              (p.StartDate >= booking.StartDate && p.EndDate <= booking.EndDate)));

                if (isDuplicate)
                {
                    ViewBag.Status = "The Booking Could not be created.Ther were no available Room";
                    return View(booking);
                }




                _context.Add(booking);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", booking.CourseId);
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", booking.LecturerId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
            return View(booking);
        }

    }
}
