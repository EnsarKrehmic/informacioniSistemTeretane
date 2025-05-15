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
    public class PrijavljeniGrupniController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrijavljeniGrupniController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PrijavljeniGrupni
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PrijavljeniGrupni.Include(p => p.GrupniTrening).Include(p => p.Klijent);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PrijavljeniGrupni/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prijavljeniGrupni = await _context.PrijavljeniGrupni
                .Include(p => p.GrupniTrening)
                .Include(p => p.Klijent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prijavljeniGrupni == null)
            {
                return NotFound();
            }

            return View(prijavljeniGrupni);
        }

        // GET: PrijavljeniGrupni/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["GrupniTreningId"] = new SelectList(_context.GrupniTreninzi, "Id", "Naziv");
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime");
            return View();
        }

        // POST: PrijavljeniGrupni/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,GrupniTreningId,KlijentId,Prisutan,VrijemeDolaska")] PrijavljeniGrupni prijavljeniGrupni)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prijavljeniGrupni);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GrupniTreningId"] = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", prijavljeniGrupni.GrupniTreningId);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", prijavljeniGrupni.KlijentId);
            return View(prijavljeniGrupni);
        }

        // GET: PrijavljeniGrupni/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prijavljeniGrupni = await _context.PrijavljeniGrupni.FindAsync(id);
            if (prijavljeniGrupni == null)
            {
                return NotFound();
            }
            ViewData["GrupniTreningId"] = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", prijavljeniGrupni.GrupniTreningId);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", prijavljeniGrupni.KlijentId);
            return View(prijavljeniGrupni);
        }

        // POST: PrijavljeniGrupni/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GrupniTreningId,KlijentId,Prisutan,VrijemeDolaska")] PrijavljeniGrupni prijavljeniGrupni)
        {
            if (id != prijavljeniGrupni.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prijavljeniGrupni);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrijavljeniGrupniExists(prijavljeniGrupni.Id))
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
            ViewData["GrupniTreningId"] = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", prijavljeniGrupni.GrupniTreningId);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", prijavljeniGrupni.KlijentId);
            return View(prijavljeniGrupni);
        }

        // GET: PrijavljeniGrupni/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prijavljeniGrupni = await _context.PrijavljeniGrupni
                .Include(p => p.GrupniTrening)
                .Include(p => p.Klijent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prijavljeniGrupni == null)
            {
                return NotFound();
            }

            return View(prijavljeniGrupni);
        }

        // POST: PrijavljeniGrupni/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prijavljeniGrupni = await _context.PrijavljeniGrupni.FindAsync(id);
            if (prijavljeniGrupni != null)
            {
                _context.PrijavljeniGrupni.Remove(prijavljeniGrupni);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrijavljeniGrupniExists(int id)
        {
            return _context.PrijavljeniGrupni.Any(e => e.Id == id);
        }
    }
}
