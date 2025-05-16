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
    public class SudijeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SudijeController> _logger;

        public SudijeController(ApplicationDbContext context, ILogger<SudijeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Sudije
        public async Task<IActionResult> Index()
        {
            var sudije = _context.Sudije
                .Include(s => s.Zaposlenik)
                .Include(s => s.Disciplina);
            return View(await sudije.ToListAsync());
        }

        // GET: Sudije/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var sudija = await _context.Sudije
                .Include(s => s.Zaposlenik)
                .Include(s => s.Disciplina)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sudija == null) return NotFound();
            return View(sudija);
        }

        // GET: Sudije/Create
        public IActionResult Create()
        {
            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime");
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv");
            return View();
        }

        // POST: Sudije/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZaposlenikId,DisciplinaId")] Sudija sudija)
        {
            _logger.LogInformation("----- Create Sudija bind values -----");
            _logger.LogInformation("ZaposlenikId: {ZaposlenikId}", sudija.ZaposlenikId);
            _logger.LogInformation("DisciplinaId: {DisciplinaId}", sudija.DisciplinaId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid for Sudija:");
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Any())
                        foreach (var err in entry.Value.Errors)
                            _logger.LogWarning(
                                " - Field '{Field}' attempted value '{Value}': {Error}",
                                entry.Key, entry.Value.AttemptedValue, err.ErrorMessage);
                }
                ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime", sudija.ZaposlenikId);
                ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", sudija.DisciplinaId);
                return View(sudija);
            }

            _context.Add(sudija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Sudije/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var sudija = await _context.Sudije.FindAsync(id);
            if (sudija == null) return NotFound();

            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime", sudija.ZaposlenikId);
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", sudija.DisciplinaId);
            return View(sudija);
        }

        // POST: Sudije/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ZaposlenikId,DisciplinaId")] Sudija sudija)
        {
            _logger.LogInformation("----- Edit Sudija bind values -----");
            _logger.LogInformation("Id: {Id}, ZaposlenikId: {ZaposlenikId}, DisciplinaId: {DisciplinaId}",
                sudija.Id, sudija.ZaposlenikId, sudija.DisciplinaId);

            if (id != sudija.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid for Sudija:");
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Any())
                        foreach (var err in entry.Value.Errors)
                            _logger.LogWarning(
                                " - Field '{Field}' attempted value '{Value}': {Error}",
                                entry.Key, entry.Value.AttemptedValue, err.ErrorMessage);
                }
                ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime", sudija.ZaposlenikId);
                ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", sudija.DisciplinaId);
                return View(sudija);
            }

            try
            {
                _context.Update(sudija);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Sudije.Any(e => e.Id == sudija.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Sudije/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var sudija = await _context.Sudije
                .Include(s => s.Zaposlenik)
                .Include(s => s.Disciplina)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sudija == null) return NotFound();
            return View(sudija);
        }

        // POST: Sudije/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sudija = await _context.Sudije.FindAsync(id);
            _context.Sudije.Remove(sudija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
