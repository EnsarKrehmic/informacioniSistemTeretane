using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;
using Microsoft.AspNetCore.Authorization;

namespace InformacioniSistemTeretane.Controllers
{
    public class TakmicenjaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TakmicenjaController> _logger;

        public TakmicenjaController(ApplicationDbContext context, ILogger<TakmicenjaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Takmicenja
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Pregled takmičenja - korisnik: {Korisnik}", User.Identity.Name);
            var takmicenja = await _context.Takmicenja
                .Include(t => t.Lokacija)
                .ToListAsync();

            return View(takmicenja);
        }

        // GET: Takmicenja/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details: ID nije dostupan");
                return NotFound();
            }

            var takmicenje = await _context.Takmicenja
                .Include(t => t.Lokacija)
                .Include(t => t.Discipline)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (takmicenje == null)
            {
                _logger.LogWarning("Details: Takmičenje nije pronađeno. ID: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Pregled detalja takmičenja ID: {Id}", id);
            return View(takmicenje);
        }

        // GET: Takmicenja/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public IActionResult Create()
        {
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv");
            _logger.LogInformation("Prikaz forme za novo takmičenje - korisnik: {Korisnik}", User.Identity.Name);
            return View();
        }

        // POST: Takmicenja/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Create([Bind("Naziv,Datum,LokacijaId,Opis,Kotizacija")] Takmicenje takmicenje)
        {
            _logger.LogInformation(
                "Kreiranje novog takmičenja - korisnik: {Korisnik}. Podaci: {@Takmicenje}",
                User.Identity.Name, takmicenje);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(takmicenje);
                    await _context.SaveChangesAsync();

                    TempData["Uspjeh"] = "Takmičenje uspješno kreirano";
                    _logger.LogInformation("Takmičenje uspješno kreirano ID: {Id}", takmicenje.Id);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri kreiranju takmičenja");
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                    TempData["Greska"] = "Greška pri kreiranju takmičenja";
                }
            }
            else
            {
                _logger.LogWarning("Neuspješna validacija forme za novo takmičenje. Broj grešaka: {BrojGresaka}",
                    ModelState.ErrorCount);
            }

            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", takmicenje.LokacijaId);
            return View(takmicenje);
        }

        // GET: Takmicenja/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit: ID nije dostupan");
                return NotFound();
            }

            var takmicenje = await _context.Takmicenja.FindAsync(id);
            if (takmicenje == null)
            {
                _logger.LogWarning("Edit: Takmičenje nije pronađeno. ID: {Id}", id);
                return NotFound();
            }

            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", takmicenje.LokacijaId);
            _logger.LogInformation("Prikaz forme za uređivanje takmičenja ID: {Id}", id);
            return View(takmicenje);
        }

        // POST: Takmicenja/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Datum,LokacijaId,Opis,Kotizacija")] Takmicenje takmicenje)
        {
            if (id != takmicenje.Id)
            {
                _logger.LogWarning("Edit: ID u putanji ({IdPut}) i modelu ({IdModel}) se ne podudaraju",
                    id, takmicenje.Id);
                return NotFound();
            }

            _logger.LogInformation(
                "Ažuriranje takmičenja ID: {Id} - korisnik: {Korisnik}. Podaci: {@Takmicenje}",
                id, User.Identity.Name, takmicenje);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(takmicenje);
                    await _context.SaveChangesAsync();

                    TempData["Uspjeh"] = "Takmičenje uspješno ažurirano";
                    _logger.LogInformation("Takmičenje uspješno ažurirano ID: {Id}", id);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TakmicenjeExists(takmicenje.Id))
                    {
                        _logger.LogWarning("Edit: Takmičenje nije pronađeno nakon DbUpdateConcurrencyException. ID: {Id}", id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Greška pri ažuriranju takmičenja ID: {Id}", id);
                        ModelState.AddModelError("", "Greška pri spremanju promjena. Pokušajte ponovno.");
                        TempData["Greska"] = "Greška pri ažuriranju takmičenja";
                    }
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri ažuriranju takmičenja ID: {Id}", id);
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                    TempData["Greska"] = "Greška pri ažuriranju takmičenja";
                }
            }
            else
            {
                _logger.LogWarning("Neuspješna validacija forme za uređivanje. Broj grešaka: {BrojGresaka}",
                    ModelState.ErrorCount);
            }

            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", takmicenje.LokacijaId);
            return View(takmicenje);
        }

        // GET: Takmicenja/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete: ID nije dostupan");
                return NotFound();
            }

            var takmicenje = await _context.Takmicenja
                .Include(t => t.Lokacija)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (takmicenje == null)
            {
                _logger.LogWarning("Delete: Takmičenje nije pronađeno. ID: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Prikaz forme za brisanje takmičenja ID: {Id}", id);
            return View(takmicenje);
        }

        // POST: Takmicenja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Brisanje takmičenja ID: {Id} - korisnik: {Korisnik}",
                id, User.Identity.Name);

            var takmicenje = await _context.Takmicenja.FindAsync(id);
            if (takmicenje == null)
            {
                _logger.LogWarning("DeleteConfirmed: Takmičenje nije pronađeno. ID: {Id}", id);
                TempData["Greska"] = "Takmičenje nije pronađeno";
                return RedirectToAction(nameof(Index));
            }

            // Provjera zavisnosti - da li takmičenje ima discipline
            var hasDiscipline = await _context.Discipline.AnyAsync(d => d.TakmicenjeId == id);
            if (hasDiscipline)
            {
                TempData["Greska"] = "Takmičenje se ne može obrisati jer ima povezane discipline";
                _logger.LogWarning("Brisanje takmičenja ID: {Id} nije moguće jer postoje povezane discipline", id);
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Takmicenja.Remove(takmicenje);
                await _context.SaveChangesAsync();

                TempData["Uspjeh"] = "Takmičenje uspješno obrisano";
                _logger.LogInformation("Takmičenje uspješno obrisano ID: {Id}", id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju takmičenja ID: {Id}", id);
                TempData["Greska"] = "Greška pri brisanju takmičenja";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TakmicenjeExists(int id)
            => _context.Takmicenja.Any(e => e.Id == id);
    }
}