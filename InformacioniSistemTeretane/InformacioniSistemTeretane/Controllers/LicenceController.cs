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
    public class LicenceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LicenceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Licence
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Licence.Include(l => l.Klijent).Include(l => l.Program);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Licence/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licenca = await _context.Licence
                .Include(l => l.Klijent)
                .Include(l => l.Program)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licenca == null)
            {
                return NotFound();
            }

            return View(licenca);
        }

        // GET: Licence/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime");
            ViewData["ProgramId"] = new SelectList(_context.LicencniProgrami, "Id", "Naziv");
            return View();
        }

        // POST: Licence/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,KlijentId,ProgramId,DatumIzdavanja,ValidnaDo")] Licenca licenca)
        {
            if (ModelState.IsValid)
            {
                _context.Add(licenca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", licenca.KlijentId);
            ViewData["ProgramId"] = new SelectList(_context.LicencniProgrami, "Id", "Naziv", licenca.ProgramId);
            return View(licenca);
        }

        // GET: Licence/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licenca = await _context.Licence.FindAsync(id);
            if (licenca == null)
            {
                return NotFound();
            }
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", licenca.KlijentId);
            ViewData["ProgramId"] = new SelectList(_context.LicencniProgrami, "Id", "Naziv", licenca.ProgramId);
            return View(licenca);
        }

        // POST: Licence/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KlijentId,ProgramId,DatumIzdavanja,ValidnaDo")] Licenca licenca)
        {
            if (id != licenca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(licenca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LicencaExists(licenca.Id))
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
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", licenca.KlijentId);
            ViewData["ProgramId"] = new SelectList(_context.LicencniProgrami, "Id", "Naziv", licenca.ProgramId);
            return View(licenca);
        }

        // GET: Licence/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licenca = await _context.Licence
                .Include(l => l.Klijent)
                .Include(l => l.Program)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licenca == null)
            {
                return NotFound();
            }

            return View(licenca);
        }

        // POST: Licence/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var licenca = await _context.Licence.FindAsync(id);
            if (licenca != null)
            {
                _context.Licence.Remove(licenca);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LicencaExists(int id)
        {
            return _context.Licence.Any(e => e.Id == id);
        }
    }
}
