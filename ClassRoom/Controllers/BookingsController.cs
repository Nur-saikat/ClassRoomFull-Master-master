using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassRoom.Areas.Identity.Data;
using classroombooking.DataCreate;
using ClassRoom.Models;
using Lecturer = ClassRoom.Models.DataCreate.Lecturer;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using ClassRoom.Models.DataCreate;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Vml;

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
        public async Task<IActionResult> Index()
        {
            var databasecon = _context.Bookings.Include(b => b.Lecturers).Include(b => b.Rooms);
            return View(await databasecon.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
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
            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime,IsActive,LecturerId,RoomId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                int RoomId = +1;
                DateTime StartTime = booking.StartTime;
                DateTime EndTime = booking.EndTime;
                if (StartTime < DateTime.Now || StartTime > EndTime)
                {
                    ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", booking.LecturerId);
                    ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);

                    if (StartTime < DateTime.Now)
                        ViewBag.Status = "The Start Date & Time cannot be in the past.";
                    else
                        ViewBag.Status = "The Start Date & Time cannot be later than the End Date & Time.";

                    return View(booking);
                }

                ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", booking.LecturerId);
                ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);

                bool isDuplicate = _context.Bookings.Any(p => p.RoomId == booking.RoomId &&
                                                              ((p.StartTime <= booking.StartTime && p.EndTime >= booking.StartTime) ||
                                                              (p.StartTime <= booking.EndTime && p.EndTime >= booking.EndTime) ||
                                                              (p.StartTime >= booking.StartTime && p.EndTime <= booking.EndTime)));

                if (isDuplicate)
                {
                    ViewBag.Status = "The Booking Could not be created.Ther were no available Room";
                    return View(booking);
                }

                //var activeBookings = _context.Bookings.Where(b => b.IsActive).ToList();
                //foreach (var Room in _context.Rooms)
                //{
                //    var activeBookingsForCurrentRoom = activeBookings.Where(b => b.RoomId == Room.Id);
                //    if (activeBookingsForCurrentRoom.All(b => StartTime < b.StartTime &&
                //    EndTime < b.StartTime || StartTime > b.EndTime && EndTime > b.EndTime))
                //    {
                //        RoomId = Room.Id;
                //        break;
                //    }
                //}

                ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", booking.LecturerId);
                ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);


                _context.Add(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["LecturerId"] = new SelectList(_context.Lecturers, "Id", "FullName", booking.LecturerId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
            ViewBag.Status = "The Booking Could not be created.Ther were no available Room";
            
            return View();
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime,IsActive,LecturerId,RoomId")] Booking booking)
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
    }
}


