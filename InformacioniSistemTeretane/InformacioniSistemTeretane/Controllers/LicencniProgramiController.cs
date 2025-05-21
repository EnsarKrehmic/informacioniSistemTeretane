// Controllers/LicencniProgramiController.cs
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;

namespace InformacioniSistemTeretane.Controllers
{
    public class LicencniProgramiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LicencniProgramiController> _logger;

        public LicencniProgramiController(ApplicationDbContext context, ILogger<LicencniProgramiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: LicencniProgrami
        public async Task<IActionResult> Index()
        {
            return View(await _context.LicencniProgrami.ToListAsync());
        }

        // GET: LicencniProgrami/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var program = await _context.LicencniProgrami
                .Include(p => p.Licence)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (program == null) return NotFound();

            return View(program);
        }

        // GET: LicencniProgrami/Create
        public IActionResult Create() => View();

        // POST: LicencniProgrami/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,Opis,TrajanjeDana,Cijena")] LicencniProgram program)
        {
            _logger.LogInformation("----- Create LicencniProgram bind -----");
            _logger.LogInformation("Naziv: {Naziv}", program.Naziv);
            _logger.LogInformation("Opis: {Opis}", program.Opis);
            _logger.LogInformation("TrajanjeDana: {Trajanje}", program.TrajanjeDana);
            _logger.LogInformation("Cijena: {Cijena}", program.Cijena);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid for LicencniProgram:");
                foreach (var entry in ModelState)
                    foreach (var err in entry.Value.Errors)
                        _logger.LogWarning(
                            " - Field '{Field}' attempted value '{Value}': {Error}",
                            entry.Key, entry.Value.AttemptedValue, err.ErrorMessage);
                return View(program);
            }

            _context.Add(program);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: LicencniProgrami/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var program = await _context.LicencniProgrami.FindAsync(id);
            if (program == null) return NotFound();
            return View(program);
        }

        // POST: LicencniProgrami/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,TrajanjeDana,Cijena")] LicencniProgram program)
        {
            _logger.LogInformation("----- Edit LicencniProgram bind -----");
            _logger.LogInformation("Id: {Id}, Naziv: {Naziv}, Opis: {Opis}, TrajanjeDana: {Trajanje}, Cijena: {Cijena}",
                program.Id, program.Naziv, program.Opis, program.TrajanjeDana, program.Cijena);

            if (id != program.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState invalid for LicencniProgram:");
                foreach (var entry in ModelState)
                    foreach (var err in entry.Value.Errors)
                        _logger.LogWarning(
                            " - Field '{Field}' attempted value '{Value}': {Error}",
                            entry.Key, entry.Value.AttemptedValue, err.ErrorMessage);
                return View(program);
            }

            try
            {
                _context.Update(program);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.LicencniProgrami.Any(e => e.Id == program.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: LicencniProgrami/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var program = await _context.LicencniProgrami.FirstOrDefaultAsync(m => m.Id == id);
            if (program == null) return NotFound();
            return View(program);
        }

        // POST: LicencniProgrami/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var program = await _context.LicencniProgrami.FindAsync(id);
            _context.LicencniProgrami.Remove(program);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
