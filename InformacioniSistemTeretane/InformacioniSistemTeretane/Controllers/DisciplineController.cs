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
    public class DisciplineController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DisciplineController> _logger;

        public DisciplineController(ApplicationDbContext context, ILogger<DisciplineController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Disciplina
        public async Task<IActionResult> Index()
        {
            var discipline = _context.Discipline.Include(d => d.Takmicenje);
            return View(await discipline.ToListAsync());
        }

        // GET: Disciplina/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var disciplina = await _context.Discipline
                .Include(d => d.Takmicenje)
                .Include(d => d.Takmicari)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (disciplina == null) return NotFound();

            return View(disciplina);
        }

        // GET: Disciplina/Create
        public IActionResult Create()
        {
            ViewData["TakmicenjeId"] = new SelectList(_context.Takmicenja, "Id", "Naziv");
            return View();
        }

        // POST: Disciplina/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TakmicenjeId,Naziv,Opis,MaxUcesnika")] Disciplina d)
        {
            _logger.LogInformation("----- Create Disciplina bind values -----");
            _logger.LogInformation("TakmicenjeId: {TakmicenjeId}", d.TakmicenjeId);
            _logger.LogInformation("Naziv: {Naziv}", d.Naziv);
            _logger.LogInformation("Opis: {Opis}", d.Opis);
            _logger.LogInformation("MaxUcesnika: {MaxUcesnika}", d.MaxUcesnika);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors:");
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                        foreach (var err in entry.Value.Errors)
                            _logger.LogWarning(
                                " - Field '{Field}' attempted value '{Value}': {Error}",
                                entry.Key, entry.Value.AttemptedValue, err.ErrorMessage
                            );
                }
                ViewData["TakmicenjeId"] = new SelectList(_context.Takmicenja, "Id", "Naziv", d.TakmicenjeId);
                return View(d);
            }

            _context.Add(d);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Disciplina/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var d = await _context.Discipline.FindAsync(id);
            if (d == null) return NotFound();
            ViewData["TakmicenjeId"] = new SelectList(_context.Takmicenja, "Id", "Naziv", d.TakmicenjeId);
            return View(d);
        }

        // POST: Disciplina/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TakmicenjeId,Naziv,Opis,MaxUcesnika")] Disciplina d)
        {
            _logger.LogInformation("----- Edit Disciplina bind values -----");
            _logger.LogInformation("Id: {Id}", d.Id);
            _logger.LogInformation("TakmicenjeId: {TakmicenjeId}", d.TakmicenjeId);
            _logger.LogInformation("Naziv: {Naziv}", d.Naziv);
            _logger.LogInformation("Opis: {Opis}", d.Opis);
            _logger.LogInformation("MaxUcesnika: {MaxUcesnika}", d.MaxUcesnika);

            if (id != d.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors:");
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                        foreach (var err in entry.Value.Errors)
                            _logger.LogWarning(
                                " - Field '{Field}' attempted value '{Value}': {Error}",
                                entry.Key, entry.Value.AttemptedValue, err.ErrorMessage
                            );
                }
                ViewData["TakmicenjeId"] = new SelectList(_context.Takmicenja, "Id", "Naziv", d.TakmicenjeId);
                return View(d);
            }

            try
            {
                _context.Update(d);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Discipline.Any(e => e.Id == d.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Disciplina/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var d = await _context.Discipline
                .Include(d => d.Takmicenje)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (d == null) return NotFound();
            return View(d);
        }

        // POST: Disciplina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var d = await _context.Discipline.FindAsync(id);
            _context.Discipline.Remove(d);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
