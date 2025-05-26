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
    public class GrupniTreninziController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GrupniTreninziController> _logger;

        public GrupniTreninziController(ApplicationDbContext context, ILogger<GrupniTreninziController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: GrupniTreninzi
        public async Task<IActionResult> Index()
        {
            var treninzi = _context.Set<GrupniTrening>()
                .Include(g => g.Sala)
                .Include(g => g.Trener).ThenInclude(t => t.Zaposlenik);
            return View(await treninzi.ToListAsync());
        }

        // GET: GrupniTreninzi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var trening = await _context.Set<GrupniTrening>()
                .Include(g => g.Sala)
                .Include(g => g.Trener).ThenInclude(t => t.Zaposlenik)
                .Include(g => g.Rezervacije)
                .Include(g => g.Prisustva)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trening == null) return NotFound();
            return View(trening);
        }

        // GET: GrupniTreninzi/Create
        public IActionResult Create()
        {
            ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv");
            ViewData["TrenerId"] = new SelectList(_context.Treneri
                                                    .Include(t => t.Zaposlenik)
                                                    .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                                                "Id", "Name");
            return View();
        }

        // POST: GrupniTreninzi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,Opis,SalaId,TrenerId,Datum,Vrijeme,MaxUcesnika")] GrupniTrening g)
        {
            _logger.LogInformation("----- Create GrupniTrening bind -----");
            _logger.LogInformation("Naziv: {Naziv}", g.Naziv);
            _logger.LogInformation("Opis: {Opis}", g.Opis);
            _logger.LogInformation("SalaId: {SalaId}", g.SalaId);
            _logger.LogInformation("TrenerId: {TrenerId}", g.TrenerId);
            _logger.LogInformation("Datum: {Datum}", g.Datum);
            _logger.LogInformation("Vrijeme: {Vrijeme}", g.Vrijeme);
            _logger.LogInformation("MaxUcesnika: {Max}", g.MaxUcesnika);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid for GrupniTrening:");
                foreach (var e in ModelState)
                    foreach (var err in e.Value.Errors)
                        _logger.LogWarning(" - {Field}: '{Value}' => {Error}",
                            e.Key, e.Value.AttemptedValue, err.ErrorMessage);

                ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv", g.SalaId);
                ViewData["TrenerId"] = new SelectList(_context.Treneri
                                                        .Include(t => t.Zaposlenik)
                                                        .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                                                    "Id", "Name", g.TrenerId);
                return View(g);
            }

            _context.Add(g);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: GrupniTreninzi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var g = await _context.Set<GrupniTrening>().FindAsync(id);
            if (g == null) return NotFound();

            ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv", g.SalaId);
            ViewData["TrenerId"] = new SelectList(_context.Treneri
                                                    .Include(t => t.Zaposlenik)
                                                    .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                                                "Id", "Name", g.TrenerId);
            return View(g);
        }

        // POST: GrupniTreninzi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,SalaId,TrenerId,Datum,Vrijeme,MaxUcesnika")] GrupniTrening g)
        {
            _logger.LogInformation("----- Edit GrupniTrening bind -----");
            _logger.LogInformation("Id: {Id}, Naziv: {Naziv}, SalaId: {SalaId}, TrenerId: {TrenerId}, Datum: {Datum}, Vrijeme: {Vrijeme}, MaxU: {Max}",
                g.Id, g.Naziv, g.SalaId, g.TrenerId, g.Datum, g.Vrijeme, g.MaxUcesnika);

            if (id != g.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid for GrupniTrening:");
                foreach (var e in ModelState)
                    foreach (var err in e.Value.Errors)
                        _logger.LogWarning(" - {Field}: '{Value}' => {Error}",
                            e.Key, e.Value.AttemptedValue, err.ErrorMessage);

                ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv", g.SalaId);
                ViewData["TrenerId"] = new SelectList(_context.Treneri
                                                        .Include(t => t.Zaposlenik)
                                                        .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                                                    "Id", "Name", g.TrenerId);
                return View(g);
            }

            try
            {
                _context.Update(g);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Set<GrupniTrening>().Any(e => e.Id == g.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: GrupniTreninzi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var g = await _context.Set<GrupniTrening>()
                .Include(g => g.Sala)
                .Include(g => g.Trener).ThenInclude(t => t.Zaposlenik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (g == null) return NotFound();
            return View(g);
        }

        // POST: GrupniTreninzi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var g = await _context.Set<GrupniTrening>().FindAsync(id);
            _context.Set<GrupniTrening>().Remove(g);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
