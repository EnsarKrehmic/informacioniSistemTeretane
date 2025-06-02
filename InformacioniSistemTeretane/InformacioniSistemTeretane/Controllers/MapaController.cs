using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace InformacioniSistemTeretane.Controllers
{
    public class MapaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MapaController> _logger;

        public MapaController(ApplicationDbContext context, ILogger<MapaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var lokacije = await _context.Mape.ToListAsync();
                return View(lokacije);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri učitavanju lokacija");
                TempData["Greska"] = "Došlo je do greške pri učitavanju lokacija";
                return View(new List<Mapa>());
            }
        }

        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var mapa = await _context.Mape.FindAsync(id);
                if (mapa == null)
                {
                    _logger.LogWarning($"Mapa sa ID {id} nije pronađena");
                    return NotFound();
                }

                return View(mapa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Greška pri učitavanju mape ID {id}");
                TempData["Greska"] = "Došlo je do greške pri učitavanju lokacije";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Mapa mapa)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(mapa);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Dodana nova lokacija: {mapa.Naziv} (ID: {mapa.Id})");
                    TempData["Uspjeh"] = "Lokacija uspješno dodana!";
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogWarning("Neispravni podaci za novu lokaciju");
                return View(mapa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška pri kreiranju lokacije");
                TempData["Greska"] = "Došlo je do greške pri dodavanju lokacije";
                return View(mapa);
            }
        }

        // Metoda Edit GET
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var mapa = await _context.Mape.FindAsync(id);
                if (mapa == null)
                {
                    _logger.LogWarning($"Pokusaj uređivanja nepostojeće lokacije ID: {id}");
                    TempData["Greska"] = "Lokacija nije pronađena";
                    return RedirectToAction(nameof(Index));
                }

                return View(mapa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Greška pri učitavanju lokacije ID {id} za uređivanje");
                TempData["Greska"] = "Došlo je do greške pri učitavanju lokacije";
                return RedirectToAction(nameof(Index));
            }
        }

        // Metoda Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id, Mapa mapa)
        {
            if (id != mapa.Id)
            {
                _logger.LogWarning($"ID mismatch pri uređivanju lokacije (ID: {id})");
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(mapa);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Lokacija ažurirana: {mapa.Naziv} (ID: {mapa.Id})");
                    TempData["Uspjeh"] = "Lokacija uspješno ažurirana!";
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogWarning("Neispravni podaci pri ažuriranju lokacije");
                return View(mapa);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!MapaExists(mapa.Id))
                {
                    _logger.LogWarning($"Pokusaj uređivanja nepostojeće lokacije ID: {id}");
                    TempData["Greska"] = "Lokacija više ne postoji u sistemu";
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogError(ex, $"Greška pri ažuriranju lokacije ID {id}");
                TempData["Greska"] = "Došlo je do greške pri ažuriranju lokacije";
                return View(mapa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Greška pri ažuriranju lokacije ID {id}");
                TempData["Greska"] = "Došlo je do neočekivane greške";
                return View(mapa);
            }
        }

        // Metoda Delete GET
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var mapa = await _context.Mape.FindAsync(id);
                if (mapa == null)
                {
                    _logger.LogWarning($"Pokusaj brisanja nepostojeće lokacije ID: {id}");
                    TempData["Greska"] = "Lokacija nije pronađena";
                    return RedirectToAction(nameof(Index));
                }

                return View(mapa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Greška pri učitavanju lokacije ID {id} za brisanje");
                TempData["Greska"] = "Došlo je do greške pri učitavanju lokacije";
                return RedirectToAction(nameof(Index));
            }
        }

        // Metoda Delete POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var mapa = await _context.Mape.FindAsync(id);
                if (mapa != null)
                {
                    _context.Mape.Remove(mapa);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Lokacija obrisana: {mapa.Naziv} (ID: {id})");
                    TempData["Uspjeh"] = "Lokacija uspješno obrisana!";
                }
                else
                {
                    _logger.LogWarning($"Pokusaj brisanja nepostojeće lokacije ID: {id}");
                    TempData["Greska"] = "Lokacija više ne postoji u sistemu";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Greška pri brisanju lokacije ID {id}");
                TempData["Greska"] = "Došlo je do greške pri brisanju lokacije";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        // Pomocna metoda
        private bool MapaExists(int id)
        {
            return _context.Mape.Any(e => e.Id == id);
        }
    }
}