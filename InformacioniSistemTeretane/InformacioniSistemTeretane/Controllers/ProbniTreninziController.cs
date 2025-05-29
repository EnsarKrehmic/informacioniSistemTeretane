using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;

namespace InformacioniSistemTeretane.Controllers
{
    [Authorize] // Zahtjeva autentifikaciju za sve akcije
    public class ProbniTreninziController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProbniTreninziController> _logger;

        public ProbniTreninziController(
            ApplicationDbContext context,
            ILogger<ProbniTreninziController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ProbniTreninzi
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Pregled probnih treninga - korisnik: {Korisnik}", User.Identity.Name);

            var probniTreninzi = await _context.ProbniTreninzi
                .Include(p => p.Klijent)
                .Include(p => p.Trener)
                    .ThenInclude(t => t.Zaposlenik)
                .ToListAsync();

            return View(probniTreninzi);
        }

        // GET: ProbniTreninzi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details: ID nije dostupan");
                return NotFound();
            }

            var probniTrening = await _context.ProbniTreninzi
                .Include(x => x.Klijent)
                .Include(x => x.Trener)
                    .ThenInclude(t => t.Zaposlenik)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (probniTrening == null)
            {
                _logger.LogWarning("Details: Probni trening nije pronađen. ID: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Pregled detalja probnog treninga ID: {Id}", id);
            return View(probniTrening);
        }

        // GET: ProbniTreninzi/Create
        [Authorize(Roles = "Admin,Zaposlenik")]
        public IActionResult Create()
        {
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime");
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new {
                        t.Id,
                        Name = $"{t.Zaposlenik.Prezime}, {t.Zaposlenik.Ime}"
                    }),
                "Id", "Name");

            _logger.LogInformation("Prikaz forme za novi probni trening - korisnik: {Korisnik}", User.Identity.Name);
            return View();
        }

        // POST: ProbniTreninzi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Create([Bind("Naziv,Opis,KlijentId,TrenerId,Datum,Ocjena")] ProbniTrening probniTrening)
        {
            _logger.LogInformation(
                "Kreiranje novog probnog treninga - korisnik: {Korisnik}. Podaci: {@ProbniTrening}",
                User.Identity.Name, probniTrening);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(probniTrening);
                    await _context.SaveChangesAsync();

                    TempData["Uspjeh"] = "Probni trening uspješno kreiran";
                    _logger.LogInformation("Probni trening uspješno kreiran ID: {Id}", probniTrening.Id);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri kreiranju probnog treninga");
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                    TempData["Greska"] = "Greška pri kreiranju probnog treninga";
                }
            }
            else
            {
                _logger.LogWarning("Neuspješna validacija forme za novi probni trening. Broj grešaka: {BrojGresaka}",
                    ModelState.ErrorCount);
            }

            // Ponovno popuni padajuće liste ako dođe do greške
            ViewBag.KlijentId = new SelectList(
                _context.Klijenti, "Id", "Prezime", probniTrening.KlijentId);
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new {
                        t.Id,
                        Name = $"{t.Zaposlenik.Prezime}, {t.Zaposlenik.Ime}"
                    }),
                "Id", "Name", probniTrening.TrenerId);

            return View(probniTrening);
        }

        // GET: ProbniTreninzi/Edit/5
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit: ID nije dostupan");
                return NotFound();
            }

            var probniTrening = await _context.ProbniTreninzi.FindAsync(id);
            if (probniTrening == null)
            {
                _logger.LogWarning("Edit: Probni trening nije pronađen. ID: {Id}", id);
                return NotFound();
            }

            ViewBag.KlijentId = new SelectList(
                _context.Klijenti, "Id", "Prezime", probniTrening.KlijentId);
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new {
                        t.Id,
                        Name = $"{t.Zaposlenik.Prezime}, {t.Zaposlenik.Ime}"
                    }),
                "Id", "Name", probniTrening.TrenerId);

            _logger.LogInformation("Prikaz forme za uređivanje probnog treninga ID: {Id}", id);
            return View(probniTrening);
        }

        // POST: ProbniTreninzi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,KlijentId,TrenerId,Datum,Ocjena")] ProbniTrening probniTrening)
        {
            if (id != probniTrening.Id)
            {
                _logger.LogWarning("Edit: ID u putanji ({IdPut}) i modelu ({IdModel}) se ne podudaraju",
                    id, probniTrening.Id);
                return NotFound();
            }

            _logger.LogInformation(
                "Ažuriranje probnog treninga ID: {Id} - korisnik: {Korisnik}. Podaci: {@ProbniTrening}",
                id, User.Identity.Name, probniTrening);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(probniTrening);
                    await _context.SaveChangesAsync();

                    TempData["Uspjeh"] = "Probni trening uspješno ažuriran";
                    _logger.LogInformation("Probni trening uspješno ažuriran ID: {Id}", id);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!ProbniTreningExists(probniTrening.Id))
                    {
                        _logger.LogWarning("Edit: Probni trening nije pronađen nakon DbUpdateConcurrencyException. ID: {Id}", id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Greška pri ažuriranju probnog treninga ID: {Id}", id);
                        ModelState.AddModelError("", "Greška pri spremanju promjena. Pokušajte ponovno.");
                        TempData["Greska"] = "Greška pri ažuriranju probnog treninga";
                    }
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri ažuriranju probnog treninga ID: {Id}", id);
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                    TempData["Greska"] = "Greška pri ažuriranju probnog treninga";
                }
            }
            else
            {
                _logger.LogWarning("Neuspješna validacija forme za uređivanje. Broj grešaka: {BrojGresaka}",
                    ModelState.ErrorCount);
            }

            // Ponovno popuni padajuće liste ako dođe do greške
            ViewBag.KlijentId = new SelectList(
                _context.Klijenti, "Id", "Prezime", probniTrening.KlijentId);
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new {
                        t.Id,
                        Name = $"{t.Zaposlenik.Prezime}, {t.Zaposlenik.Ime}"
                    }),
                "Id", "Name", probniTrening.TrenerId);

            return View(probniTrening);
        }

        // GET: ProbniTreninzi/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete: ID nije dostupan");
                return NotFound();
            }

            var probniTrening = await _context.ProbniTreninzi
                .Include(x => x.Klijent)
                .Include(x => x.Trener)
                    .ThenInclude(t => t.Zaposlenik)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (probniTrening == null)
            {
                _logger.LogWarning("Delete: Probni trening nije pronađen. ID: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Prikaz forme za brisanje probnog treninga ID: {Id}", id);
            return View(probniTrening);
        }

        // POST: ProbniTreninzi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Brisanje probnog treninga ID: {Id} - korisnik: {Korisnik}",
                id, User.Identity.Name);

            var probniTrening = await _context.ProbniTreninzi.FindAsync(id);
            if (probniTrening == null)
            {
                _logger.LogWarning("DeleteConfirmed: Probni trening nije pronađen. ID: {Id}", id);
                TempData["Greska"] = "Probni trening nije pronađen";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.ProbniTreninzi.Remove(probniTrening);
                await _context.SaveChangesAsync();

                TempData["Uspjeh"] = "Probni trening uspješno obrisan";
                _logger.LogInformation("Probni trening uspješno obrisan ID: {Id}", id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju probnog treninga ID: {Id}", id);
                TempData["Greska"] = "Greška pri brisanju probnog treninga";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProbniTreningExists(int id)
        {
            return _context.ProbniTreninzi.Any(e => e.Id == id);
        }
    }
}