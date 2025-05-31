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
    [Authorize] // Zahtjeva autentifikaciju za sve akcije
    public class PersonalniTreninziController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PersonalniTreninziController> _logger;

        public PersonalniTreninziController(ApplicationDbContext context, ILogger<PersonalniTreninziController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: PersonalniTreninzi
        [HttpGet]
        [Route("")]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("----- GET: PersonalniTreninzi/Index ----- Korisnik: {Korisnik}", User.Identity.Name);

            var personalniTreninzi = await _context.PersonalniTreninzi
                .Include(p => p.Trener).ThenInclude(t => t.Zaposlenik)
                .Include(p => p.Klijent)
                .ToListAsync();

            return View(personalniTreninzi);
        }

        // GET: PersonalniTreninzi/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details: ID nije pronađen");
                return NotFound();
            }

            var personalniTrening = await _context.PersonalniTreninzi
                .Include(x => x.Trener).ThenInclude(t => t.Zaposlenik)
                .Include(x => x.Klijent)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (personalniTrening == null)
            {
                _logger.LogWarning("Details: Personalni trening s ID-om {Id} nije pronađen", id);
                return NotFound();
            }

            _logger.LogInformation("Details: Prikaz detalja za personalni trening ID {Id}", id);
            return View(personalniTrening);
        }

        // GET: PersonalniTreninzi/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public IActionResult Create()
        {
            _logger.LogInformation("Create: Prikaz forme za novi personalni trening - Korisnik: {Korisnik}", User.Identity.Name);

            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                "Id", "Name"
            );
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime");
            return View();
        }

        // POST: PersonalniTreninzi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Create([Bind("Naziv,Opis,TrenerId,KlijentId,Datum,Vrijeme,Napredak")] PersonalniTrening personalniTrening)
        {
            _logger.LogInformation("----- POST: PersonalniTreninzi/Create ----- Korisnik: {Korisnik}", User.Identity.Name);
            _logger.LogInformation("Parametri: Naziv={Naziv}, TrenerId={TrenerId}, KlijentId={KlijentId}, Datum={Datum}, Vrijeme={Vrijeme}",
                personalniTrening.Naziv, personalniTrening.TrenerId, personalniTrening.KlijentId,
                personalniTrening.Datum, personalniTrening.Vrijeme);

            if (ModelState.IsValid)
            {
                try
                {
                    // Provjera zauzetosti termina za trenera
                    bool terminZauzet = await _context.PersonalniTreninzi
                        .AnyAsync(t => t.TrenerId == personalniTrening.TrenerId &&
                                      t.Datum == personalniTrening.Datum &&
                                      t.Vrijeme == personalniTrening.Vrijeme);

                    if (terminZauzet)
                    {
                        ModelState.AddModelError("Vrijeme", "Trener je već zauzet u ovom terminu");
                        throw new InvalidOperationException("Termin trenera je zauzet");
                    }

                    // Provjera zauzetosti termina za klijenta
                    bool klijentZauzet = await _context.PersonalniTreninzi
                        .AnyAsync(t => t.KlijentId == personalniTrening.KlijentId &&
                                      t.Datum == personalniTrening.Datum &&
                                      t.Vrijeme == personalniTrening.Vrijeme);

                    if (klijentZauzet)
                    {
                        ModelState.AddModelError("Vrijeme", "Klijent je već zauzet u ovom terminu");
                        throw new InvalidOperationException("Termin klijenta je zauzet");
                    }

                    _context.Add(personalniTrening);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Kreiran novi personalni trening ID {Id}", personalniTrening.Id);
                    TempData["Uspjeh"] = "Personalni trening uspješno kreiran!";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri kreiranju personalnog treninga");
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning("Validacijska greška: {Poruka}", ex.Message);
                }
            }

            _logger.LogWarning("Neuspješna validacija: {BrojGrešaka} grešaka", ModelState.ErrorCount);
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                "Id", "Name", personalniTrening.TrenerId
            );
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", personalniTrening.KlijentId);
            return View(personalniTrening);
        }

        // GET: PersonalniTreninzi/Edit/5
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

            var personalniTrening = await _context.PersonalniTreninzi.FindAsync(id);
            if (personalniTrening == null)
            {
                _logger.LogWarning("Edit GET: Personalni trening s ID-om {Id} nije pronađen", id);
                return NotFound();
            }

            _logger.LogInformation("Edit GET: Uređivanje personalnog treninga ID {Id}", id);
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                "Id", "Name", personalniTrening.TrenerId
            );
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", personalniTrening.KlijentId);
            return View(personalniTrening);
        }

        // POST: PersonalniTreninzi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,TrenerId,KlijentId,Datum,Vrijeme,Napredak")] PersonalniTrening personalniTrening)
        {
            _logger.LogInformation("----- POST: PersonalniTreninzi/Edit/{id} ----- Korisnik: {Korisnik}", id, User.Identity.Name);
            _logger.LogInformation("Parametri: Id={Id}, Naziv={Naziv}, TrenerId={TrenerId}, KlijentId={KlijentId}, Datum={Datum}, Vrijeme={Vrijeme}",
                personalniTrening.Id, personalniTrening.Naziv, personalniTrening.TrenerId, personalniTrening.KlijentId,
                personalniTrening.Datum, personalniTrening.Vrijeme);

            if (id != personalniTrening.Id)
            {
                _logger.LogWarning("Edit POST: ID u rutu ({RutaId}) i modelu ({ModelId}) se ne podudaraju", id, personalniTrening.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Provjera zauzetosti termina za trenera (osim trenutnog treninga)
                    bool terminZauzet = await _context.PersonalniTreninzi
                        .AnyAsync(t => t.TrenerId == personalniTrening.TrenerId &&
                                      t.Datum == personalniTrening.Datum &&
                                      t.Vrijeme == personalniTrening.Vrijeme &&
                                      t.Id != personalniTrening.Id);

                    if (terminZauzet)
                    {
                        ModelState.AddModelError("Vrijeme", "Trener je već zauzet u ovom terminu");
                        throw new InvalidOperationException("Termin trenera je zauzet");
                    }

                    // Provjera zauzetosti termina za klijenta (osim trenutnog treninga)
                    bool klijentZauzet = await _context.PersonalniTreninzi
                        .AnyAsync(t => t.KlijentId == personalniTrening.KlijentId &&
                                      t.Datum == personalniTrening.Datum &&
                                      t.Vrijeme == personalniTrening.Vrijeme &&
                                      t.Id != personalniTrening.Id);

                    if (klijentZauzet)
                    {
                        ModelState.AddModelError("Vrijeme", "Klijent je već zauzet u ovom terminu");
                        throw new InvalidOperationException("Termin klijenta je zauzet");
                    }

                    _context.Update(personalniTrening);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Personalni trening ID {Id} uspješno ažuriran", id);
                    TempData["Uspjeh"] = "Personalni trening uspješno ažuriran!";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!PersonalniTreningExists(personalniTrening.Id))
                    {
                        _logger.LogWarning("Edit POST: Personalni trening s ID-om {Id} više ne postoji u bazi", id);
                        return NotFound();
                    }
                    _logger.LogError(ex, "Greška pri ažuriranju personalnog treninga ID {Id}", id);
                    TempData["Greska"] = "Greška pri ažuriranju. Podatak je promijenjen ili obrisan od strane drugog korisnika.";
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri ažuriranju personalnog treninga ID {Id}", id);
                    ModelState.AddModelError("", "Greška pri spremanju promjena. Pokušajte ponovno.");
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning("Validacijska greška: {Poruka}", ex.Message);
                }
            }

            _logger.LogWarning("Neuspješna validacija: {BrojGrešaka} grešaka", ModelState.ErrorCount);
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                "Id", "Name", personalniTrening.TrenerId
            );
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", personalniTrening.KlijentId);
            return View(personalniTrening);
        }

        // GET: PersonalniTreninzi/Delete/5
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

            var personalniTrening = await _context.PersonalniTreninzi
                .Include(x => x.Trener).ThenInclude(t => t.Zaposlenik)
                .Include(x => x.Klijent)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (personalniTrening == null)
            {
                _logger.LogWarning("Delete GET: Personalni trening s ID-om {Id} nije pronađen", id);
                return NotFound();
            }

            // Provjera statusa treninga
            bool prosaoTrening = personalniTrening.Datum < DateTime.Today ||
                                (personalniTrening.Datum == DateTime.Today &&
                                 personalniTrening.Vrijeme < DateTime.Now.TimeOfDay);

            ViewData["ProsaoTrening"] = prosaoTrening;

            _logger.LogInformation("Delete GET: Potvrda brisanja personalnog treninga ID {Id}", id);
            return View(personalniTrening);
        }

        // POST: PersonalniTreninzi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("----- POST: PersonalniTreninzi/Delete/{id} ----- Korisnik: {Korisnik}", id, User.Identity.Name);

            var personalniTrening = await _context.PersonalniTreninzi.FindAsync(id);
            if (personalniTrening == null)
            {
                _logger.LogWarning("DeleteConfirmed: Personalni trening s ID-om {Id} nije pronađen", id);
                TempData["Greska"] = "Personalni trening nije pronađen!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Provjera statusa treninga
                bool prosaoTrening = personalniTrening.Datum < DateTime.Today ||
                                    (personalniTrening.Datum == DateTime.Today &&
                                     personalniTrening.Vrijeme < DateTime.Now.TimeOfDay);

                if (prosaoTrening)
                {
                    _logger.LogWarning("Brisanje onemogućeno: Personalni trening ID {Id} je već održan", id);
                    TempData["Greska"] = "Ne možete obrisati već održan trening!";
                    return RedirectToAction(nameof(Delete), new { id });
                }

                _context.PersonalniTreninzi.Remove(personalniTrening);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Personalni trening ID {Id} uspješno obrisan", id);
                TempData["Uspjeh"] = "Personalni trening uspješno obrisan!";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju personalnog treninga ID {Id}", id);
                TempData["Greska"] = "Greška pri brisanju treninga. Pokušajte ponovno ili kontaktirajte administratora.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PersonalniTreningExists(int id)
        {
            return _context.PersonalniTreninzi.Any(e => e.Id == id);
        }
    }
}