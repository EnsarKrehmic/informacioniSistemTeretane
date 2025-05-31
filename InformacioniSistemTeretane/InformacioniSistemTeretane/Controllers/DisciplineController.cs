using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace InformacioniSistemTeretane.Controllers
{
    [Authorize]
    public class DisciplineController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DisciplineController> _logger;

        public DisciplineController(ApplicationDbContext context, ILogger<DisciplineController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Discipline/Index
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Korisnik {UserName} pregleda listu disciplina", User.Identity.Name);
            var discipline = _context.Discipline.Include(d => d.Takmicenje);
            return View(await discipline.ToListAsync());
        }

        // GET: Discipline/Details/{id}
        [HttpGet]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj pristupa detaljima discipline bez ID-a od strane {UserName}", User.Identity.Name);
                return NotFound();
            }

            var disciplina = await _context.Discipline
                .Include(d => d.Takmicenje)
                .Include(d => d.Takmicari)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (disciplina == null)
            {
                _logger.LogWarning("Disciplina sa ID-om {DisciplinaId} nije pronađena (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} pregleda detalje discipline: {DisciplinaNaziv}", User.Identity.Name, disciplina.Naziv);
            return View(disciplina);
        }

        // GET: Discipline/Create
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            _logger.LogInformation("Korisnik {UserName} pokreće kreiranje nove discipline", User.Identity.Name);
            ViewData["TakmicenjeId"] = new SelectList(_context.Takmicenja, "Id", "Naziv");
            return View();
        }

        // POST: Discipline/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Create([Bind("TakmicenjeId,Naziv,Opis,MaxUcesnika")] Disciplina d)
        {
            _logger.LogInformation("Korisnik {UserName} kreira novu disciplinu", User.Identity.Name);
            _logger.LogDebug("Parametri: TakmicenjeId={TakmicenjeId}, Naziv={Naziv}, Opis={Opis}, MaxUcesnika={MaxUcesnika}",
                d.TakmicenjeId, d.Naziv, d.Opis, d.MaxUcesnika);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Neuspješna validacija za novu disciplinu (korisnik: {UserName})", User.Identity.Name);
                ViewData["TakmicenjeId"] = new SelectList(_context.Takmicenja, "Id", "Naziv", d.TakmicenjeId);
                return View(d);
            }

            try
            {
                _context.Add(d);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Disciplina {Naziv} uspješno kreirana (ID: {DisciplinaId}) od strane {UserName}",
                    d.Naziv, d.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Disciplina '{d.Naziv}' je uspješno kreirana!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri čuvanju discipline u bazu (korisnik: {UserName})", User.Identity.Name);
                ModelState.AddModelError("", "Došlo je do greške pri čuvanju podataka. Pokušajte ponovo.");
                ViewData["TakmicenjeId"] = new SelectList(_context.Takmicenja, "Id", "Naziv", d.TakmicenjeId);
                return View(d);
            }
        }

        // GET: Discipline/Edit/{id}
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj uređivanja discipline bez ID-a od strane {UserName}", User.Identity.Name);
                return NotFound();
            }

            var d = await _context.Discipline.FindAsync(id);
            if (d == null)
            {
                _logger.LogWarning("Disciplina sa ID-om {DisciplinaId} nije pronađena za uređivanje (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} uređuje disciplinu: {DisciplinaNaziv} (ID: {DisciplinaId})",
                User.Identity.Name, d.Naziv, d.Id);

            ViewData["TakmicenjeId"] = new SelectList(_context.Takmicenja, "Id", "Naziv", d.TakmicenjeId);
            return View(d);
        }

        // POST: Discipline/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TakmicenjeId,Naziv,Opis,MaxUcesnika")] Disciplina d)
        {
            _logger.LogInformation("Korisnik {UserName} ažurira disciplinu ID: {DisciplinaId}", User.Identity.Name, id);
            _logger.LogDebug("Parametri: TakmicenjeId={TakmicenjeId}, Naziv={Naziv}, Opis={Opis}, MaxUcesnika={MaxUcesnika}",
                d.TakmicenjeId, d.Naziv, d.Opis, d.MaxUcesnika);

            if (id != d.Id)
            {
                _logger.LogWarning("ID u putanji ({PutanjaId}) i ID discipline ({DisciplinaId}) se ne podudaraju (korisnik: {UserName})", id, d.Id, User.Identity.Name);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Neuspješna validacija za ažuriranje discipline (korisnik: {UserName})", User.Identity.Name);
                ViewData["TakmicenjeId"] = new SelectList(_context.Takmicenja, "Id", "Naziv", d.TakmicenjeId);
                return View(d);
            }

            try
            {
                _context.Update(d);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Disciplina {Naziv} (ID: {DisciplinaId}) uspješno ažurirana od strane {UserName}", d.Naziv, d.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Disciplina '{d.Naziv}' je uspješno ažurirana!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_context.Discipline.Any(e => e.Id == d.Id))
                {
                    _logger.LogWarning("Disciplina sa ID-om {DisciplinaId} nije pronađena (korisnik: {UserName})", d.Id, User.Identity.Name);
                    return NotFound();
                }

                _logger.LogError(ex, "Greška pri ažuriranju discipline (ID: {DisciplinaId})", d.Id);
                ModelState.AddModelError("", "Došlo je do greške pri čuvanju izmjena. Pokušajte ponovo.");
                ViewData["TakmicenjeId"] = new SelectList(_context.Takmicenja, "Id", "Naziv", d.TakmicenjeId);
                return View(d);
            }
        }

        // GET: Discipline/Delete/{id}
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj brisanja discipline bez ID-a od strane {UserName}", User.Identity.Name);
                return NotFound();
            }

            var d = await _context.Discipline
                .Include(d => d.Takmicenje)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (d == null)
            {
                _logger.LogWarning("Disciplina sa ID-om {DisciplinaId} nije pronađena za brisanje (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} pokreće brisanje discipline: {DisciplinaNaziv} (ID: {DisciplinaId})", User.Identity.Name, d.Naziv, d.Id);

            return View(d);
        }

        // POST: Discipline/DeleteConfirmed/{id}
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var d = await _context.Discipline
                .Include(d => d.Takmicari)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (d == null)
            {
                _logger.LogWarning("Disciplina sa ID-om {DisciplinaId} nije pronađena za brisanje (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            if (d.Takmicari != null && d.Takmicari.Any())
            {
                _logger.LogWarning("Pokušaj brisanja discipline {DisciplinaNaziv} koja ima takmičare (korisnik: {UserName})", d.Naziv, User.Identity.Name);

                TempData["Greska"] = $"Ne možete obrisati disciplinu '{d.Naziv}' jer ima pridružene takmičare!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Discipline.Remove(d);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Disciplina {Naziv} (ID: {DisciplinaId}) uspješno obrisana od strane {UserName}", d.Naziv, d.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Disciplina '{d.Naziv}' je uspješno obrisana!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju discipline ID: {DisciplinaId} (korisnik: {UserName})", id, User.Identity.Name);

                TempData["Greska"] = $"Došlo je do greške pri brisanju discipline '{d.Naziv}'!";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
