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
    public class LicenceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LicenceController> _logger;

        public LicenceController(ApplicationDbContext context, ILogger<LicenceController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Licence/Index
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("----- GET: Licence/Index ----- Korisnik: {Korisnik}", User.Identity.Name);

            var licence = await _context.Licence
                .Include(x => x.Klijent)
                .Include(x => x.Program)
                .ToListAsync();

            return View(licence);
        }

        // GET: Licence/Details/{id}
        [HttpGet]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details: ID nije pronađen");
                return NotFound();
            }

            var licenca = await _context.Licence
                .Include(x => x.Klijent)
                .Include(x => x.Program)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (licenca == null)
            {
                _logger.LogWarning("Details: Licenca s ID-om {Id} nije pronađena", id);
                return NotFound();
            }

            _logger.LogInformation("Details: Prikaz detalja za licencu ID {Id}", id);
            return View(licenca);
        }

        // GET: Licence/Create
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            _logger.LogInformation("Create: Prikaz forme za novu licencu - Korisnik: {Korisnik}", User.Identity.Name);

            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime");
            ViewData["ProgramId"] = new SelectList(_context.LicencniProgrami, "Id", "Naziv");
            return View();
        }

        // POST: Licence/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Create([Bind("KlijentId,ProgramId,DatumIzdavanja,ValidnaDo")] Licenca licenca)
        {
            _logger.LogInformation("----- POST: Licence/Create ----- Korisnik: {Korisnik}", User.Identity.Name);
            _logger.LogInformation("Parametri: KlijentId={KlijentId}, ProgramId={ProgramId}, DatumIzdavanja={DatumIzdavanja}, ValidnaDo={ValidnaDo}",
                licenca.KlijentId, licenca.ProgramId, licenca.DatumIzdavanja, licenca.ValidnaDo);

            if (ModelState.IsValid)
            {
                try
                {
                    if (licenca.ValidnaDo < licenca.DatumIzdavanja)
                    {
                        ModelState.AddModelError("ValidnaDo", "Datum isteka licence ne može biti prije datuma izdavanja");
                        throw new InvalidOperationException("Nevaljani datumi licence");
                    }

                    _context.Add(licenca);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Kreirana nova licenca ID {Id}", licenca.Id);
                    TempData["Uspjeh"] = "Licenca uspješno kreirana!";

                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning("Validacijska greška: {Poruka}", ex.Message);
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri kreiranju licence");
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                }
            }

            _logger.LogWarning("Neuspješna validacija: {BrojGrešaka} grešaka", ModelState.ErrorCount);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", licenca.KlijentId);
            ViewData["ProgramId"] = new SelectList(_context.LicencniProgrami, "Id", "Naziv", licenca.ProgramId);
            return View(licenca);
        }

        // GET: Licence/Edit/{id}
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

            var licenca = await _context.Licence.FindAsync(id);
            if (licenca == null)
            {
                _logger.LogWarning("Edit GET: Licenca s ID-om {Id} nije pronađena", id);
                return NotFound();
            }

            _logger.LogInformation("Edit GET: Uređivanje licence ID {Id}", id);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", licenca.KlijentId);
            ViewData["ProgramId"] = new SelectList(_context.LicencniProgrami, "Id", "Naziv", licenca.ProgramId);
            return View(licenca);
        }

        // POST: Licence/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KlijentId,ProgramId,DatumIzdavanja,ValidnaDo")] Licenca licenca)
        {
            _logger.LogInformation("----- POST: Licence/Edit/{id} ----- Korisnik: {Korisnik}", id, User.Identity.Name);
            _logger.LogInformation("Parametri: Id={Id}, KlijentId={KlijentId}, ProgramId={ProgramId}, DatumIzdavanja={DatumIzdavanja}, ValidnaDo={ValidnaDo}",
                licenca.Id, licenca.KlijentId, licenca.ProgramId, licenca.DatumIzdavanja, licenca.ValidnaDo);

            if (id != licenca.Id)
            {
                _logger.LogWarning("Edit POST: ID u rutu ({RutaId}) i modelu ({ModelId}) se ne podudaraju", id, licenca.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (licenca.ValidnaDo < licenca.DatumIzdavanja)
                    {
                        ModelState.AddModelError("ValidnaDo", "Datum isteka licence ne može biti prije datuma izdavanja");
                        throw new InvalidOperationException("Nevaljani datumi licence");
                    }

                    _context.Update(licenca);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Licenca ID {Id} uspješno ažurirana", id);
                    TempData["Uspjeh"] = "Licenca uspješno ažurirana!";

                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning("Validacijska greška: {Poruka}", ex.Message);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!LicencaExists(licenca.Id))
                    {
                        _logger.LogWarning("Edit POST: Licenca s ID-om {Id} više ne postoji u bazi", id);
                        return NotFound();
                    }
                    _logger.LogError(ex, "Greška pri ažuriranju licence ID {Id}", id);
                    TempData["Greska"] = "Greška pri ažuriranju. Podatak je promijenjen ili obrisan od strane drugog korisnika.";
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri ažuriranju licence ID {Id}", id);
                    ModelState.AddModelError("", "Greška pri spremanju promjena. Pokušajte ponovno.");
                }
            }

            _logger.LogWarning("Neuspješna validacija: {BrojGrešaka} grešaka", ModelState.ErrorCount);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", licenca.KlijentId);
            ViewData["ProgramId"] = new SelectList(_context.LicencniProgrami, "Id", "Naziv", licenca.ProgramId);
            return View(licenca);
        }

        // GET: Licence/Delete/{id}
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

            var licenca = await _context.Licence
                .Include(x => x.Klijent)
                .Include(x => x.Program)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (licenca == null)
            {
                _logger.LogWarning("Delete GET: Licenca s ID-om {Id} nije pronađena", id);
                return NotFound();
            }

            var status = licenca.ValidnaDo < DateTime.Now ? "Istekla" : "Aktivna";
            ViewData["Status"] = status;

            _logger.LogInformation("Delete GET: Potvrda brisanja licence ID {Id}", id);
            return View(licenca);
        }

        // POST: Licence/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("----- POST: Licence/Delete/{id} ----- Korisnik: {Korisnik}", id, User.Identity.Name);

            var licenca = await _context.Licence.FindAsync(id);
            if (licenca == null)
            {
                _logger.LogWarning("DeleteConfirmed: Licenca s ID-om {Id} nije pronađena", id);
                TempData["Greska"] = "Licenca nije pronađena!";
                return RedirectToAction(nameof(Index));
            }

            if (licenca.ValidnaDo > DateTime.Now)
            {
                TempData["Greska"] = "Ne možete obrisati aktivnu licencu!";
                return RedirectToAction(nameof(Delete), new { id });
            }

            try
            {
                _context.Licence.Remove(licenca);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Licenca ID {Id} uspješno obrisana", id);
                TempData["Uspjeh"] = "Licenca uspješno obrisana!";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju licence ID {Id}", id);
                TempData["Greska"] = "Greška pri brisanju licence. Pokušajte ponovno ili kontaktirajte administratora.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LicencaExists(int id)
        {
            return _context.Licence.Any(e => e.Id == id);
        }
    }
}
