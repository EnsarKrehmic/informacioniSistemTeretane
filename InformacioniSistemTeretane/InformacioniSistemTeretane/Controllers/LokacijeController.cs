using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;

namespace InformacioniSistemTeretane.Controllers
{
    public class LokacijeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LokacijeController> _logger;
        public LokacijeController(ApplicationDbContext context, ILogger<LokacijeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Lokacije
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lokacije.ToListAsync());
        }

        // GET: Lokacije/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var lok = await _context.Lokacije
                .Include(l => l.Sale)
                .Include(l => l.Igraonice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lok == null) return NotFound();
            return View(lok);
        }

        // GET: Lokacije/Create
        public IActionResult Create() => View();

        // POST: Lokacije/Create
        // POST: Lokacije/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,Adresa,KontaktTelefon,Email")] Lokacija lokacija)
        {
            // 1) Ispiši bindane vrijednosti
            _logger.LogInformation("----- Create Lokacija bind values -----");
            _logger.LogInformation("Naziv: {Naziv}", lokacija.Naziv);
            _logger.LogInformation("Adresa: {Adresa}", lokacija.Adresa);
            _logger.LogInformation("KontaktTelefon: {KontaktTelefon}", lokacija.KontaktTelefon);
            _logger.LogInformation("Email: {Email}", lokacija.Email);

            // 2) Ako validacija ne prođe, ispiši ModelState greške
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors:");

                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    var state = entry.Value;
                    if (state.Errors.Count > 0)
                    {
                        foreach (var error in state.Errors)
                        {
                            _logger.LogWarning(" - Field '{Field}' attempted value '{AttemptedValue}': {ErrorMessage}",
                                key,
                                state.AttemptedValue,
                                error.ErrorMessage);
                        }
                    }
                }

                // Vrati view s originalnim modelom (da možeš popraviti formu i ponovo testirati)
                return View(lokacija);
            }

            // Ako je validacija prošla, ide na spremanje
            _context.Add(lokacija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Lokacije/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var lok = await _context.Lokacije.FindAsync(id);
            if (lok == null) return NotFound();
            return View(lok);
        }

        // POST: Lokacije/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Adresa,KontaktTelefon,Email")] Lokacija lokacija)
        {
            if (id != lokacija.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lokacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Lokacije.Any(e => e.Id == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(lokacija);
        }

        // GET: Lokacije/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var lok = await _context.Lokacije.FirstOrDefaultAsync(m => m.Id == id);
            if (lok == null) return NotFound();
            return View(lok);
        }

        // POST: Lokacije/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lok = await _context.Lokacije.FindAsync(id);
            _context.Lokacije.Remove(lok);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}