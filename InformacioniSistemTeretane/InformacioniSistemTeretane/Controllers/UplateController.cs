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
    public class UplateController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UplateController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Uplate
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Uplate.Include(u => u.Klijent).Include(u => u.Paket);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Uplate/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uplata = await _context.Uplate
                .Include(u => u.Klijent)
                .Include(u => u.Paket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uplata == null)
            {
                return NotFound();
            }

            return View(uplata);
        }

        // GET: Uplate/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime");
            ViewData["PaketId"] = new SelectList(_context.Paketi, "Id", "Naziv");
            return View();
        }

        // POST: Uplate/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,KlijentId,PaketId,DatumUplate,NacinUplate,Iznos")] Uplata uplata)
        {
            if (ModelState.IsValid)
            {
                _context.Add(uplata);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", uplata.KlijentId);
            ViewData["PaketId"] = new SelectList(_context.Paketi, "Id", "Naziv", uplata.PaketId);
            return View(uplata);
        }

        // GET: Uplate/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uplata = await _context.Uplate.FindAsync(id);
            if (uplata == null)
            {
                return NotFound();
            }
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", uplata.KlijentId);
            ViewData["PaketId"] = new SelectList(_context.Paketi, "Id", "Naziv", uplata.PaketId);
            return View(uplata);
        }

        // POST: Uplate/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KlijentId,PaketId,DatumUplate,NacinUplate,Iznos")] Uplata uplata)
        {
            if (id != uplata.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uplata);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UplataExists(uplata.Id))
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
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", uplata.KlijentId);
            ViewData["PaketId"] = new SelectList(_context.Paketi, "Id", "Naziv", uplata.PaketId);
            return View(uplata);
        }

        // GET: Uplate/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uplata = await _context.Uplate
                .Include(u => u.Klijent)
                .Include(u => u.Paket)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uplata == null)
            {
                return NotFound();
            }

            return View(uplata);
        }

        // POST: Uplate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uplata = await _context.Uplate.FindAsync(id);
            if (uplata != null)
            {
                _context.Uplate.Remove(uplata);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UplataExists(int id)
        {
            return _context.Uplate.Any(e => e.Id == id);
        }
    }
}
