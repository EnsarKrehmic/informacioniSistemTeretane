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
    public class TakmicariController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TakmicariController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Takmicari
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Takmicari.Include(t => t.Disciplina).Include(t => t.Klijent);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Takmicari/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var takmicar = await _context.Takmicari
                .Include(t => t.Disciplina)
                .Include(t => t.Klijent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (takmicar == null)
            {
                return NotFound();
            }

            return View(takmicar);
        }

        // GET: Takmicari/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv");
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime");
            return View();
        }

        // POST: Takmicari/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,KlijentId,DisciplinaId,Rezultat,Pozicija")] Takmicar takmicar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(takmicar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", takmicar.DisciplinaId);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", takmicar.KlijentId);
            return View(takmicar);
        }

        // GET: Takmicari/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var takmicar = await _context.Takmicari.FindAsync(id);
            if (takmicar == null)
            {
                return NotFound();
            }
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", takmicar.DisciplinaId);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", takmicar.KlijentId);
            return View(takmicar);
        }

        // POST: Takmicari/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KlijentId,DisciplinaId,Rezultat,Pozicija")] Takmicar takmicar)
        {
            if (id != takmicar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(takmicar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TakmicarExists(takmicar.Id))
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
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", takmicar.DisciplinaId);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", takmicar.KlijentId);
            return View(takmicar);
        }

        // GET: Takmicari/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var takmicar = await _context.Takmicari
                .Include(t => t.Disciplina)
                .Include(t => t.Klijent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (takmicar == null)
            {
                return NotFound();
            }

            return View(takmicar);
        }

        // POST: Takmicari/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var takmicar = await _context.Takmicari.FindAsync(id);
            if (takmicar != null)
            {
                _context.Takmicari.Remove(takmicar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TakmicarExists(int id)
        {
            return _context.Takmicari.Any(e => e.Id == id);
        }
    }
}
