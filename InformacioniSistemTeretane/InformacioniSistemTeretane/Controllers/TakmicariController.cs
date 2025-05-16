// Controllers/TakmicariController.cs
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
    public class TakmicariController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TakmicariController> _logger;

        public TakmicariController(ApplicationDbContext context, ILogger<TakmicariController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Takmicari
        public async Task<IActionResult> Index()
        {
            var takmicari = _context.Takmicari
                .Include(t => t.Klijent)
                .Include(t => t.Disciplina);
            return View(await takmicari.ToListAsync());
        }

        // GET: Takmicari/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var t = await _context.Takmicari
                .Include(x => x.Klijent)
                .Include(x => x.Disciplina)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (t == null) return NotFound();
            return View(t);
        }

        // GET: Takmicari/Create
        public IActionResult Create()
        {
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime");
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv");
            return View();
        }

        // POST: Takmicari/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KlijentId,DisciplinaId,Rezultat,Pozicija")] Takmicar t)
        {
            _logger.LogInformation("----- Create Takmicar bind -----");
            _logger.LogInformation("KlijentId: {0}, DisciplinaId: {1}, Rezultat: {2}, Pozicija: {3}",
                t.KlijentId, t.DisciplinaId, t.Rezultat, t.Pozicija);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid for Takmicar:");
                foreach (var e in ModelState)
                    foreach (var err in e.Value.Errors)
                        _logger.LogWarning(" - {Field}: '{Value}' => {Error}",
                            e.Key, e.Value.AttemptedValue, err.ErrorMessage);

                ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", t.KlijentId);
                ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", t.DisciplinaId);
                return View(t);
            }

            _context.Add(t);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Takmicari/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var t = await _context.Takmicari.FindAsync(id);
            if (t == null) return NotFound();

            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", t.KlijentId);
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", t.DisciplinaId);
            return View(t);
        }

        // POST: Takmicari/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KlijentId,DisciplinaId,Rezultat,Pozicija")] Takmicar t)
        {
            _logger.LogInformation("----- Edit Takmicar bind -----");
            _logger.LogInformation("Id: {0}, KlijentId: {1}, DisciplinaId: {2}, Rezultat: {3}, Pozicija: {4}",
                t.Id, t.KlijentId, t.DisciplinaId, t.Rezultat, t.Pozicija);

            if (id != t.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid for Takmicar:");
                foreach (var e in ModelState)
                    foreach (var err in e.Value.Errors)
                        _logger.LogWarning(" - {Field}: '{Value}' => {Error}",
                            e.Key, e.Value.AttemptedValue, err.ErrorMessage);

                ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", t.KlijentId);
                ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", t.DisciplinaId);
                return View(t);
            }

            try
            {
                _context.Update(t);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Takmicari.Any(e => e.Id == t.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Takmicari/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var t = await _context.Takmicari
                .Include(x => x.Klijent)
                .Include(x => x.Disciplina)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (t == null) return NotFound();
            return View(t);
        }

        // POST: Takmicari/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var t = await _context.Takmicari.FindAsync(id);
            _context.Takmicari.Remove(t);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
