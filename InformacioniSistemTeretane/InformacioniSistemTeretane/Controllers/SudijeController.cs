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
    public class SudijeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SudijeController> _logger;

        public SudijeController(ApplicationDbContext context, ILogger<SudijeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Sudije
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Pregled sudija - korisnik: {Korisnik}", User.Identity.Name);
            var sudije = await _context.Sudije
                .Include(s => s.Zaposlenik)
                .Include(s => s.Disciplina)
                .ToListAsync();

            return View(sudije);
        }

        // GET: Sudije/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details: ID nije dostupan");
                return NotFound();
            }

            var sudija = await _context.Sudije
                .Include(s => s.Zaposlenik)
                .Include(s => s.Disciplina)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sudija == null)
            {
                _logger.LogWarning("Details: Sudija nije pronađena. ID: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Pregled detalja sudije ID: {Id}", id);
            return View(sudija);
        }

        // GET: Sudije/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public IActionResult Create()
        {
            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime");
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv");
            _logger.LogInformation("Prikaz forme za novu sudiju - korisnik: {Korisnik}", User.Identity.Name);
            return View();
        }

        // POST: Sudije/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Create([Bind("ZaposlenikId,DisciplinaId")] Sudija sudija)
        {
            _logger.LogInformation(
                "Kreiranje nove sudije - korisnik: {Korisnik}. Podaci: {@Sudija}",
                User.Identity.Name, sudija);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(sudija);
                    await _context.SaveChangesAsync();

                    TempData["Uspjeh"] = "Sudija uspješno dodana";
                    _logger.LogInformation("Sudija uspješno dodana ID: {Id}", sudija.Id);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri kreiranju sudije");
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                    TempData["Greska"] = "Greška pri dodavanju sudije";
                }
            }
            else
            {
                _logger.LogWarning("Neuspješna validacija forme za novu sudiju. Broj grešaka: {BrojGresaka}",
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

            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime", sudija.ZaposlenikId);
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", sudija.DisciplinaId);
            return View(sudija);
        }

        // GET: Sudije/Edit/5
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

            var sudija = await _context.Sudije.FindAsync(id);
            if (sudija == null)
            {
                _logger.LogWarning("Edit: Sudija nije pronađena. ID: {Id}", id);
                return NotFound();
            }

            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime", sudija.ZaposlenikId);
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", sudija.DisciplinaId);
            _logger.LogInformation("Prikaz forme za uređivanje sudije ID: {Id}", id);
            return View(sudija);
        }

        // POST: Sudije/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ZaposlenikId,DisciplinaId")] Sudija sudija)
        {
            if (id != sudija.Id)
            {
                _logger.LogWarning("Edit: ID u putanji ({IdPut}) i modelu ({IdModel}) se ne podudaraju",
                    id, sudija.Id);
                return NotFound();
            }

            _logger.LogInformation(
                "Ažuriranje sudije ID: {Id} - korisnik: {Korisnik}. Podaci: {@Sudija}",
                id, User.Identity.Name, sudija);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sudija);
                    await _context.SaveChangesAsync();

                    TempData["Uspjeh"] = "Sudija uspješno ažurirana";
                    _logger.LogInformation("Sudija uspješno ažurirana ID: {Id}", id);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!SudijaExists(sudija.Id))
                    {
                        _logger.LogWarning("Edit: Sudija nije pronađena nakon DbUpdateConcurrencyException. ID: {Id}", id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Greška pri ažuriranju sudije ID: {Id}", id);
                        ModelState.AddModelError("", "Greška pri spremanju promjena. Pokušajte ponovno.");
                        TempData["Greska"] = "Greška pri ažuriranju sudije";
                    }
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri ažuriranju sudije ID: {Id}", id);
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                    TempData["Greska"] = "Greška pri ažuriranju sudije";
                }
            }
            else
            {
                _logger.LogWarning("Neuspješna validacija forme za uređivanje. Broj grešaka: {BrojGresaka}",
                    ModelState.ErrorCount);
            }

            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime", sudija.ZaposlenikId);
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", sudija.DisciplinaId);
            return View(sudija);
        }

        // GET: Sudije/Delete/5
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

            var sudija = await _context.Sudije
                .Include(s => s.Zaposlenik)
                .Include(s => s.Disciplina)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sudija == null)
            {
                _logger.LogWarning("Delete: Sudija nije pronađena. ID: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Prikaz forme za brisanje sudije ID: {Id}", id);
            return View(sudija);
        }

        // POST: Sudije/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Brisanje sudije ID: {Id} - korisnik: {Korisnik}",
                id, User.Identity.Name);

            var sudija = await _context.Sudije.FindAsync(id);
            if (sudija == null)
            {
                _logger.LogWarning("DeleteConfirmed: Sudija nije pronađena. ID: {Id}", id);
                TempData["Greska"] = "Sudija nije pronađena";
                return RedirectToAction(nameof(Index));
            }

            // Provjera zavisnosti - da li sudija sudi na nekom takmičenju
            var hasDependencies = await _context.Takmicenja.AnyAsync(t => t.SudijaId == id);
            if (hasDependencies)
            {
                TempData["Greska"] = "Sudija se ne može obrisati jer sudi na takmičenju";
                _logger.LogWarning("Brisanje sudije ID: {Id} nije moguće jer postoje zavisna takmičenja", id);
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Sudije.Remove(sudija);
                await _context.SaveChangesAsync();

                TempData["Uspjeh"] = "Sudija uspješno obrisana";
                _logger.LogInformation("Sudija uspješno obrisana ID: {Id}", id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju sudije ID: {Id}", id);
                TempData["Greska"] = "Greška pri brisanju sudije";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SudijaExists(int id)
        {
            return _context.Sudije.Any(e => e.Id == id);
        }
    }
}