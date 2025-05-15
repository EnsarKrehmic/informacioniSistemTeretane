using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;

namespace InformacioniSistemTeretane.Controllers
{
    public class TreneriController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TreneriController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Treneri
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Treneri.Include(t => t.Zaposlenik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Treneri/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trener = await _context.Treneri
                .Include(t => t.Zaposlenik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trener == null)
            {
                return NotFound();
            }

            return View(trener);
        }

        // GET: Treneri/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Ime");
            return View();
        }

        // POST: Treneri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,ZaposlenikId")] Trener trener)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trener);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Ime", trener.ZaposlenikId);
            return View(trener);
        }

        // GET: Treneri/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trener = await _context.Treneri.FindAsync(id);
            if (trener == null)
            {
                return NotFound();
            }
            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Ime", trener.ZaposlenikId);
            return View(trener);
        }

        // POST: Treneri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ZaposlenikId")] Trener trener)
        {
            if (id != trener.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trener);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrenerExists(trener.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Ime", trener.ZaposlenikId);
            return View(trener);
        }

        // GET: Treneri/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trener = await _context.Treneri
                .Include(t => t.Zaposlenik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trener == null)
            {
                return NotFound();
            }

            return View(trener);
        }

        // POST: Treneri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trener = await _context.Treneri.FindAsync(id);
            if (trener != null)
            {
                _context.Treneri.Remove(trener);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrenerExists(int id)
        {
            return _context.Treneri.Any(e => e.Id == id);
        }
    }
}
