using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;
using Microsoft.AspNetCore.Authorization;

namespace InformacioniSistemTeretane.Controllers
{
    public class LokacijeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LokacijeController> _logger;

        public LokacijeController(ApplicationDbContext context, ILogger<LokacijeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Lokacije/Index
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("----- GET: Lokacije/Index ----- Korisnik: {Korisnik}", User.Identity.Name);
            var lokacije = await _context.Lokacije.ToListAsync();
            return View(lokacije);
        }

        // GET: Lokacije/Details/{id}
        [HttpGet]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details: ID nije pronađen");
                return NotFound();
            }

            var lokacija = await _context.Lokacije
                .Include(l => l.Sale)
                .Include(l => l.Igraonice)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lokacija == null)
            {
                _logger.LogWarning("Details: Lokacija s ID-om {Id} nije pronađena", id);
                return NotFound();
            }

            _logger.LogInformation("Details: Prikaz detalja za lokaciju ID {Id}", id);
            return View(lokacija);
        }

        // GET: Lokacije/Create
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            _logger.LogInformation("Create: Prikaz forme za novu lokaciju - Korisnik: {Korisnik}", User.Identity.Name);
            return View();
        }

        // POST: Lokacije/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Create([Bind("Naziv,Adresa,KontaktTelefon,Email")] Lokacija lokacija)
        {
            _logger.LogInformation("----- POST: Lokacije/Create ----- Korisnik: {Korisnik}", User.Identity.Name);
            _logger.LogInformation("Parametri: Naziv={Naziv}, Adresa={Adresa}, KontaktTelefon={KontaktTelefon}, Email={Email}",
                lokacija.Naziv, lokacija.Adresa, lokacija.KontaktTelefon, lokacija.Email);

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _context.Lokacije.AnyAsync(l => l.Naziv == lokacija.Naziv))
                    {
                        ModelState.AddModelError("Naziv", "Lokacija s ovim nazivom već postoji");
                        throw new InvalidOperationException("Naziv lokacije nije jedinstven");
                    }

                    _context.Add(lokacija);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Kreirana nova lokacija ID {Id}", lokacija.Id);
                    TempData["Uspjeh"] = "Lokacija uspješno kreirana!";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning("Validacijska greška: {Poruka}", ex.Message);
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri kreiranju lokacije");
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                }
            }

            _logger.LogWarning("Neuspješna validacija: {BrojGrešaka} grešaka", ModelState.ErrorCount);
            return View(lokacija);
        }

        // GET: Lokacije/Edit/{id}
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Edit GET: ID nije pronađen");
                return NotFound();
            }

            var lokacija = await _context.Lokacije.FindAsync(id);
            if (lokacija == null)
            {
                _logger.LogWarning("Edit GET: Lokacija s ID-om {Id} nije pronađena", id);
                return NotFound();
            }

            _logger.LogInformation("Edit GET: Uređivanje lokacije ID {Id}", id);
            return View(lokacija);
        }

        // POST: Lokacije/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Adresa,KontaktTelefon,Email")] Lokacija lokacija)
        {
            _logger.LogInformation("----- POST: Lokacije/Edit/{id} ----- Korisnik: {Korisnik}", id, User.Identity.Name);
            _logger.LogInformation("Parametri: Id={Id}, Naziv={Naziv}, Adresa={Adresa}, KontaktTelefon={KontaktTelefon}, Email={Email}",
                lokacija.Id, lokacija.Naziv, lokacija.Adresa, lokacija.KontaktTelefon, lokacija.Email);

            if (id != lokacija.Id)
            {
                _logger.LogWarning("Edit POST: ID u rutu ({RutaId}) i modelu ({ModelId}) se ne podudaraju", id, lokacija.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _context.Lokacije.AnyAsync(l => l.Naziv == lokacija.Naziv && l.Id != lokacija.Id))
                    {
                        ModelState.AddModelError("Naziv", "Lokacija s ovim nazivom već postoji");
                        throw new InvalidOperationException("Naziv lokacije nije jedinstven");
                    }

                    _context.Update(lokacija);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Lokacija ID {Id} uspješno ažurirana", id);
                    TempData["Uspjeh"] = "Lokacija uspješno ažurirana!";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning("Validacijska greška: {Poruka}", ex.Message);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!LokacijaExists(lokacija.Id))
                    {
                        _logger.LogWarning("Edit POST: Lokacija s ID-om {Id} više ne postoji u bazi", id);
                        return NotFound();
                    }
                    _logger.LogError(ex, "Greška pri ažuriranju lokacije ID {Id}", id);
                    TempData["Greska"] = "Greška pri ažuriranju. Podatak je promijenjen ili obrisan od strane drugog korisnika.";
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri ažuriranju lokacije ID {Id}", id);
                    ModelState.AddModelError("", "Greška pri spremanju promjena. Pokušajte ponovno.");
                }
            }

            _logger.LogWarning("Neuspješna validacija: {BrojGrešaka} grešaka", ModelState.ErrorCount);
            return View(lokacija);
        }

        // GET: Lokacije/Delete/{id}
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Delete GET: ID nije pronađen");
                return NotFound();
            }

            var lokacija = await _context.Lokacije
                .Include(l => l.Sale)
                .Include(l => l.Igraonice)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lokacija == null)
            {
                _logger.LogWarning("Delete GET: Lokacija s ID-om {Id} nije pronađena", id);
                return NotFound();
            }

            bool imaSale = lokacija.Sale?.Any() ?? false;
            bool imaIgraonice = lokacija.Igraonice?.Any() ?? false;
            ViewData["ImaZavisnosti"] = imaSale || imaIgraonice;
            ViewData["ImaSale"] = imaSale;
            ViewData["ImaIgraonice"] = imaIgraonice;

            _logger.LogInformation("Delete GET: Potvrda brisanja lokacije ID {Id}", id);
            return View(lokacija);
        }

        // POST: Lokacije/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("----- POST: Lokacije/Delete/{id} ----- Korisnik: {Korisnik}", id, User.Identity.Name);

            var lokacija = await _context.Lokacije
                .Include(l => l.Sale)
                .Include(l => l.Igraonice)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lokacija == null)
            {
                _logger.LogWarning("DeleteConfirmed: Lokacija s ID-om {Id} nije pronađena", id);
                TempData["Greska"] = "Lokacija nije pronađena!";
                return RedirectToAction(nameof(Index));
            }

            if (lokacija.Sale?.Any() == true || lokacija.Igraonice?.Any() == true)
            {
                _logger.LogWarning("Brisanje onemogućeno: Lokacija ID {Id} ima vezane sale ili igraonice", id);
                TempData["Greska"] = "Ne možete obrisati lokaciju jer postoje vezani entiteti (sale ili igraonice)!";
                return RedirectToAction(nameof(Delete), new { id });
            }

            try
            {
                _context.Lokacije.Remove(lokacija);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Lokacija ID {Id} uspješno obrisana", id);
                TempData["Uspjeh"] = "Lokacija uspješno obrisana!";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju lokacije ID {Id}", id);
                TempData["Greska"] = "Greška pri brisanju lokacije. Pokušajte ponovno ili kontaktirajte administratora.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LokacijaExists(int id)
        {
            return _context.Lokacije.Any(e => e.Id == id);
        }
    }
}
