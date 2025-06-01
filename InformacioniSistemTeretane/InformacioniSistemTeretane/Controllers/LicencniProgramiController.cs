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
    public class LicencniProgramiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LicencniProgramiController> _logger;

        public LicencniProgramiController(ApplicationDbContext context, ILogger<LicencniProgramiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: LicencniProgrami/Index
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("----- GET: LicencniProgrami/Index ----- Korisnik: {Korisnik}", User.Identity.Name);
            var programs = await _context.LicencniProgrami.ToListAsync();
            return View(programs);
        }

        // GET: LicencniProgrami/Details/{id}
        [HttpGet]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Details: ID nije pronađen");
                return NotFound();
            }

            var program = await _context.LicencniProgrami
                .Include(p => p.Licence)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (program == null)
            {
                _logger.LogWarning("Details: Program s ID-om {Id} nije pronađen", id);
                return NotFound();
            }

            _logger.LogInformation("Details: Prikaz detalja za program ID {Id}", id);
            return View(program);
        }

        // GET: LicencniProgrami/Create
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            _logger.LogInformation("Create: Prikaz forme za novi program - Korisnik: {Korisnik}", User.Identity.Name);
            return View();
        }

        // POST: LicencniProgrami/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Create([Bind("Naziv,Opis,TrajanjeDana,Cijena")] LicencniProgram program)
        {
            _logger.LogInformation("----- POST: LicencniProgrami/Create ----- Korisnik: {Korisnik}", User.Identity.Name);
            _logger.LogInformation("Parametri: Naziv={Naziv}, Opis={Opis}, TrajanjeDana={TrajanjeDana}, Cijena={Cijena}",
                program.Naziv, program.Opis, program.TrajanjeDana, program.Cijena);

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _context.LicencniProgrami.AnyAsync(p => p.Naziv == program.Naziv))
                    {
                        ModelState.AddModelError("Naziv", "Program s ovim nazivom već postoji");
                        throw new InvalidOperationException("Naziv programa nije jedinstven");
                    }

                    _context.Add(program);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Kreiran novi program ID {Id}", program.Id);
                    TempData["Uspjeh"] = "Program uspješno kreiran!";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning("Validacijska greška: {Poruka}", ex.Message);
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri kreiranju programa");
                    ModelState.AddModelError("", "Greška pri spremanju podataka. Pokušajte ponovno.");
                }
            }

            _logger.LogWarning("Neuspješna validacija: {BrojGrešaka} grešaka", ModelState.ErrorCount);
            return View(program);
        }

        // GET: LicencniProgrami/Edit/{id}
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

            var program = await _context.LicencniProgrami.FindAsync(id);
            if (program == null)
            {
                _logger.LogWarning("Edit GET: Program s ID-om {Id} nije pronađen", id);
                return NotFound();
            }

            _logger.LogInformation("Edit GET: Uređivanje programa ID {Id}", id);
            return View(program);
        }

        // POST: LicencniProgrami/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,TrajanjeDana,Cijena")] LicencniProgram program)
        {
            _logger.LogInformation("----- POST: LicencniProgrami/Edit/{id} ----- Korisnik: {Korisnik}", id, User.Identity.Name);
            _logger.LogInformation("Parametri: Id={Id}, Naziv={Naziv}, Opis={Opis}, TrajanjeDana={TrajanjeDana}, Cijena={Cijena}",
                program.Id, program.Naziv, program.Opis, program.TrajanjeDana, program.Cijena);

            if (id != program.Id)
            {
                _logger.LogWarning("Edit POST: ID u rutu ({RutaId}) i modelu ({ModelId}) se ne podudaraju", id, program.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (await _context.LicencniProgrami.AnyAsync(p => p.Naziv == program.Naziv && p.Id != program.Id))
                    {
                        ModelState.AddModelError("Naziv", "Program s ovim nazivom već postoji");
                        throw new InvalidOperationException("Naziv programa nije jedinstven");
                    }

                    _context.Update(program);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Program ID {Id} uspješno ažuriran", id);
                    TempData["Uspjeh"] = "Program uspješno ažuriran!";
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    _logger.LogWarning("Validacijska greška: {Poruka}", ex.Message);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!_context.LicencniProgrami.Any(e => e.Id == program.Id))
                    {
                        _logger.LogWarning("Edit POST: Program s ID-om {Id} više ne postoji u bazi", id);
                        return NotFound();
                    }
                    _logger.LogError(ex, "Greška pri ažuriranju programa ID {Id}", id);
                    TempData["Greska"] = "Greška pri ažuriranju. Podatak je promijenjen ili obrisan od strane drugog korisnika.";
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Greška pri ažuriranju programa ID {Id}", id);
                    ModelState.AddModelError("", "Greška pri spremanju promjena. Pokušajte ponovno.");
                }
            }

            _logger.LogWarning("Neuspješna validacija: {BrojGrešaka} grešaka", ModelState.ErrorCount);
            return View(program);
        }

        // GET: LicencniProgrami/Delete/{id}
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

            var program = await _context.LicencniProgrami
                .Include(p => p.Licence)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (program == null)
            {
                _logger.LogWarning("Delete GET: Program s ID-om {Id} nije pronađen", id);
                return NotFound();
            }

            ViewData["ImaLicenci"] = program.Licence?.Any() ?? false;

            _logger.LogInformation("Delete GET: Potvrda brisanja programa ID {Id}", id);
            return View(program);
        }

        // POST: LicencniProgrami/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("----- POST: LicencniProgrami/Delete/{id} ----- Korisnik: {Korisnik}", id, User.Identity.Name);

            var program = await _context.LicencniProgrami
                .Include(p => p.Licence)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (program == null)
            {
                _logger.LogWarning("DeleteConfirmed: Program s ID-om {Id} nije pronađen", id);
                TempData["Greska"] = "Program nije pronađen!";
                return RedirectToAction(nameof(Index));
            }

            if (program.Licence?.Any() == true)
            {
                TempData["Greska"] = "Ne možete obrisati program jer postoje vezane licence!";
                return RedirectToAction(nameof(Delete), new { id });
            }

            try
            {
                _context.LicencniProgrami.Remove(program);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Program ID {Id} uspješno obrisan", id);
                TempData["Uspjeh"] = "Program uspješno obrisan!";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju programa ID {Id}", id);
                TempData["Greska"] = "Greška pri brisanju programa. Pokušajte ponovno ili kontaktirajte administratora.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProgramExists(int id)
        {
            return _context.LicencniProgrami.Any(e => e.Id == id);
        }
    }
}
