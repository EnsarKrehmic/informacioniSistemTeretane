using System.Diagnostics;
using InformacioniSistemTeretane.Models;
using InformacioniSistemTeretane.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace InformacioniSistemTeretane.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            // Dohvati sve lokacije teretana
            var gymLocations = await _context.Mape
                .Where(m => m.Tip == TipMape.Teretana)
                .ToListAsync();

            // Dodaj defaultnu lokaciju ako nema drugih
            if (!gymLocations.Any())
            {
                gymLocations.Add(new Mapa
                {
                    Id = 0,
                    Naziv = "Politehnički fakultet Zenica",
                    Adresa = "Zmaja od Bosne bb, 72000 Zenica",
                    Latitude = 44.2019,
                    Longitude = 17.9083,
                    Tip = TipMape.Teretana,
                    Opis = "Glavna lokacija teretane"
                });
            }

            // Proslijedi lokacije u ViewBag kao JSON
            ViewBag.GymLocations = JsonSerializer.Serialize(gymLocations);

            return View();
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
