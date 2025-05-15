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
    public class SaleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SaleController> _logger;

        public SaleController(ApplicationDbContext context, ILogger<SaleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Sale
        public async Task<IActionResult> Index()
            => View(await _context.Sale.Include(s => s.Lokacija).ToListAsync());

        // GET: Sale/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var sala = await _context.Sale
                .Include(s => s.Lokacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sala == null) return NotFound();
            return View(sala);
        }

        // GET: Sale/Create
        public IActionResult Create()
        {
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv");
            return View();
        }

        // POST: Sale/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,Kapacitet,LokacijaId")] Sala sala)
        {
            // 1) Ispiši bindane vrijednosti
            _logger.LogInformation("----- Create Sala bind values -----");
            _logger.LogInformation("Naziv: {Naziv}", sala.Naziv);
            _logger.LogInformation("Kapacitet: {Kapacitet}", sala.Kapacitet);
            _logger.LogInformation("LokacijaId: {LokacijaId}", sala.LokacijaId);

            // 2) Ako validacija ne prođe, ispiši ModelState greške
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
                                error.ErrorMessage);
                        }
                    }
                }

                ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", sala.LokacijaId);
                return View(sala);
            }

            // 3) Spremi u bazu
            _context.Add(sala);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Sale/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var sala = await _context.Sale.FindAsync(id);
            if (sala == null) return NotFound();
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", sala.LokacijaId);
            return View(sala);
        }

        // POST: Sale/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Kapacitet,LokacijaId")] Sala sala)
        {
            // Log bind values
            _logger.LogInformation("----- Edit Sala bind values -----");
            _logger.LogInformation("Id: {Id}", sala.Id);
            _logger.LogInformation("Naziv: {Naziv}", sala.Naziv);
            _logger.LogInformation("Kapacitet: {Kapacitet}", sala.Kapacitet);
            _logger.LogInformation("LokacijaId: {LokacijaId}", sala.LokacijaId);

            if (id != sala.Id) return NotFound();

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
                                error.ErrorMessage);
                        }
                    }
                }
                ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", sala.LokacijaId);
                return View(sala);
            }

            try
            {
                _context.Update(sala);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Sale.Any(e => e.Id == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Sale/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var sala = await _context.Sale
                .Include(s => s.Lokacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sala == null) return NotFound();
            return View(sala);
        }

        // POST: Sale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sala = await _context.Sale.FindAsync(id);
            _context.Sale.Remove(sala);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
