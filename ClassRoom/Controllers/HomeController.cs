using ClassRoom.Areas.Identity.Data;
using ClassRoom.Models;
using ClassRoom.Models.DataCreate;
using classroombooking.DataCreate;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ClassRoom.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController>_Context;
        private readonly Databasecon _context;
        public HomeController(Databasecon context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var databasecon = _context.Routines.Include(r => r.Course).Include(r => r.Lecturers).Include(r => r.Sessions);
            return View(await databasecon.ToListAsync());
        }

        //public IActionResult Index()
        //{


        //    return View();
        //}
        [HttpGet]
        public IActionResult Search(string SearchTerm)
        {
           
            return View();
        }

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
    