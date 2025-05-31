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
    [Authorize]
    public class KlijentiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<KlijentiController> _logger;

        public KlijentiController(ApplicationDbContext context, ILogger<KlijentiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Klijenti/Index
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("----- GET: Klijenti/Index ----- Korisnik: {Korisnik}", User.Identity.Name);

            var klijenti = _context.Klijenti.Include(k => k.User);
            return View(await klijenti.ToListAsync());
        }

        // GET: Klijenti/Details/{id}
        [HttpGet]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details: ID nije pronađen");
                return NotFound();
            }

            var klijent = await _context.Klijenti
                .Include(k => k.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (klijent == null)
            {
                _logger.LogWarning("Details: Klijent s ID-om {Id} nije pronađen", id);
                return NotFound();
            }

            _logger.LogInformation("Details: Prikaz detalja za klijenta ID {Id}", id);
            return View(klijent);
        }

        // GET: Klijenti/Create
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            _logger.LogInformation("Create: Prikaz forme za novog klijenta - Korisnik: {Korisnik}", User.Identity.Name);

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Klijenti/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Create([Bind("Ime,Prezime,DatumRodjenja,UserId")] Klijent klijent)
        {
            _logger.LogInformation("----- POST: Klijenti/Create ----- Korisnik: {Korisnik}", User.Identity.Name);
            _logger.LogInformation("Parametri: Ime={Ime}, Prezime={Prezime}, DatumRodjenja={DatumRodjenja}, UserId={UserId}",
                klijent.Ime, klijent.Prezime, klijent.DatumRodjenja, klijent.UserId);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(klijent);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Kreiran novi klijent ID {Id}", klijent.Id);
                    TempData["Uspjeh"] = "Klijent uspješno kreiran!";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri kreiranju klijenta");
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                }
            }

            _logger.LogWarning("Neuspješna validacija: {BrojGrešaka} grešaka", ModelState.ErrorCount);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", klijent.UserId);
            return View(klijent);
        }

        // GET: Klijenti/Edit/{id}
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

            var klijent = await _context.Klijenti.FindAsync(id);
            if (klijent == null)
            {
                _logger.LogWarning("Edit GET: Klijent s ID-om {Id} nije pronađen", id);
                return NotFound();
            }

            _logger.LogInformation("Edit GET: Uređivanje klijenta ID {Id}", id);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", klijent.UserId);
            return View(klijent);
        }

        // POST: Klijenti/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Prezime,DatumRodjenja,UserId")] Klijent klijent)
        {
            _logger.LogInformation("----- POST: Klijenti/Edit/{id} ----- Korisnik: {Korisnik}", id, User.Identity.Name);
            _logger.LogInformation("Parametri: Id={Id}, Ime={Ime}, Prezime={Prezime}, DatumRodjenja={DatumRodjenja}, UserId={UserId}",
                klijent.Id, klijent.Ime, klijent.Prezime, klijent.DatumRodjenja, klijent.UserId);

            if (id != klijent.Id)
            {
                _logger.LogWarning("Edit POST: ID u rutu ({RutaId}) i modelu ({ModelId}) se ne podudaraju", id, klijent.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(klijent);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Klijent ID {Id} uspješno ažuriran", id);
                    TempData["Uspjeh"] = "Klijent uspješno ažuriran!";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!KlijentExists(klijent.Id))
                    {
                        _logger.LogWarning("Edit POST: Klijent s ID-om {Id} više ne postoji u bazi", id);
                        return NotFound();
                    }
                    _logger.LogError(ex, "Greška pri ažuriranju klijenta ID {Id}", id);
                    TempData["Greska"] = "Greška pri ažuriranju. Podatak je promijenjen ili obrisan od strane drugog korisnika.";
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri ažuriranju klijenta ID {Id}", id);
                    ModelState.AddModelError("", "Greška pri spremanju promjena. Pokušajte ponovno.");
                }
            }

            _logger.LogWarning("Neuspješna validacija: {BrojGrešaka} grešaka", ModelState.ErrorCount);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", klijent.UserId);
            return View(klijent);
        }

        // GET: Klijenti/Delete/{id}
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

            var klijent = await _context.Klijenti
                .Include(k => k.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (klijent == null)
            {
                _logger.LogWarning("Delete GET: Klijent s ID-om {Id} nije pronađen", id);
                return NotFound();
            }

            _logger.LogInformation("Delete GET: Potvrda brisanja klijenta ID {Id}", id);
            return View(klijent);
        }

        // POST: Klijenti/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("----- POST: Klijenti/Delete/{id} ----- Korisnik: {Korisnik}", id, User.Identity.Name);

            var klijent = await _context.Klijenti.FindAsync(id);
            if (klijent == null)
            {
                _logger.LogWarning("DeleteConfirmed: Klijent s ID-om {Id} nije pronađen", id);
                TempData["Greska"] = "Klijent nije pronađen!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Klijenti.Remove(klijent);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Klijent ID {Id} uspješno obrisan", id);
                TempData["Uspjeh"] = "Klijent uspješno obrisan!";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju klijenta ID {Id}", id);
                TempData["Greska"] = "Greška pri brisanju klijenta. Pokušajte ponovno ili kontaktirajte administratora.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool KlijentExists(int id)
        {
            return _context.Klijenti.Any(e => e.Id == id);
        }
    }
}