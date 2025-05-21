using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;

namespace InformacioniSistemTeretane.Controllers
{
    public class PaketiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PaketiController> _logger;

        public PaketiController(ApplicationDbContext context, ILogger<PaketiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Paketi
        public async Task<IActionResult> Index()
        {
            return View(await _context.Paketi.ToListAsync());
        }

        // GET: Paketi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var paket = await _context.Paketi
                .Include(p => p.Uplate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paket == null) return NotFound();

            return View(paket);
        }

        // GET: Paketi/Create
        public IActionResult Create() => View();

        // POST: Paketi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,Opis,Cijena,TrajanjeDana")] Paket paket)
        {
            _logger.LogInformation("----- Create Paket bind values -----");
            _logger.LogInformation("Naziv: {Naziv}", paket.Naziv);
            _logger.LogInformation("Opis: {Opis}", paket.Opis);
            _logger.LogInformation("Cijena: {Cijena}", paket.Cijena);
            _logger.LogInformation("TrajanjeDana: {TrajanjeDana}", paket.TrajanjeDana);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid for Paket:");
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Any())
                        foreach (var err in entry.Value.Errors)
                            _logger.LogWarning(
                                " - Field '{Field}' attempted value '{Value}': {Error}",
                                entry.Key, entry.Value.AttemptedValue, err.ErrorMessage);
                }
                return View(paket);
            }

            _context.Add(paket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Paketi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var paket = await _context.Paketi.FindAsync(id);
            if (paket == null) return NotFound();
            return View(paket);
        }

        // POST: Paketi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,Cijena,TrajanjeDana")] Paket paket)
        {
            _logger.LogInformation("----- Edit Paket bind values -----");
            _logger.LogInformation("Id: {Id}", paket.Id);
            _logger.LogInformation("Naziv: {Naziv}", paket.Naziv);
            _logger.LogInformation("Opis: {Opis}", paket.Opis);
            _logger.LogInformation("Cijena: {Cijena}", paket.Cijena);
            _logger.LogInformation("TrajanjeDana: {TrajanjeDana}", paket.TrajanjeDana);

            if (id != paket.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid for Paket:");
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Any())
                        foreach (var err in entry.Value.Errors)
                            _logger.LogWarning(
                                " - Field '{Field}' attempted value '{Value}': {Error}",
                                entry.Key, entry.Value.AttemptedValue, err.ErrorMessage);
                }
                return View(paket);
            }

            try
            {
                _context.Update(paket);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Paketi.Any(e => e.Id == paket.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Paketi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var paket = await _context.Paketi.FirstOrDefaultAsync(m => m.Id == id);
            if (paket == null) return NotFound();
            return View(paket);
        }

        // POST: Paketi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paket = await _context.Paketi.FindAsync(id);
            _context.Paketi.Remove(paket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
