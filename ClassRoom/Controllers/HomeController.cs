using ClassRoom.Areas.Identity.Data;
using ClassRoom.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ClassRoom.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController>_Context;
        private readonly Databasecon _context;
        public HomeController(Databasecon context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var databasecon = _context.Bookings.Include(b => b.Lecturers).Include(b => b.Rooms);
            return View(await databasecon.ToListAsync());
        }

        //public IActionResult Index()
        //{
            

        //    return View();
        //}


     
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}