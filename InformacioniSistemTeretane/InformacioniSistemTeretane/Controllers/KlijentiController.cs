using System;
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
    public class KlijentiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<KlijentiController> _logger;

        public KlijentiController(ApplicationDbContext context, ILogger<KlijentiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Klijenti
        public async Task<IActionResult> Index()
        {
            var klijenti = _context.Klijenti.Include(k => k.User);
            return View(await klijenti.ToListAsync());
        }

        // GET: Klijenti/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var klijent = await _context.Klijenti
                .Include(k => k.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (klijent == null) return NotFound();
            return View(klijent);
        }

        // GET: Klijenti/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Klijenti/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ime,Prezime,DatumRodjenja,UserId")] Klijent klijent)
        {
            _logger.LogInformation("----- Create Klijent bind values -----");
            _logger.LogInformation("Ime: {Ime}", klijent.Ime);
            _logger.LogInformation("Prezime: {Prezime}", klijent.Prezime);
            _logger.LogInformation("DatumRodjenja: {DatumRodjenja}", klijent.DatumRodjenja);
            _logger.LogInformation("UserId: {UserId}", klijent.UserId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors:");
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        foreach (var err in entry.Value.Errors)
                        {
                            _logger.LogWarning(
                                " - Field '{Field}' attempted value '{Value}': {Error}",
                                entry.Key, entry.Value.AttemptedValue, err.ErrorMessage);
                        }
                    }
                }
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", klijent.UserId);
                return View(klijent);
            }

            _context.Add(klijent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Klijenti/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var klijent = await _context.Klijenti.FindAsync(id);
            if (klijent == null) return NotFound();

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", klijent.UserId);
            return View(klijent);
        }

        // POST: Klijenti/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Prezime,DatumRodjenja,UserId")] Klijent klijent)
        {
            _logger.LogInformation("----- Edit Klijent bind values -----");
            _logger.LogInformation("Id: {Id}", klijent.Id);
            _logger.LogInformation("Ime: {Ime}", klijent.Ime);
            _logger.LogInformation("Prezime: {Prezime}", klijent.Prezime);
            _logger.LogInformation("DatumRodjenja: {DatumRodjenja}", klijent.DatumRodjenja);
            _logger.LogInformation("UserId: {UserId}", klijent.UserId);

            if (id != klijent.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid. Errors:");
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        foreach (var err in entry.Value.Errors)
                        {
                            _logger.LogWarning(
                                " - Field '{Field}' attempted value '{Value}': {Error}",
                                entry.Key, entry.Value.AttemptedValue, err.ErrorMessage);
                        }
                    }
                }
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", klijent.UserId);
                return View(klijent);
            }

            try
            {
                _context.Update(klijent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Klijenti.Any(e => e.Id == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Klijenti/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var klijent = await _context.Klijenti
                .Include(k => k.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (klijent == null) return NotFound();
            return View(klijent);
        }

        // POST: Klijenti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var klijent = await _context.Klijenti.FindAsync(id);
            _context.Klijenti.Remove(klijent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
