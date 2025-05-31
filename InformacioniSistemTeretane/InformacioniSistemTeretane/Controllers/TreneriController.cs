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
    public class TreneriController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TreneriController> _logger;

        public TreneriController(ApplicationDbContext context, ILogger<TreneriController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Treneri
        [HttpGet]
        [Route("")]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Pregled trenera - korisnik: {Korisnik}", User.Identity.Name);
            var treneri = await _context.Treneri
                .Include(t => t.Zaposlenik)
                .ToListAsync();

            return View(treneri);
        }

        // GET: Treneri/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details: ID nije dostupan");
                return NotFound();
            }

            var trener = await _context.Treneri
                .Include(t => t.Zaposlenik)
                .Include(t => t.GrupniTreninzi)
                .Include(t => t.PersonalniTreninzi)
                .Include(t => t.ProbniTreninzi)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trener == null)
            {
                _logger.LogWarning("Details: Trener nije pronađen. ID: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Pregled detalja trenera ID: {Id}", id);
            return View(trener);
        }

        // GET: Treneri/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public IActionResult Create()
        {
            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime");
            _logger.LogInformation("Prikaz forme za novog trenera - korisnik: {Korisnik}", User.Identity.Name);
            return View();
        }

        // POST: Treneri/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Create([Bind("ZaposlenikId")] Trener trener)
        {
            _logger.LogInformation(
                "Kreiranje novog trenera - korisnik: {Korisnik}. Podaci: {@Trener}",
                User.Identity.Name, trener);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(trener);
                    await _context.SaveChangesAsync();

                    TempData["Uspjeh"] = "Trener uspješno dodan";
                    _logger.LogInformation("Trener uspješno dodan ID: {Id}", trener.Id);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri kreiranju trenera");
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                    TempData["Greska"] = "Greška pri dodavanju trenera";
                }
            }
            else
            {
                _logger.LogWarning("Neuspješna validacija forme za novog trenera. Broj grešaka: {BrojGresaka}",
                    ModelState.ErrorCount);
            }

            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime", trener.ZaposlenikId);
            return View(trener);
        }

        // GET: Treneri/Edit/5
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

            var trener = await _context.Treneri.FindAsync(id);
            if (trener == null)
            {
                _logger.LogWarning("Edit: Trener nije pronađen. ID: {Id}", id);
                return NotFound();
            }

            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime", trener.ZaposlenikId);
            _logger.LogInformation("Prikaz forme za uređivanje trenera ID: {Id}", id);
            return View(trener);
        }

        // POST: Treneri/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ZaposlenikId")] Trener trener)
        {
            if (id != trener.Id)
            {
                _logger.LogWarning("Edit: ID u putanji ({IdPut}) i modelu ({IdModel}) se ne podudaraju",
                    id, trener.Id);
                return NotFound();
            }

            _logger.LogInformation(
                "Ažuriranje trenera ID: {Id} - korisnik: {Korisnik}. Podaci: {@Trener}",
                id, User.Identity.Name, trener);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trener);
                    await _context.SaveChangesAsync();

                    TempData["Uspjeh"] = "Trener uspješno ažuriran";
                    _logger.LogInformation("Trener uspješno ažuriran ID: {Id}", id);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TrenerExists(trener.Id))
                    {
                        _logger.LogWarning("Edit: Trener nije pronađen nakon DbUpdateConcurrencyException. ID: {Id}", id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Greška pri ažuriranju trenera ID: {Id}", id);
                        ModelState.AddModelError("", "Greška pri spremanju promjena. Pokušajte ponovno.");
                        TempData["Greska"] = "Greška pri ažuriranju trenera";
                    }
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri ažuriranju trenera ID: {Id}", id);
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                    TempData["Greska"] = "Greška pri ažuriranju trenera";
                }
            }
            else
            {
                _logger.LogWarning("Neuspješna validacija forme za uređivanje. Broj grešaka: {BrojGresaka}",
                    ModelState.ErrorCount);
            }

            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime", trener.ZaposlenikId);
            return View(trener);
        }

        // GET: Treneri/Delete/5
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

            var trener = await _context.Treneri
                .Include(t => t.Zaposlenik)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trener == null)
            {
                _logger.LogWarning("Delete: Trener nije pronađen. ID: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Prikaz forme za brisanje trenera ID: {Id}", id);
            return View(trener);
        }

        // POST: Treneri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Brisanje trenera ID: {Id} - korisnik: {Korisnik}",
                id, User.Identity.Name);

            var trener = await _context.Treneri.FindAsync(id);
            if (trener == null)
            {
                _logger.LogWarning("DeleteConfirmed: Trener nije pronađen. ID: {Id}", id);
                TempData["Greska"] = "Trener nije pronađen";
                return RedirectToAction(nameof(Index));
            }

            // Provjera zavisnosti - da li trener ima treninge
            var hasTrainings = await _context.GrupniTreninzi.AnyAsync(g => g.TrenerId == id) ||
                               await _context.PersonalniTreninzi.AnyAsync(p => p.TrenerId == id) ||
                               await _context.ProbniTreninzi.AnyAsync(p => p.TrenerId == id);

            if (hasTrainings)
            {
                TempData["Greska"] = "Trener se ne može obrisati jer ima povezane treninge";
                _logger.LogWarning("Brisanje trenera ID: {Id} nije moguće jer postoje povezani treningi", id);
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Treneri.Remove(trener);
                await _context.SaveChangesAsync();

                TempData["Uspjeh"] = "Trener uspješno obrisan";
                _logger.LogInformation("Trener uspješno obrisan ID: {Id}", id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju trenera ID: {Id}", id);
                TempData["Greska"] = "Greška pri brisanju trenera";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TrenerExists(int id)
        {
            return _context.Treneri.Any(e => e.Id == id);
        }
    }
}