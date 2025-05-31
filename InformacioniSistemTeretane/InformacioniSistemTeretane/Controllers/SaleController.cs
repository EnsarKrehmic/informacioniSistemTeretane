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
    [Authorize] // Zahtjeva autentifikaciju za sve akcije
    public class SaleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SaleController> _logger;

        public SaleController(ApplicationDbContext context, ILogger<SaleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Sale
        [HttpGet]
        [Route("")]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Pregled sala - korisnik: {Korisnik}", User.Identity.Name);
            var sale = await _context.Sale
                .Include(s => s.Lokacija)
                .ToListAsync();

            return View(sale);
        }

        // GET: Sale/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details: ID nije dostupan");
                return NotFound();
            }

            var sala = await _context.Sale
                .Include(s => s.Lokacija)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sala == null)
            {
                _logger.LogWarning("Details: Sala nije pronađena. ID: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Pregled detalja sale ID: {Id}", id);
            return View(sala);
        }

        // GET: Sale/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public IActionResult Create()
        {
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv");
            _logger.LogInformation("Prikaz forme za novu salu - korisnik: {Korisnik}", User.Identity.Name);
            return View();
        }

        // POST: Sale/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Create([Bind("Naziv,Kapacitet,LokacijaId")] Sala sala)
        {
            _logger.LogInformation(
                "Kreiranje nove sale - korisnik: {Korisnik}. Podaci: {@Sala}",
                User.Identity.Name, sala);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(sala);
                    await _context.SaveChangesAsync();

                    TempData["Uspjeh"] = "Sala uspješno kreirana";
                    _logger.LogInformation("Sala uspješno kreirana ID: {Id}", sala.Id);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri kreiranju sale");
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                    TempData["Greska"] = "Greška pri kreiranju sale";
                }
            }
            else
            {
                _logger.LogWarning("Neuspješna validacija forme za novu salu. Broj grešaka: {BrojGresaka}",
                    ModelState.ErrorCount);

                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        foreach (var error in entry.Value.Errors)
                        {
                            _logger.LogWarning(
                                " - Polje '{Field}': {ErrorMessage}",
                                entry.Key,
                                error.ErrorMessage);
                        }
                    }
                }
            }

            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", sala.LokacijaId);
            return View(sala);
        }

        // GET: Sale/Edit/5
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

            var sala = await _context.Sale.FindAsync(id);
            if (sala == null)
            {
                _logger.LogWarning("Edit: Sala nije pronađena. ID: {Id}", id);
                return NotFound();
            }

            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", sala.LokacijaId);
            _logger.LogInformation("Prikaz forme za uređivanje sale ID: {Id}", id);
            return View(sala);
        }

        // POST: Sale/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Kapacitet,LokacijaId")] Sala sala)
        {
            if (id != sala.Id)
            {
                _logger.LogWarning("Edit: ID u putanji ({IdPut}) i modelu ({IdModel}) se ne podudaraju",
                    id, sala.Id);
                return NotFound();
            }

            _logger.LogInformation(
                "Ažuriranje sale ID: {Id} - korisnik: {Korisnik}. Podaci: {@Sala}",
                id, User.Identity.Name, sala);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sala);
                    await _context.SaveChangesAsync();

                    TempData["Uspjeh"] = "Sala uspješno ažurirana";
                    _logger.LogInformation("Sala uspješno ažurirana ID: {Id}", id);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!SalaExists(sala.Id))
                    {
                        _logger.LogWarning("Edit: Sala nije pronađena nakon DbUpdateConcurrencyException. ID: {Id}", id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Greška pri ažuriranju sale ID: {Id}", id);
                        ModelState.AddModelError("", "Greška pri spremanju promjena. Pokušajte ponovno.");
                        TempData["Greska"] = "Greška pri ažuriranju sale";
                    }
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri ažuriranju sale ID: {Id}", id);
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                    TempData["Greska"] = "Greška pri ažuriranju sale";
                }
            }
            else
            {
                _logger.LogWarning("Neuspješna validacija forme za uređivanje. Broj grešaka: {BrojGresaka}",
                    ModelState.ErrorCount);
            }

            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", sala.LokacijaId);
            return View(sala);
        }

        // GET: Sale/Delete/5
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

            var sala = await _context.Sale
                .Include(s => s.Lokacija)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sala == null)
            {
                _logger.LogWarning("Delete: Sala nije pronađena. ID: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Prikaz forme za brisanje sale ID: {Id}", id);
            return View(sala);
        }

        // POST: Sale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Brisanje sale ID: {Id} - korisnik: {Korisnik}",
                id, User.Identity.Name);

            var sala = await _context.Sale.FindAsync(id);
            if (sala == null)
            {
                _logger.LogWarning("DeleteConfirmed: Sala nije pronađena. ID: {Id}", id);
                TempData["Greska"] = "Sala nije pronađena";
                return RedirectToAction(nameof(Index));
            }

            // Provjera zavisnosti - da li se sala koristi u nekom treningu
            var hasDependencies = await _context.GrupniTreninzi.AnyAsync(t => t.SalaId == id);
            if (hasDependencies)
            {
                TempData["Greska"] = "Sala se ne može obrisati jer se koristi u treningu";
                _logger.LogWarning("Brisanje sale ID: {Id} nije moguće jer postoje zavisni treningi", id);
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Sale.Remove(sala);
                await _context.SaveChangesAsync();

                TempData["Uspjeh"] = "Sala uspješno obrisana";
                _logger.LogInformation("Sala uspješno obrisana ID: {Id}", id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju sale ID: {Id}", id);
                TempData["Greska"] = "Greška pri brisanju sale";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SalaExists(int id)
        {
            return _context.Sale.Any(e => e.Id == id);
        }
    }
}