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
    public class LicenceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LicenceController> _logger;

        public LicenceController(ApplicationDbContext context, ILogger<LicenceController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Licence
        public async Task<IActionResult> Index()
        {
            var l = _context.Licence
                .Include(x => x.Klijent)
                .Include(x => x.Program);
            return View(await l.ToListAsync());
        }

        // GET: Licence/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var licenca = await _context.Licence
                .Include(x => x.Klijent)
                .Include(x => x.Program)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licenca == null) return NotFound();

            return View(licenca);
        }

        // GET: Licence/Create
        public IActionResult Create()
        {
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime");
            ViewData["ProgramId"] = new SelectList(_context.LicencniProgrami, "Id", "Naziv");
            return View();
        }

        // POST: Licence/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KlijentId,ProgramId,DatumIzdavanja,ValidnaDo")] Licenca m)
        {
            _logger.LogInformation("----- Create Licenca bind -----");
            _logger.LogInformation("KlijentId: {KlijentId}, ProgramId: {ProgramId}, DatumIzdavanja: {Datum}, ValidnaDo: {Valid}",
                m.KlijentId, m.ProgramId, m.DatumIzdavanja, m.ValidnaDo);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid for Licenca:");
                foreach (var e in ModelState)
                    foreach (var err in e.Value.Errors)
                        _logger.LogWarning(" - {Field}: '{Value}' => {Error}",
                            e.Key, e.Value.AttemptedValue, err.ErrorMessage);

                ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", m.KlijentId);
                ViewData["ProgramId"] = new SelectList(_context.LicencniProgrami, "Id", "Naziv", m.ProgramId);
                return View(m);
            }

            _context.Add(m);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Licence/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var m = await _context.Licence.FindAsync(id);
            if (m == null) return NotFound();

            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", m.KlijentId);
            ViewData["ProgramId"] = new SelectList(_context.LicencniProgrami, "Id", "Naziv", m.ProgramId);
            return View(m);
        }

        // POST: Licence/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KlijentId,ProgramId,DatumIzdavanja,ValidnaDo")] Licenca m)
        {
            _logger.LogInformation("----- Edit Licenca bind -----");
            _logger.LogInformation("Id: {Id}, KlijentId: {KlijentId}, ProgramId: {ProgramId}, DatumIzdavanja: {Datum}, ValidnaDo: {Valid}",
                m.Id, m.KlijentId, m.ProgramId, m.DatumIzdavanja, m.ValidnaDo);

            if (id != m.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid for Licenca:");
                foreach (var e in ModelState)
                    foreach (var err in e.Value.Errors)
                        _logger.LogWarning(" - {Field}: '{Value}' => {Error}",
                            e.Key, e.Value.AttemptedValue, err.ErrorMessage);

                ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", m.KlijentId);
                ViewData["ProgramId"] = new SelectList(_context.LicencniProgrami, "Id", "Naziv", m.ProgramId);
                return View(m);
            }

            try
            {
                _context.Update(m);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Licence.Any(e => e.Id == m.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Licence/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var licenca = await _context.Licence
                .Include(x => x.Klijent)
                .Include(x => x.Program)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licenca == null) return NotFound();
            return View(licenca);
        }

        // POST: Licence/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var m = await _context.Licence.FindAsync(id);
            _context.Licence.Remove(m);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
