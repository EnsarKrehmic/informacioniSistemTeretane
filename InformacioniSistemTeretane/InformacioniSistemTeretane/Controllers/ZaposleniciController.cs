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
    public class ZaposleniciController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ZaposleniciController> _logger;

        public ZaposleniciController(ApplicationDbContext context, ILogger<ZaposleniciController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Zaposlenici
        public async Task<IActionResult> Index()
        {
            var zaposleni = _context.Zaposlenici.Include(z => z.User);
            return View(await zaposleni.ToListAsync());
        }

        // GET: Zaposlenici/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var zap = await _context.Zaposlenici
                .Include(z => z.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zap == null) return NotFound();
            return View(zap);
        }

        // GET: Zaposlenici/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Zaposlenici/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ime,Prezime,Pozicija,Telefon,UserId")] Zaposlenik zap)
        {
            _logger.LogInformation("----- Create Zaposlenik bind values -----");
            _logger.LogInformation("Ime: {Ime}", zap.Ime);
            _logger.LogInformation("Prezime: {Prezime}", zap.Prezime);
            _logger.LogInformation("Pozicija: {Pozicija}", zap.Pozicija);
            _logger.LogInformation("Telefon: {Telefon}", zap.Telefon);
            _logger.LogInformation("UserId: {UserId}", zap.UserId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors:");
                foreach (var e in ModelState)
                {
                    if (e.Value.Errors.Count > 0)
                        foreach (var err in e.Value.Errors)
                            _logger.LogWarning(
                                " - Field '{Field}' attempted value '{Value}': {Error}",
                                e.Key, e.Value.AttemptedValue, err.ErrorMessage);
                }
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", zap.UserId);
                return View(zap);
            }

            _context.Add(zap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Zaposlenici/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var zap = await _context.Zaposlenici.FindAsync(id);
            if (zap == null) return NotFound();

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", zap.UserId);
            return View(zap);
        }

        // POST: Zaposlenici/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Prezime,Pozicija,Telefon,UserId")] Zaposlenik zap)
        {
            _logger.LogInformation("----- Edit Zaposlenik bind values -----");
            _logger.LogInformation("Id: {Id}", zap.Id);
            _logger.LogInformation("Ime: {Ime}", zap.Ime);
            _logger.LogInformation("Prezime: {Prezime}", zap.Prezime);
            _logger.LogInformation("Pozicija: {Pozicija}", zap.Pozicija);
            _logger.LogInformation("Telefon: {Telefon}", zap.Telefon);
            _logger.LogInformation("UserId: {UserId}", zap.UserId);

            if (id != zap.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors:");
                foreach (var e in ModelState)
                {
                    if (e.Value.Errors.Count > 0)
                        foreach (var err in e.Value.Errors)
                            _logger.LogWarning(
                                " - Field '{Field}' attempted value '{Value}': {Error}",
                                e.Key, e.Value.AttemptedValue, err.ErrorMessage);
                }
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", zap.UserId);
                return View(zap);
            }

            try
            {
                _context.Update(zap);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Zaposlenici.Any(e => e.Id == zap.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Zaposlenici/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var zap = await _context.Zaposlenici
                .Include(z => z.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zap == null) return NotFound();
            return View(zap);
        }

        // POST: Zaposlenici/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zap = await _context.Zaposlenici.FindAsync(id);
            _context.Zaposlenici.Remove(zap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
