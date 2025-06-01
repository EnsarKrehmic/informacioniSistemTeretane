using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InformacioniSistemTeretane.Controllers
{
    public class ScreenshotsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ScreenshotsController> _logger;

        public ScreenshotsController(
            ApplicationDbContext context,
            ILogger<ScreenshotsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Screenshots/Index
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Korisnik {UserName} pregleda listu screenshotova", User.Identity.Name);
            return View(await _context.Screenshots.ToListAsync());
        }

        // GET: Screenshots/Details/{id}
        [HttpGet]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj pristupa detaljima screenshot-a bez ID-a od strane {UserName}", User.Identity.Name);
                return NotFound();
            }

            var screenshot = await _context.Screenshots
                .FirstOrDefaultAsync(m => m.Id == id);

            if (screenshot == null)
            {
                _logger.LogWarning("Screenshot sa ID-om {ScreenshotId} nije pronađen (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} pregleda detalje screenshot-a: {ScreenshotNaziv}", User.Identity.Name, screenshot.Naziv);
            return View(screenshot);
        }

        // GET: Screenshots/Create
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            _logger.LogInformation("Korisnik {UserName} pokreće kreiranje novog screenshot-a", User.Identity.Name);
            return View();
        }

        // POST: Screenshots/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Create([Bind("Naziv,DatotekaSlike")] Screenshot screenshot)
        {
            _logger.LogInformation("Korisnik {UserName} kreira novi screenshot", User.Identity.Name);
            _logger.LogDebug("Parametri: Naziv={Naziv}, DatotekaSlike={DatotekaSlike}",
                screenshot.Naziv, screenshot.DatotekaSlike?.FileName);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Neuspješna validacija za novi screenshot (korisnik: {UserName})", User.Identity.Name);
                return View(screenshot);
            }

            if (screenshot.DatotekaSlike == null || screenshot.DatotekaSlike.Length == 0)
            {
                _logger.LogWarning("Nije priložena datoteka za screenshot (korisnik: {UserName})", User.Identity.Name);
                ModelState.AddModelError(nameof(screenshot.DatotekaSlike), "Potrebno je odabrati sliku.");
                return View(screenshot);
            }

            try
            {
                // Konverzija u Base64
                using (var ms = new MemoryStream())
                {
                    await screenshot.DatotekaSlike.CopyToAsync(ms);
                    screenshot.Opis = $"data:{screenshot.DatotekaSlike.ContentType};base64,{Convert.ToBase64String(ms.ToArray())}";
                }

                _context.Add(screenshot);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Screenshot {Naziv} uspješno kreiran (ID: {ScreenshotId}) od strane {UserName}",
                    screenshot.Naziv, screenshot.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Screenshot '{screenshot.Naziv}' je uspješno kreiran!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri čuvanju screenshot-a u bazu (korisnik: {UserName})", User.Identity.Name);
                ModelState.AddModelError("", "Došlo je do greške pri čuvanju podataka. Pokušajte ponovo.");
                return View(screenshot);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Neočekivana greška pri kreiranju screenshot-a (korisnik: {UserName})", User.Identity.Name);
                ModelState.AddModelError("", "Došlo je do neočekivane greške. Pokušajte ponovo.");
                return View(screenshot);
            }
        }

        // GET: Screenshots/Edit/{id}
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj uređivanja screenshot-a bez ID-a od strane {UserName}", User.Identity.Name);
                return NotFound();
            }

            var screenshot = await _context.Screenshots.FindAsync(id);
            if (screenshot == null)
            {
                _logger.LogWarning("Screenshot sa ID-om {ScreenshotId} nije pronađen za uređivanje (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} uređuje screenshot: {ScreenshotNaziv} (ID: {ScreenshotId})",
                User.Identity.Name, screenshot.Naziv, screenshot.Id);

            return View(screenshot);
        }

        // POST: Screenshots/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,DatotekaSlike")] Screenshot screenshot)
        {
            _logger.LogInformation("Korisnik {UserName} ažurira screenshot ID: {ScreenshotId}", User.Identity.Name, id);
            _logger.LogDebug("Parametri: Naziv={Naziv}, DatotekaSlike={DatotekaSlike}",
                screenshot.Naziv, screenshot.DatotekaSlike?.FileName);

            if (id != screenshot.Id)
            {
                _logger.LogWarning("ID u putanji ({PutanjaId}) i ID screenshot-a ({ScreenshotId}) se ne podudaraju (korisnik: {UserName})", id, screenshot.Id, User.Identity.Name);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Neuspješna validacija za ažuriranje screenshot-a (korisnik: {UserName})", User.Identity.Name);
                return View(screenshot);
            }

            try
            {
                var existingScreenshot = await _context.Screenshots.FindAsync(id);
                if (existingScreenshot == null)
                {
                    _logger.LogWarning("Screenshot sa ID-om {ScreenshotId} nije pronađen (korisnik: {UserName})", id, User.Identity.Name);
                    return NotFound();
                }

                // Ažuriranje slike samo ako je uploadovana nova
                if (screenshot.DatotekaSlike != null && screenshot.DatotekaSlike.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await screenshot.DatotekaSlike.CopyToAsync(ms);
                        existingScreenshot.Opis = $"data:{screenshot.DatotekaSlike.ContentType};base64,{Convert.ToBase64String(ms.ToArray())}";
                    }
                }

                existingScreenshot.Naziv = screenshot.Naziv;
                _context.Update(existingScreenshot);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Screenshot {Naziv} (ID: {ScreenshotId}) uspješno ažuriran od strane {UserName}",
                    screenshot.Naziv, screenshot.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Screenshot '{screenshot.Naziv}' je uspješno ažuriran!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_context.Screenshots.Any(e => e.Id == screenshot.Id))
                {
                    _logger.LogWarning("Screenshot sa ID-om {ScreenshotId} nije pronađen (korisnik: {UserName})", screenshot.Id, User.Identity.Name);
                    return NotFound();
                }

                _logger.LogError(ex, "Greška pri ažuriranju screenshot-a (ID: {ScreenshotId})", screenshot.Id);
                ModelState.AddModelError("", "Došlo je do greške pri čuvanju izmjena. Pokušajte ponovo.");
                return View(screenshot);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Neočekivana greška pri ažuriranju screenshot-a (ID: {ScreenshotId})", id);
                ModelState.AddModelError("", "Došlo je do neočekivane greške. Pokušajte ponovo.");
                return View(screenshot);
            }
        }

        // GET: Screenshots/Delete/{id}
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj brisanja screenshot-a bez ID-a od strane {UserName}", User.Identity.Name);
                return NotFound();
            }

            var screenshot = await _context.Screenshots
                .FirstOrDefaultAsync(m => m.Id == id);

            if (screenshot == null)
            {
                _logger.LogWarning("Screenshot sa ID-om {ScreenshotId} nije pronađen za brisanje (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} pokreće brisanje screenshot-a: {ScreenshotNaziv} (ID: {ScreenshotId})",
                User.Identity.Name, screenshot.Naziv, screenshot.Id);

            return View(screenshot);
        }

        // POST: Screenshots/DeleteConfirmed/{id}
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var screenshot = await _context.Screenshots.FindAsync(id);

            if (screenshot == null)
            {
                _logger.LogWarning("Screenshot sa ID-om {ScreenshotId} nije pronađen za brisanje (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            try
            {
                _context.Screenshots.Remove(screenshot);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Screenshot {Naziv} (ID: {ScreenshotId}) uspješno obrisan od strane {UserName}",
                    screenshot.Naziv, screenshot.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Screenshot '{screenshot.Naziv}' je uspješno obrisan!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju screenshot-a ID: {ScreenshotId} (korisnik: {UserName})", id, User.Identity.Name);

                TempData["Greska"] = $"Došlo je do greške pri brisanju screenshot-a '{screenshot.Naziv}'!";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}