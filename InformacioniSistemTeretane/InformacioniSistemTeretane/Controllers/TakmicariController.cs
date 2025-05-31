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
    public class TakmicariController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TakmicariController> _logger;

        public TakmicariController(ApplicationDbContext context, ILogger<TakmicariController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Takmicari
        [HttpGet]
        [Route("")]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Pregled takmičara - korisnik: {Korisnik}", User.Identity.Name);
            var takmicari = await _context.Takmicari
                .Include(t => t.Klijent)
                .Include(t => t.Disciplina)
                .ToListAsync();

            return View(takmicari);
        }

        // GET: Takmicari/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details: ID nije dostupan");
                return NotFound();
            }

            var takmicar = await _context.Takmicari
                .Include(x => x.Klijent)
                .Include(x => x.Disciplina)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (takmicar == null)
            {
                _logger.LogWarning("Details: Takmičar nije pronađen. ID: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Pregled detalja takmičara ID: {Id}", id);
            return View(takmicar);
        }

        // GET: Takmicari/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public IActionResult Create()
        {
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime");
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv");
            _logger.LogInformation("Prikaz forme za novog takmičara - korisnik: {Korisnik}", User.Identity.Name);
            return View();
        }

        // POST: Takmicari/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Create([Bind("KlijentId,DisciplinaId,Rezultat,Pozicija")] Takmicar takmicar)
        {
            _logger.LogInformation(
                "Kreiranje novog takmičara - korisnik: {Korisnik}. Podaci: {@Takmicar}",
                User.Identity.Name, takmicar);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(takmicar);
                    await _context.SaveChangesAsync();

                    TempData["Uspjeh"] = "Takmičar uspješno dodan";
                    _logger.LogInformation("Takmičar uspješno dodan ID: {Id}", takmicar.Id);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri kreiranju takmičara");
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                    TempData["Greska"] = "Greška pri dodavanju takmičara";
                }
            }
            else
            {
                _logger.LogWarning("Neuspješna validacija forme za novog takmičara. Broj grešaka: {BrojGresaka}",
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

            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", takmicar.KlijentId);
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", takmicar.DisciplinaId);
            return View(takmicar);
        }

        // GET: Takmicari/Edit/5
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

            var takmicar = await _context.Takmicari.FindAsync(id);
            if (takmicar == null)
            {
                _logger.LogWarning("Edit: Takmičar nije pronađen. ID: {Id}", id);
                return NotFound();
            }

            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", takmicar.KlijentId);
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", takmicar.DisciplinaId);
            _logger.LogInformation("Prikaz forme za uređivanje takmičara ID: {Id}", id);
            return View(takmicar);
        }

        // POST: Takmicari/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KlijentId,DisciplinaId,Rezultat,Pozicija")] Takmicar takmicar)
        {
            if (id != takmicar.Id)
            {
                _logger.LogWarning("Edit: ID u putanji ({IdPut}) i modelu ({IdModel}) se ne podudaraju",
                    id, takmicar.Id);
                return NotFound();
            }

            _logger.LogInformation(
                "Ažuriranje takmičara ID: {Id} - korisnik: {Korisnik}. Podaci: {@Takmicar}",
                id, User.Identity.Name, takmicar);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(takmicar);
                    await _context.SaveChangesAsync();

                    TempData["Uspjeh"] = "Takmičar uspješno ažuriran";
                    _logger.LogInformation("Takmičar uspješno ažuriran ID: {Id}", id);

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TakmicarExists(takmicar.Id))
                    {
                        _logger.LogWarning("Edit: Takmičar nije pronađen nakon DbUpdateConcurrencyException. ID: {Id}", id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Greška pri ažuriranju takmičara ID: {Id}", id);
                        ModelState.AddModelError("", "Greška pri spremanju promjena. Pokušajte ponovno.");
                        TempData["Greska"] = "Greška pri ažuriranju takmičara";
                    }
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri ažuriranju takmičara ID: {Id}", id);
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                    TempData["Greska"] = "Greška pri ažuriranju takmičara";
                }
            }
            else
            {
                _logger.LogWarning("Neuspješna validacija forme za uređivanje. Broj grešaka: {BrojGresaka}",
                    ModelState.ErrorCount);
            }

            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", takmicar.KlijentId);
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", takmicar.DisciplinaId);
            return View(takmicar);
        }

        // GET: Takmicari/Delete/5
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

            var takmicar = await _context.Takmicari
                .Include(x => x.Klijent)
                .Include(x => x.Disciplina)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (takmicar == null)
            {
                _logger.LogWarning("Delete: Takmičar nije pronađen. ID: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Prikaz forme za brisanje takmičara ID: {Id}", id);
            return View(takmicar);
        }

        // POST: Takmicari/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Brisanje takmičara ID: {Id} - korisnik: {Korisnik}",
                id, User.Identity.Name);

            var takmicar = await _context.Takmicari.FindAsync(id);
            if (takmicar == null)
            {
                _logger.LogWarning("DeleteConfirmed: Takmičar nije pronađen. ID: {Id}", id);
                TempData["Greska"] = "Takmičar nije pronađen";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Takmicari.Remove(takmicar);
                await _context.SaveChangesAsync();

                TempData["Uspjeh"] = "Takmičar uspješno obrisan";
                _logger.LogInformation("Takmičar uspješno obrisan ID: {Id}", id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju takmičara ID: {Id}", id);
                TempData["Greska"] = "Greška pri brisanju takmičara";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TakmicarExists(int id)
        {
            return _context.Takmicari.Any(e => e.Id == id);
        }
    }
}