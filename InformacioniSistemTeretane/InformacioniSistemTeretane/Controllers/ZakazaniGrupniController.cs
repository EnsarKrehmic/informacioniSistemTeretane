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
    public class ZakazaniGrupniController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZakazaniGrupniController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ZakazaniGrupni
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ZakazaniGrupni.Include(z => z.GrupniTrening).Include(z => z.Klijent);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ZakazaniGrupni/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakazaniGrupni = await _context.ZakazaniGrupni
                .Include(z => z.GrupniTrening)
                .Include(z => z.Klijent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zakazaniGrupni == null)
            {
                return NotFound();
            }

            return View(zakazaniGrupni);
        }

        // GET: ZakazaniGrupni/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["GrupniTreningId"] = new SelectList(_context.GrupniTreninzi, "Id", "Naziv");
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime");
            return View();
        }

        // POST: ZakazaniGrupni/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,GrupniTreningId,KlijentId,DatumPrijave")] ZakazaniGrupni zakazaniGrupni)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zakazaniGrupni);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GrupniTreningId"] = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", zakazaniGrupni.GrupniTreningId);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", zakazaniGrupni.KlijentId);
            return View(zakazaniGrupni);
        }

        // GET: ZakazaniGrupni/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakazaniGrupni = await _context.ZakazaniGrupni.FindAsync(id);
            if (zakazaniGrupni == null)
            {
                return NotFound();
            }
            ViewData["GrupniTreningId"] = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", zakazaniGrupni.GrupniTreningId);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", zakazaniGrupni.KlijentId);
            return View(zakazaniGrupni);
        }

        // POST: ZakazaniGrupni/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GrupniTreningId,KlijentId,DatumPrijave")] ZakazaniGrupni zakazaniGrupni)
        {
            if (id != zakazaniGrupni.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zakazaniGrupni);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZakazaniGrupniExists(zakazaniGrupni.Id))
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
            ViewData["GrupniTreningId"] = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", zakazaniGrupni.GrupniTreningId);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", zakazaniGrupni.KlijentId);
            return View(zakazaniGrupni);
        }

        // GET: ZakazaniGrupni/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zakazaniGrupni = await _context.ZakazaniGrupni
                .Include(z => z.GrupniTrening)
                .Include(z => z.Klijent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zakazaniGrupni == null)
            {
                return NotFound();
            }

            return View(zakazaniGrupni);
        }

        // POST: ZakazaniGrupni/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zakazaniGrupni = await _context.ZakazaniGrupni.FindAsync(id);
            if (zakazaniGrupni != null)
            {
                _context.ZakazaniGrupni.Remove(zakazaniGrupni);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZakazaniGrupniExists(int id)
        {
            return _context.ZakazaniGrupni.Any(e => e.Id == id);
        }
    }
}
