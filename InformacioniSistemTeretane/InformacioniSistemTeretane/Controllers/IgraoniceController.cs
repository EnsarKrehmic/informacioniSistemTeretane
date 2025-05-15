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
    public class IgraoniceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IgraoniceController> _logger;

        public IgraoniceController(ApplicationDbContext context, ILogger<IgraoniceController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Igraonice
        public async Task<IActionResult> Index()
            => View(await _context.Igraonice.Include(i => i.Lokacija).ToListAsync());

        // GET: Igraonice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var igra = await _context.Igraonice
                .Include(i => i.Lokacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (igra == null) return NotFound();
            return View(igra);
        }

        // GET: Igraonice/Create
        public IActionResult Create()
        {
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv");
            return View();
        }

        // POST: Igraonice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,Kapacitet,LokacijaId")] Igraonica igraonica)
        {
            // Log bind values
            _logger.LogInformation("----- Create Igraonica bind values -----");
            _logger.LogInformation("Naziv: {Naziv}", igraonica.Naziv);
            _logger.LogInformation("Kapacitet: {Kapacitet}", igraonica.Kapacitet);
            _logger.LogInformation("LokacijaId: {LokacijaId}", igraonica.LokacijaId);

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

                ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", igraonica.LokacijaId);
                return View(igraonica);
            }

            _context.Add(igraonica);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Igraonice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var igra = await _context.Igraonice.FindAsync(id);
            if (igra == null) return NotFound();
            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", igra.LokacijaId);
            return View(igra);
        }

        // POST: Igraonice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Kapacitet,LokacijaId")] Igraonica igraonica)
        {
            _logger.LogInformation("----- Edit Igraonica bind values -----");
            _logger.LogInformation("Id: {Id}", igraonica.Id);
            _logger.LogInformation("Naziv: {Naziv}", igraonica.Naziv);
            _logger.LogInformation("Kapacitet: {Kapacitet}", igraonica.Kapacitet);
            _logger.LogInformation("LokacijaId: {LokacijaId}", igraonica.LokacijaId);

            if (id != igraonica.Id) return NotFound();

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

                ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", igraonica.LokacijaId);
                return View(igraonica);
            }

            try
            {
                _context.Update(igraonica);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Igraonice.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Igraonice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var igra = await _context.Igraonice
                .Include(i => i.Lokacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (igra == null) return NotFound();
            return View(igra);
        }

        // POST: Igraonice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var igra = await _context.Igraonice.FindAsync(id);
            _context.Igraonice.Remove(igra);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
