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
    public class SudijeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SudijeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sudije
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sudije.Include(s => s.Disciplina).Include(s => s.Zaposlenik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sudije/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sudija = await _context.Sudije
                .Include(s => s.Disciplina)
                .Include(s => s.Zaposlenik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sudija == null)
            {
                return NotFound();
            }

            return View(sudija);
        }

        // GET: Sudije/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv");
            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Ime");
            return View();
        }

        // POST: Sudije/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,ZaposlenikId,DisciplinaId")] Sudija sudija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sudija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", sudija.DisciplinaId);
            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Ime", sudija.ZaposlenikId);
            return View(sudija);
        }

        // GET: Sudije/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sudija = await _context.Sudije.FindAsync(id);
            if (sudija == null)
            {
                return NotFound();
            }
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", sudija.DisciplinaId);
            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Ime", sudija.ZaposlenikId);
            return View(sudija);
        }

        // POST: Sudije/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ZaposlenikId,DisciplinaId")] Sudija sudija)
        {
            if (id != sudija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sudija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SudijaExists(sudija.Id))
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
            ViewData["DisciplinaId"] = new SelectList(_context.Discipline, "Id", "Naziv", sudija.DisciplinaId);
            ViewData["ZaposlenikId"] = new SelectList(_context.Zaposlenici, "Id", "Ime", sudija.ZaposlenikId);
            return View(sudija);
        }

        // GET: Sudije/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sudija = await _context.Sudije
                .Include(s => s.Disciplina)
                .Include(s => s.Zaposlenik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sudija == null)
            {
                return NotFound();
            }

            return View(sudija);
        }

        // POST: Sudije/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sudija = await _context.Sudije.FindAsync(id);
            if (sudija != null)
            {
                _context.Sudije.Remove(sudija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SudijaExists(int id)
        {
            return _context.Sudije.Any(e => e.Id == id);
        }
    }
}
