// Controllers/TreninziController.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InformacioniSistemTeretane.Controllers
{
    [Authorize]
    public class TreninziController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TreninziController> _logger;

        public TreninziController(
            ApplicationDbContext context,
            ILogger<TreninziController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Treninzi
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation(
                    "Korisnik {KorisnikId} pregleda listu treninga",
                    User.Identity.Name
                );

                var treninzi = await _context.Treninzi
                    .AsNoTracking()
                    .ToListAsync();

                return View(treninzi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška prilikom učitavanja treninga");
                TempData["Greska"] = "Došlo je do greške prilikom učitavanja treninga.";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Treninzi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning(
                    "Pokušaj pregleda detalja treninga bez ID-a od strane {Korisnik}",
                    User.Identity.Name
                );
                return NotFound();
            }

            try
            {
                _logger.LogInformation(
                    "Korisnik {Korisnik} pregleda detalje treninga ID: {TreningId}",
                    User.Identity.Name,
                    id
                );

                // Dinamičko učitavanje specifičnog tipa treninga
                var trening = await LoadTreningWithDetails(id.Value);

                if (trening == null)
                {
                    _logger.LogWarning(
                        "Trening ID {TreningId} nije pronađen. Korisnik: {Korisnik}",
                        id,
                        User.Identity.Name
                    );
                    TempData["Greska"] = "Trening nije pronađen.";
                    return NotFound();
                }

                return View(trening);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Greška prilikom učitavanja detalja treninga ID: {TreningId}. Korisnik: {Korisnik}",
                    id,
                    User.Identity.Name
                );
                TempData["Greska"] = "Došlo je do greške prilikom učitavanja detalja treninga.";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task<Trening> LoadTreningWithDetails(int id)
        {
            var trening = await _context.Treninzi
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trening == null) return null;

            // Učitaj specifične detalje na osnovu tipa treninga
            switch (trening)
            {
                case GrupniTrening grupni:
                    await _context.Entry(grupni)
                        .Reference(t => t.Sala)
                        .LoadAsync();
                    await _context.Entry(grupni)
                        .Reference(t => t.Trener)
                        .LoadAsync();
                    break;

                case PersonalniTrening personalni:
                    await _context.Entry(personalni)
                        .Reference(t => t.Trener)
                        .LoadAsync();
                    await _context.Entry(personalni)
                        .Reference(t => t.Klijent)
                        .LoadAsync();
                    break;

                case ProbniTrening probni:
                    await _context.Entry(probni)
                        .Reference(t => t.Trener)
                        .LoadAsync();
                    await _context.Entry(probni)
                        .Reference(t => t.Klijent)
                        .LoadAsync();
                    break;
            }

            return trening;
        }
    }
}