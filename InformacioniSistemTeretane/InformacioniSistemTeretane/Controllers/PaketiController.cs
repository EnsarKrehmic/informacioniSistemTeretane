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
    [Authorize]
    public class PaketiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaketiController> _logger;

        public PaketiController(ApplicationDbContext context, ILogger<PaketiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Paketi/Index
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("----- GET: Paketi/Index ----- Korisnik: {Korisnik}", User.Identity.Name);
            var paketi = await _context.Paketi.ToListAsync();
            return View(paketi);
        }

        // GET: Paketi/Details/{id}
        [HttpGet]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details: ID nije pronađen");
                return NotFound();
            }

            var paket = await _context.Paketi
                .Include(p => p.Uplate)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (paket == null)
            {
                _logger.LogWarning("Details: Paket s ID-om {Id} nije pronađen", id);
                return NotFound();
            }

            _logger.LogInformation("Details: Prikaz detalja za paket ID {Id}", id);
            return View(paket);
        }

        // GET: Paketi/Create
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            _logger.LogInformation("Create: Prikaz forme za novi paket - Korisnik: {Korisnik}", User.Identity.Name);
            return View();
        }

        // POST: Paketi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Create([Bind("Naziv,Opis,Cijena,TrajanjeDana")] Paket paket)
        {
            _logger.LogInformation("----- POST: Paketi/Create ----- Korisnik: {Korisnik}", User.Identity.Name);
            _logger.LogInformation("Parametri: Naziv={Naziv}, Opis={Opis}, Cijena={Cijena}, TrajanjeDana={TrajanjeDana}",
                paket.Naziv, paket.Opis, paket.Cijena, paket.TrajanjeDana);

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _context.Paketi.AnyAsync(p => p.Naziv == paket.Naziv))
                    {
                        ModelState.AddModelError("Naziv", "Paket s ovim nazivom već postoji");
                        throw new InvalidOperationException("Naziv paketa nije jedinstven");
                    }
                    if (paket.Cijena <= 0)
                    {
                        ModelState.AddModelError("Cijena", "Cijena mora biti veća od 0");
                        throw new InvalidOperationException("Nevaljana cijena paketa");
                    }
                    if (paket.TrajanjeDana <= 0)
                    {
                        ModelState.AddModelError("TrajanjeDana", "Trajanje mora biti veće od 0 dana");
                        throw new InvalidOperationException("Nevaljano trajanje paketa");
                    }

                    _context.Add(paket);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Kreiran novi paket ID {Id}", paket.Id);
                    TempData["Uspjeh"] = "Paket uspješno kreiran!";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning("Validacijska greška: {Poruka}", ex.Message);
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri kreiranju paketa");
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                }
            }

            _logger.LogWarning("Neuspješna validacija: {BrojGrešaka} grešaka", ModelState.ErrorCount);
            return View(paket);
        }

        // GET: Paketi/Edit/{id}
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

            var paket = await _context.Paketi.FindAsync(id);
            if (paket == null)
            {
                _logger.LogWarning("Edit GET: Paket s ID-om {Id} nije pronađen", id);
                return NotFound();
            }

            _logger.LogInformation("Edit GET: Uređivanje paketa ID {Id}", id);
            return View(paket);
        }

        // POST: Paketi/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,Cijena,TrajanjeDana")] Paket paket)
        {
            _logger.LogInformation("----- POST: Paketi/Edit/{id} ----- Korisnik: {Korisnik}", id, User.Identity.Name);
            _logger.LogInformation("Parametri: Id={Id}, Naziv={Naziv}, Opis={Opis}, Cijena={Cijena}, TrajanjeDana={TrajanjeDana}",
                paket.Id, paket.Naziv, paket.Opis, paket.Cijena, paket.TrajanjeDana);

            if (id != paket.Id)
            {
                _logger.LogWarning("Edit POST: ID u rutu ({RutaId}) i modelu ({ModelId}) se ne podudaraju", id, paket.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _context.Paketi.AnyAsync(p => p.Naziv == paket.Naziv && p.Id != paket.Id))
                    {
                        ModelState.AddModelError("Naziv", "Paket s ovim nazivom već postoji");
                        throw new InvalidOperationException("Naziv paketa nije jedinstven");
                    }
                    if (paket.Cijena <= 0)
                    {
                        ModelState.AddModelError("Cijena", "Cijena mora biti veća od 0");
                        throw new InvalidOperationException("Nevaljana cijena paketa");
                    }
                    if (paket.TrajanjeDana <= 0)
                    {
                        ModelState.AddModelError("TrajanjeDana", "Trajanje mora biti veće od 0 dana");
                        throw new InvalidOperationException("Nevaljano trajanje paketa");
                    }

                    _context.Update(paket);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Paket ID {Id} uspješno ažuriran", id);
                    TempData["Uspjeh"] = "Paket uspješno ažuriran!";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning("Validacijska greška: {Poruka}", ex.Message);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!PaketiExists(paket.Id))
                    {
                        _logger.LogWarning("Edit POST: Paket s ID-om {Id} više ne postoji u bazi", id);
                        return NotFound();
                    }
                    _logger.LogError(ex, "Greška pri ažuriranju paketa ID {Id}", id);
                    TempData["Greska"] = "Greška pri ažuriranju. Podatak je promijenjen ili obrisan od strane drugog korisnika.";
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri ažuriranju paketa ID {Id}", id);
                    ModelState.AddModelError("", "Greška pri spremanju promjena. Pokušajte ponovno.");
                }
            }

            _logger.LogWarning("Neuspješna validacija: {BrojGrešaka} grešaka", ModelState.ErrorCount);
            return View(paket);
        }

        // GET: Paketi/Delete/{id}
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

            var paket = await _context.Paketi
                .Include(p => p.Uplate)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (paket == null)
            {
                _logger.LogWarning("Delete GET: Paket s ID-om {Id} nije pronađen", id);
                return NotFound();
            }

            bool imaUplata = paket.Uplate?.Any() ?? false;
            ViewData["ImaUplata"] = imaUplata;
            ViewData["BrojUplata"] = paket.Uplate?.Count ?? 0;

            _logger.LogInformation("Delete GET: Potvrda brisanja paketa ID {Id}", id);
            return View(paket);
        }

        // POST: Paketi/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("----- POST: Paketi/Delete/{id} ----- Korisnik: {Korisnik}", id, User.Identity.Name);

            var paket = await _context.Paketi
                .Include(p => p.Uplate)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (paket == null)
            {
                _logger.LogWarning("DeleteConfirmed: Paket s ID-om {Id} nije pronađen", id);
                TempData["Greska"] = "Paket nije pronađen!";
                return RedirectToAction(nameof(Index));
            }

            if (paket.Uplate?.Any() == true)
            {
                _logger.LogWarning("Brisanje onemogućeno: Paket ID {Id} ima vezane uplate", id);
                TempData["Greska"] = "Ne možete obrisati paket jer postoje vezane uplate!";
                return RedirectToAction(nameof(Delete), new { id });
            }

            try
            {
                _context.Paketi.Remove(paket);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Paket ID {Id} uspješno obrisan", id);
                TempData["Uspjeh"] = "Paket uspješno obrisan!";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju paketa ID {Id}", id);
                TempData["Greska"] = "Greška pri brisanju paketa. Pokušajte ponovno ili kontaktirajte administratora.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PaketiExists(int id)
        {
            return _context.Paketi.Any(e => e.Id == id);
        }
    }
}
