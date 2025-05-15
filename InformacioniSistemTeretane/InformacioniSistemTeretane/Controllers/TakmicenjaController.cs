// Controllers/TakmicenjaController.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;

namespace InformacioniSistemTeretane.Controllers
{
    public class TakmicenjaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TakmicenjaController> _logger;

        public TakmicenjaController(ApplicationDbContext context, ILogger<TakmicenjaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Takmicenja
        public async Task<IActionResult> Index()
        {
            var takmicenja = _context.Takmicenja.Include(t => t.Lokacija);
            return View(await takmicenja.ToListAsync());
        }

        // GET: Takmicenja/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var takmicenje = await _context.Takmicenja
                .Include(t => t.Lokacija)
                .Include(t => t.Discipline)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (takmicenje == null) return NotFound();

            return View(takmicenje);
        }

        // GET: Takmicenja/Create
        public IActionResult Create()
        {
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv");
            return View();
        }

        // POST: Takmicenja/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,Datum,LokacijaId,Opis,Kotizacija")] Takmicenje takmicenje)
        {
            // 1) Log bindanih vrijednosti
            _logger.LogInformation("----- Create Takmicenje bind values -----");
            _logger.LogInformation("Naziv: {Naziv}", takmicenje.Naziv);
            _logger.LogInformation("Datum: {Datum}", takmicenje.Datum);
            _logger.LogInformation("LokacijaId: {LokacijaId}", takmicenje.LokacijaId);
            _logger.LogInformation("Opis: {Opis}", takmicenje.Opis);
            _logger.LogInformation("Kotizacija: {Kotizacija}", takmicenje.Kotizacija);

            // 2) Ako validacija ne prođe, ispiši greške
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors:");
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        foreach (var error in entry.Value.Errors)
                        {
                            _logger.LogWarning(
                                " - Field '{Field}' attempted value '{AttemptedValue}': {ErrorMessage}",
                                entry.Key,
                                entry.Value.AttemptedValue,
                                error.ErrorMessage
                            );
                        }
                    }
                }

                ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", takmicenje.LokacijaId);
                return View(takmicenje);
            }

            // 3) Spremi u bazu
            _context.Add(takmicenje);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Takmicenja/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var takmicenje = await _context.Takmicenja.FindAsync(id);
            if (takmicenje == null) return NotFound();
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", takmicenje.LokacijaId);
            return View(takmicenje);
        }

        // POST: Takmicenja/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Datum,LokacijaId,Opis,Kotizacija")] Takmicenje takmicenje)
        {
            // Log bindanih vrijednosti
            _logger.LogInformation("----- Edit Takmicenje bind values -----");
            _logger.LogInformation("Id: {Id}", takmicenje.Id);
            _logger.LogInformation("Naziv: {Naziv}", takmicenje.Naziv);
            _logger.LogInformation("Datum: {Datum}", takmicenje.Datum);
            _logger.LogInformation("LokacijaId: {LokacijaId}", takmicenje.LokacijaId);
            _logger.LogInformation("Opis: {Opis}", takmicenje.Opis);
            _logger.LogInformation("Kotizacija: {Kotizacija}", takmicenje.Kotizacija);

            if (id != takmicenje.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors:");
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        foreach (var error in entry.Value.Errors)
                        {
                            _logger.LogWarning(
                                " - Field '{Field}' attempted value '{AttemptedValue}': {ErrorMessage}",
                                entry.Key,
                                entry.Value.AttemptedValue,
                                error.ErrorMessage
                            );
                        }
                    }
                }

                ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", takmicenje.LokacijaId);
                return View(takmicenje);
            }

            try
            {
                _context.Update(takmicenje);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Takmicenja.Any(e => e.Id == takmicenje.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Takmicenja/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var takmicenje = await _context.Takmicenja
                .Include(t => t.Lokacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (takmicenje == null) return NotFound();

            return View(takmicenje);
        }

        // POST: Takmicenja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var takmicenje = await _context.Takmicenja.FindAsync(id);
            _context.Takmicenja.Remove(takmicenje);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TakmicenjeExists(int id)
            => _context.Takmicenja.Any(e => e.Id == id);
    }
}
