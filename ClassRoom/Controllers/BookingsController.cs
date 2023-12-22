using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassRoom.Areas.Identity.Data;
using ClassRoom.Models.Room_Booking;
using Microsoft.AspNetCore.Authorization;

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

        // GET: Bookings2
        public async Task<IActionResult> Index()
        {
            var databasecon = _context.Bookings.Include(b => b.Rooms).Include(b => b.Routines).Include(b => b.Slots);
            return View(await databasecon.ToListAsync());
        }

        // GET: Bookings2/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Rooms)
                .Include(b => b.Routines)
                .Include(b => b.Slots)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings2/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name");
            ViewData["RoutineId"] = new SelectList(_context.Routines, "Id", "Id");
            ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Name");
            return View();
        }

        // POST: Bookings2/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SlotId,Class_Date,RoomId,RoutineId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                var slot = _context
                    .Slots
                    .FirstOrDefault(s => s.Id == booking.SlotId);

                var bookings = await _context
                    .Bookings
                    .Include(b => b.Slots)
                    .Where(b => b.RoomId == booking.RoomId &&
                            (
                                (slot.StartTime.TimeOfDay <= b.Slots.StartTime.TimeOfDay && slot.EndTime.TimeOfDay >= b.Slots.StartTime.TimeOfDay) ||
                                (slot.StartTime.TimeOfDay <= b.Slots.EndTime.TimeOfDay && slot.EndTime.TimeOfDay >= b.Slots.EndTime.TimeOfDay) ||
                                (b.Slots.StartTime.TimeOfDay <= slot.StartTime.TimeOfDay && b.Slots.StartTime.TimeOfDay >= slot.EndTime.TimeOfDay) ||
                                (b.Slots.EndTime.TimeOfDay <= slot.StartTime.TimeOfDay && b.Slots.EndTime.TimeOfDay >= slot.EndTime.TimeOfDay)
                            )
                        )
                    .ToListAsync();


                if (bookings.Any())
                {
                    ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
                    ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Name", booking.SlotId);

                    ViewBag.Status = "The Booking Could not be created.There were no available Room";

                    return View(booking);
                }
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
            ViewData["RoutineId"] = new SelectList(_context.Routines, "Id", "Id", booking.RoutineId);
            ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Name", booking.SlotId);
            return View(booking);
        }

        // GET: Bookings2/Edit/5
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
            ViewData["RoutineId"] = new SelectList(_context.Routines, "Id", "Id", booking.RoutineId);
            ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Name", booking.SlotId);
            return View(booking);
        }

        // POST: Bookings2/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SlotId,Class_Date,RoomId,RoutineId")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    var slot = _context
                   .Slots
                   .FirstOrDefault(s => s.Id == booking.SlotId);

                    var bookings = await _context
                        .Bookings
                        .Include(b => b.Slots)
                        .Where(b => b.RoomId == booking.RoomId &&
                                (
                                    (slot.StartTime.TimeOfDay <= b.Slots.StartTime.TimeOfDay && slot.EndTime.TimeOfDay >= b.Slots.StartTime.TimeOfDay) ||
                                    (slot.StartTime.TimeOfDay <= b.Slots.EndTime.TimeOfDay && slot.EndTime.TimeOfDay >= b.Slots.EndTime.TimeOfDay) ||
                                    (b.Slots.StartTime.TimeOfDay <= slot.StartTime.TimeOfDay && b.Slots.StartTime.TimeOfDay >= slot.EndTime.TimeOfDay) ||
                                    (b.Slots.EndTime.TimeOfDay <= slot.StartTime.TimeOfDay && b.Slots.EndTime.TimeOfDay >= slot.EndTime.TimeOfDay)
                                )
                            )
                        .ToListAsync();


                    if (bookings.Any())
                    {
                        ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
                        ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Name", booking.SlotId);

                        ViewBag.Status = "The Booking Could not be created.There were no available Room";

                        return View(booking);
                    }


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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", booking.RoomId);
            ViewData["RoutineId"] = new SelectList(_context.Routines, "Id", "Id", booking.RoutineId);
            ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Name", booking.SlotId);
            return View(booking);
        }

        // GET: Bookings2/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bookings == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Rooms)
                .Include(b => b.Routines)
                .Include(b => b.Slots)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
        [Authorize(Roles = "Admin")]
        // POST: Bookings2/Delete/5
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
