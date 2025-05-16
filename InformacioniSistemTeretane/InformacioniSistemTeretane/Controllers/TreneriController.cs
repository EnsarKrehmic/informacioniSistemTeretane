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
    public class TreneriController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TreneriController> _logger;

        public TreneriController(ApplicationDbContext context, ILogger<TreneriController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Treneri
        public async Task<IActionResult> Index()
        {
            var treneri = _context.Treneri.Include(t => t.Zaposlenik);
            return View(await treneri.ToListAsync());
        }

        // GET: Treneri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var trener = await _context.Treneri
                .Include(t => t.Zaposlenik)
                .Include(t => t.GrupniTreninzi)
                .Include(t => t.PersonalniTreninzi)
                .Include(t => t.ProbniTreninzi)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (trener == null) return NotFound();
            return View(trener);
        }

        // GET: Treneri/Create
        public IActionResult Create()
        {
            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime");
            return View();
        }

        // POST: Treneri/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZaposlenikId")] Trener trener)
        {
            _logger.LogInformation("----- Create Trener bind values -----");
            _logger.LogInformation("ZaposlenikId: {ZaposlenikId}", trener.ZaposlenikId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid for Trener:");
                foreach (var e in ModelState)
                    foreach (var err in e.Value.Errors)
                        _logger.LogWarning(" - {Field}: '{Value}' => {Error}",
                            e.Key, e.Value.AttemptedValue, err.ErrorMessage);

                ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime", trener.ZaposlenikId);
                return View(trener);
            }

            _context.Add(trener);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Treneri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var trener = await _context.Treneri.FindAsync(id);
            if (trener == null) return NotFound();
            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime", trener.ZaposlenikId);
            return View(trener);
        }

        // POST: Treneri/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ZaposlenikId")] Trener trener)
        {
            _logger.LogInformation("----- Edit Trener bind values -----");
            _logger.LogInformation("Id: {Id}, ZaposlenikId: {ZaposlenikId}", trener.Id, trener.ZaposlenikId);

            if (id != trener.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid for Trener:");
                foreach (var e in ModelState)
                    foreach (var err in e.Value.Errors)
                        _logger.LogWarning(" - {Field}: '{Value}' => {Error}",
                            e.Key, e.Value.AttemptedValue, err.ErrorMessage);

                ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Prezime", trener.ZaposlenikId);
                return View(trener);
            }

            try
            {
                _context.Update(trener);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Treneri.Any(e => e.Id == trener.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Treneri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var trener = await _context.Treneri
                .Include(t => t.Zaposlenik)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (trener == null) return NotFound();
            return View(trener);
        }

        // POST: Treneri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trener = await _context.Treneri.FindAsync(id);
            _context.Treneri.Remove(trener);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
