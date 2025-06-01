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
    public class PrijavljeniGrupniController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PrijavljeniGrupniController> _logger;

        public PrijavljeniGrupniController(ApplicationDbContext context, ILogger<PrijavljeniGrupniController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: PrijavljeniGrupni
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("----- GET: PrijavljeniGrupni/Index ----- Korisnik: {Korisnik}", User.Identity.Name);

            var prijave = await _context.PrijavljeniGrupni
                .Include(p => p.GrupniTrening)
                .Include(p => p.Klijent)
                .ToListAsync();

            return View(prijave);
        }

        // GET: PrijavljeniGrupni/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details: ID nije pronađen");
                return NotFound();
            }

            var prijava = await _context.PrijavljeniGrupni
                .Include(x => x.GrupniTrening)
                .Include(x => x.Klijent)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prijava == null)
            {
                _logger.LogWarning("Details: Prijava s ID-om {Id} nije pronađena", id);
                return NotFound();
            }

            _logger.LogInformation("Details: Prikaz detalja za prijavu ID {Id}", id);
            return View(prijava);
        }

        // GET: PrijavljeniGrupni/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public IActionResult Create()
        {
            _logger.LogInformation("Create: Prikaz forme za novu prijavu - Korisnik: {Korisnik}", User.Identity.Name);

            ViewBag.GrupniTreningId = new SelectList(_context.GrupniTreninzi, "Id", "Naziv");
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime");
            return View();
        }

        // POST: PrijavljeniGrupni/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Create([Bind("GrupniTreningId,KlijentId,Prisutan,VrijemeDolaska")] PrijavljeniGrupni prijava)
        {
            _logger.LogInformation("----- POST: PrijavljeniGrupni/Create ----- Korisnik: {Korisnik}", User.Identity.Name);
            _logger.LogInformation("Parametri: GrupniTreningId={GrupniTreningId}, KlijentId={KlijentId}, Prisutan={Prisutan}, VrijemeDolaska={VrijemeDolaska}",
                prijava.GrupniTreningId, prijava.KlijentId, prijava.Prisutan, prijava.VrijemeDolaska);

            if (ModelState.IsValid)
            {
                try
                {
                    // Provjera kapaciteta grupnog treninga
                    var grupniTrening = await _context.GrupniTreninzi
                        .Include(g => g.Prisustva)
                        .FirstOrDefaultAsync(g => g.Id == prijava.GrupniTreningId);

                    if (grupniTrening == null)
                    {
                        ModelState.AddModelError("GrupniTreningId", "Odabrani grupni trening ne postoji");
                        throw new InvalidOperationException("Grupni trening nije pronađen");
                    }

                    // Provjera da li je klijent već prijavljen
                    bool vecPrijavljen = await _context.PrijavljeniGrupni
                        .AnyAsync(p => p.GrupniTreningId == prijava.GrupniTreningId &&
                                      p.KlijentId == prijava.KlijentId);

                    if (vecPrijavljen)
                    {
                        ModelState.AddModelError("KlijentId", "Klijent je već prijavljen na ovaj trening");
                        throw new InvalidOperationException("Klijent je već prijavljen");
                    }

                    _context.Add(prijava);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Kreirana nova prijava ID {Id}", prijava.Id);
                    TempData["Uspjeh"] = "Prijava uspješno kreirana!";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri kreiranju prijave");
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning("Validacijska greška: {Poruka}", ex.Message);
                }
            }

            _logger.LogWarning("Neuspješna validacija: {BrojGrešaka} grešaka", ModelState.ErrorCount);
            ViewBag.GrupniTreningId = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", prijava.GrupniTreningId);
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", prijava.KlijentId);
            return View(prijava);
        }

        // GET: PrijavljeniGrupni/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit GET: ID nije pronađen");
                return NotFound();
            }

            var prijava = await _context.PrijavljeniGrupni.FindAsync(id);
            if (prijava == null)
            {
                _logger.LogWarning("Edit GET: Prijava s ID-om {Id} nije pronađena", id);
                return NotFound();
            }

            _logger.LogInformation("Edit GET: Uređivanje prijave ID {Id}", id);
            ViewBag.GrupniTreningId = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", prijava.GrupniTreningId);
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", prijava.KlijentId);
            return View(prijava);
        }

        // POST: PrijavljeniGrupni/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GrupniTreningId,KlijentId,Prisutan,VrijemeDolaska")] PrijavljeniGrupni prijava)
        {
            _logger.LogInformation("----- POST: PrijavljeniGrupni/Edit/{id} ----- Korisnik: {Korisnik}", id, User.Identity.Name);
            _logger.LogInformation("Parametri: Id={Id}, GrupniTreningId={GrupniTreningId}, KlijentId={KlijentId}, Prisutan={Prisutan}, VrijemeDolaska={VrijemeDolaska}",
                prijava.Id, prijava.GrupniTreningId, prijava.KlijentId, prijava.Prisutan, prijava.VrijemeDolaska);

            if (id != prijava.Id)
            {
                _logger.LogWarning("Edit POST: ID u rutu ({RutaId}) i modelu ({ModelId}) se ne podudaraju", id, prijava.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Provjera da li je klijent već prijavljen na drugu prijavu za isti trening
                    bool vecPrijavljen = await _context.PrijavljeniGrupni
                        .AnyAsync(p => p.GrupniTreningId == prijava.GrupniTreningId &&
                                      p.KlijentId == prijava.KlijentId &&
                                      p.Id != prijava.Id);

                    if (vecPrijavljen)
                    {
                        ModelState.AddModelError("KlijentId", "Klijent je već prijavljen na ovaj trening");
                        throw new InvalidOperationException("Klijent je već prijavljen");
                    }

                    _context.Update(prijava);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Prijava ID {Id} uspješno ažurirana", id);
                    TempData["Uspjeh"] = "Prijava uspješno ažurirana!";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!PrijavaExists(prijava.Id))
                    {
                        _logger.LogWarning("Edit POST: Prijava s ID-om {Id} više ne postoji u bazi", id);
                        return NotFound();
                    }
                    _logger.LogError(ex, "Greška pri ažuriranju prijave ID {Id}", id);
                    TempData["Greska"] = "Greška pri ažuriranju. Podatak je promijenjen ili obrisan od strane drugog korisnika.";
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri ažuriranju prijave ID {Id}", id);
                    ModelState.AddModelError("", "Greška pri spremanju promjena. Pokušajte ponovno.");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning("Validacijska greška: {Poruka}", ex.Message);
                }
            }

            _logger.LogWarning("Neuspješna validacija: {BrojGrešaka} grešaka", ModelState.ErrorCount);
            ViewBag.GrupniTreningId = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", prijava.GrupniTreningId);
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", prijava.KlijentId);
            return View(prijava);
        }

        // GET: PrijavljeniGrupni/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete GET: ID nije pronađen");
                return NotFound();
            }

            var prijava = await _context.PrijavljeniGrupni
                .Include(x => x.GrupniTrening)
                .Include(x => x.Klijent)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prijava == null)
            {
                _logger.LogWarning("Delete GET: Prijava s ID-om {Id} nije pronađena", id);
                return NotFound();
            }

            // Provjera da li je trening već počeo ili završio
            var grupniTrening = await _context.GrupniTreninzi.FindAsync(prijava.GrupniTreningId);
            bool treningZavrsen = grupniTrening != null &&
                                 (grupniTrening.Datum < DateTime.Today ||
                                  (grupniTrening.Datum == DateTime.Today &&
                                   grupniTrening.Vrijeme < DateTime.Now.TimeOfDay));

            ViewData["TreningZavrsen"] = treningZavrsen;

            _logger.LogInformation("Delete GET: Potvrda brisanja prijave ID {Id}", id);
            return View(prijava);
        }

        // POST: PrijavljeniGrupni/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("----- POST: PrijavljeniGrupni/Delete/{id} ----- Korisnik: {Korisnik}", id, User.Identity.Name);

            var prijava = await _context.PrijavljeniGrupni
                .Include(p => p.GrupniTrening)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prijava == null)
            {
                _logger.LogWarning("DeleteConfirmed: Prijava s ID-om {Id} nije pronađena", id);
                TempData["Greska"] = "Prijava nije pronađena!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Provjera da li je trening već počeo ili završio
                bool treningZavrsen = prijava.GrupniTrening != null &&
                                     (prijava.GrupniTrening.Datum < DateTime.Today ||
                                      (prijava.GrupniTrening.Datum == DateTime.Today &&
                                       prijava.GrupniTrening.Vrijeme < DateTime.Now.TimeOfDay));

                if (treningZavrsen)
                {
                    _logger.LogWarning("Brisanje onemogućeno: Trening ID {TreningId} je već održan", prijava.GrupniTreningId);
                    TempData["Greska"] = "Ne možete obrisati prijavu za već održan trening!";
                    return RedirectToAction(nameof(Delete), new { id });
                }

                _context.PrijavljeniGrupni.Remove(prijava);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Prijava ID {Id} uspješno obrisana", id);
                TempData["Uspjeh"] = "Prijava uspješno obrisana!";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju prijave ID {Id}", id);
                TempData["Greska"] = "Greška pri brisanju prijave. Pokušajte ponovno ili kontaktirajte administratora.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PrijavaExists(int id)
        {
            return _context.PrijavljeniGrupni.Any(e => e.Id == id);
        }
    }
}