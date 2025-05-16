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
    public class IgraonicaPonudeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IgraonicaPonudeController> _logger;

        public IgraonicaPonudeController(ApplicationDbContext context, ILogger<IgraonicaPonudeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: IgraonicaPonude
        public async Task<IActionResult> Index()
        {
            var ponude = _context.IgraonicaPonude.Include(p => p.Igraonica);
            return View(await ponude.ToListAsync());
        }

        // GET: IgraonicaPonude/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ponuda = await _context.IgraonicaPonude
                .Include(p => p.Igraonica)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ponuda == null) return NotFound();

            return View(ponuda);
        }

        // GET: IgraonicaPonude/Create
        public IActionResult Create()
        {
            ViewData["IgraonicaId"] = new SelectList(_context.Igraonice, "Id", "Naziv");
            return View();
        }

        // POST: IgraonicaPonude/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IgraonicaId,OpisUsluge,Cijena,Trajanje")] IgraonicaPonuda ponuda)
        {
            // Log bind values
            _logger.LogInformation("----- Create IgraonicaPonuda bind values -----");
            _logger.LogInformation("IgraonicaId: {IgraonicaId}", ponuda.IgraonicaId);
            _logger.LogInformation("OpisUsluge: {Opis}", ponuda.OpisUsluge);
            _logger.LogInformation("Cijena: {Cijena}", ponuda.Cijena);
            _logger.LogInformation("Trajanje: {Trajanje}", ponuda.Trajanje);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors:");
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        foreach (var err in entry.Value.Errors)
                        {
                            _logger.LogWarning(
                                " - Field '{Field}' attempted value '{AttemptedValue}': {ErrorMessage}",
                                entry.Key,
                                entry.Value.AttemptedValue,
                                err.ErrorMessage
                            );
                        }
                    }
                }
                ViewData["IgraonicaId"] = new SelectList(_context.Igraonice, "Id", "Naziv", ponuda.IgraonicaId);
                return View(ponuda);
            }

            _context.Add(ponuda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: IgraonicaPonude/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ponuda = await _context.IgraonicaPonude.FindAsync(id);
            if (ponuda == null) return NotFound();

            ViewData["IgraonicaId"] = new SelectList(_context.Igraonice, "Id", "Naziv", ponuda.IgraonicaId);
            return View(ponuda);
        }

        // POST: IgraonicaPonude/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IgraonicaId,OpisUsluge,Cijena,Trajanje")] IgraonicaPonuda ponuda)
        {
            _logger.LogInformation("----- Edit IgraonicaPonuda bind values -----");
            _logger.LogInformation("Id: {Id}", ponuda.Id);
            _logger.LogInformation("IgraonicaId: {IgraonicaId}", ponuda.IgraonicaId);
            _logger.LogInformation("OpisUsluge: {Opis}", ponuda.OpisUsluge);
            _logger.LogInformation("Cijena: {Cijena}", ponuda.Cijena);
            _logger.LogInformation("Trajanje: {Trajanje}", ponuda.Trajanje);

            if (id != ponuda.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors:");
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        foreach (var err in entry.Value.Errors)
                        {
                            _logger.LogWarning(
                                " - Field '{Field}' attempted value '{AttemptedValue}': {ErrorMessage}",
                                entry.Key,
                                entry.Value.AttemptedValue,
                                err.ErrorMessage
                            );
                        }
                    }
                }
                ViewData["IgraonicaId"] = new SelectList(_context.Igraonice, "Id", "Naziv", ponuda.IgraonicaId);
                return View(ponuda);
            }

            try
            {
                _context.Update(ponuda);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.IgraonicaPonude.Any(e => e.Id == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: IgraonicaPonude/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ponuda = await _context.IgraonicaPonude
                .Include(p => p.Igraonica)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ponuda == null) return NotFound();

            return View(ponuda);
        }

        // POST: IgraonicaPonude/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ponuda = await _context.IgraonicaPonude.FindAsync(id);
            _context.IgraonicaPonude.Remove(ponuda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
