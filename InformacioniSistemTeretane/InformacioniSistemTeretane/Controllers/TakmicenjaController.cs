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
    public class TakmicenjaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TakmicenjaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Takmicenja
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Takmicenje.Include(t => t.Lokacija);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Takmicenja/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var takmicenje = await _context.Takmicenje
                .Include(t => t.Lokacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (takmicenje == null)
            {
                return NotFound();
            }

            return View(takmicenje);
        }

        // GET: Takmicenja/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["LokacijaId"] = new SelectList(_context.Lokacija, "Id", "Adresa");
            return View();
        }

        // POST: Takmicenja/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Datum,LokacijaId,Opis,Kotizacija")] Takmicenje takmicenje)
        {
            if (ModelState.IsValid)
            {
                _context.Add(takmicenje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LokacijaId"] = new SelectList(_context.Lokacija, "Id", "Adresa", takmicenje.LokacijaId);
            return View(takmicenje);
        }

        // GET: Takmicenja/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var takmicenje = await _context.Takmicenje.FindAsync(id);
            if (takmicenje == null)
            {
                return NotFound();
            }
            ViewData["LokacijaId"] = new SelectList(_context.Lokacija, "Id", "Adresa", takmicenje.LokacijaId);
            return View(takmicenje);
        }

        // POST: Takmicenja/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Datum,LokacijaId,Opis,Kotizacija")] Takmicenje takmicenje)
        {
            if (id != takmicenje.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(takmicenje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TakmicenjeExists(takmicenje.Id))
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
            ViewData["LokacijaId"] = new SelectList(_context.Lokacija, "Id", "Adresa", takmicenje.LokacijaId);
            return View(takmicenje);
        }

        // GET: Takmicenja/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var takmicenje = await _context.Takmicenje
                .Include(t => t.Lokacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (takmicenje == null)
            {
                return NotFound();
            }

            return View(takmicenje);
        }

        // POST: Takmicenja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var takmicenje = await _context.Takmicenje.FindAsync(id);
            if (takmicenje != null)
            {
                _context.Takmicenje.Remove(takmicenje);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TakmicenjeExists(int id)
        {
            return _context.Takmicenje.Any(e => e.Id == id);
        }
    }
}
