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
    public class UplateController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UplateController> _logger;

        public UplateController(ApplicationDbContext context, ILogger<UplateController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Uplate
        public async Task<IActionResult> Index()
        {
            var uplate = _context.Uplate
                .Include(u => u.Klijent)
                .Include(u => u.Paket);
            return View(await uplate.ToListAsync());
        }

        // GET: Uplate/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var uplata = await _context.Uplate
                .Include(u => u.Klijent)
                .Include(u => u.Paket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uplata == null) return NotFound();

            return View(uplata);
        }

        // GET: Uplate/Create
        public IActionResult Create()
        {
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime");
            ViewData["PaketId"] = new SelectList(_context.Paketi, "Id", "Naziv");
            return View();
        }

        // POST: Uplate/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KlijentId,PaketId,DatumUplate,NacinUplate,Iznos")] Uplata u)
        {
            _logger.LogInformation("----- Create Uplata bind values -----");
            _logger.LogInformation("KlijentId: {KlijentId}", u.KlijentId);
            _logger.LogInformation("PaketId: {PaketId}", u.PaketId);
            _logger.LogInformation("DatumUplate: {Datum}", u.DatumUplate);
            _logger.LogInformation("NacinUplate: {Nacin}", u.NacinUplate);
            _logger.LogInformation("Iznos: {Iznos}", u.Iznos);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid for Uplata:");
                foreach (var e in ModelState)
                    foreach (var err in e.Value.Errors)
                        _logger.LogWarning(
                            " - Field '{Field}' attempted value '{Value}': {Error}",
                            e.Key, e.Value.AttemptedValue, err.ErrorMessage);

                ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", u.KlijentId);
                ViewData["PaketId"] = new SelectList(_context.Paketi, "Id", "Naziv", u.PaketId);
                return View(u);
            }

            _context.Add(u);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Uplate/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var u = await _context.Uplate.FindAsync(id);
            if (u == null) return NotFound();

            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", u.KlijentId);
            ViewData["PaketId"] = new SelectList(_context.Paketi, "Id", "Naziv", u.PaketId);
            return View(u);
        }

        // POST: Uplate/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KlijentId,PaketId,DatumUplate,NacinUplate,Iznos")] Uplata u)
        {
            _logger.LogInformation("----- Edit Uplata bind values -----");
            _logger.LogInformation("Id: {Id}, KlijentId: {KlijentId}, PaketId: {PaketId}, DatumUplate: {Datum}, NacinUplate: {Nacin}, Iznos: {Iznos}",
                u.Id, u.KlijentId, u.PaketId, u.DatumUplate, u.NacinUplate, u.Iznos);

            if (id != u.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid for Uplata:");
                foreach (var e in ModelState)
                    foreach (var err in e.Value.Errors)
                        _logger.LogWarning(
                            " - Field '{Field}' attempted value '{Value}': {Error}",
                            e.Key, e.Value.AttemptedValue, err.ErrorMessage);

                ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", u.KlijentId);
                ViewData["PaketId"] = new SelectList(_context.Paketi, "Id", "Naziv", u.PaketId);
                return View(u);
            }

            try
            {
                _context.Update(u);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Uplate.Any(e => e.Id == u.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Uplate/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var u = await _context.Uplate
                .Include(x => x.Klijent)
                .Include(x => x.Paket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (u == null) return NotFound();
            return View(u);
        }

        // POST: Uplate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var u = await _context.Uplate.FindAsync(id);
            _context.Uplate.Remove(u);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
