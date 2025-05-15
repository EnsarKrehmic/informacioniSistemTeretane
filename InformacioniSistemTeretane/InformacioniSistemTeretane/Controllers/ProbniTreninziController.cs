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
    public class ProbniTreninziController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProbniTreninziController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProbniTreninzi
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ProbniTreninzi.Include(p => p.Klijent).Include(p => p.Trener);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProbniTreninzi/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var probniTrening = await _context.ProbniTreninzi
                .Include(p => p.Klijent)
                .Include(p => p.Trener)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (probniTrening == null)
            {
                return NotFound();
            }

            return View(probniTrening);
        }

        // GET: ProbniTreninzi/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime");
            ViewData["TrenerId"] = new SelectList(_context.Treneri, "Id", "Id");
            return View();
        }

        // POST: ProbniTreninzi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("KlijentId,TrenerId,Datum,Ocjena,Id,Naziv,Opis,VrstaTreninga")] ProbniTrening probniTrening)
        {
            if (ModelState.IsValid)
            {
                _context.Add(probniTrening);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", probniTrening.KlijentId);
            ViewData["TrenerId"] = new SelectList(_context.Treneri, "Id", "Id", probniTrening.TrenerId);
            return View(probniTrening);
        }

        // GET: ProbniTreninzi/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var probniTrening = await _context.ProbniTreninzi.FindAsync(id);
            if (probniTrening == null)
            {
                return NotFound();
            }
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", probniTrening.KlijentId);
            ViewData["TrenerId"] = new SelectList(_context.Treneri, "Id", "Id", probniTrening.TrenerId);
            return View(probniTrening);
        }

        // POST: ProbniTreninzi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("KlijentId,TrenerId,Datum,Ocjena,Id,Naziv,Opis,VrstaTreninga")] ProbniTrening probniTrening)
        {
            if (id != probniTrening.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(probniTrening);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProbniTreningExists(probniTrening.Id))
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
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Ime", probniTrening.KlijentId);
            ViewData["TrenerId"] = new SelectList(_context.Treneri, "Id", "Id", probniTrening.TrenerId);
            return View(probniTrening);
        }

        // GET: ProbniTreninzi/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var probniTrening = await _context.ProbniTreninzi
                .Include(p => p.Klijent)
                .Include(p => p.Trener)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (probniTrening == null)
            {
                return NotFound();
            }

            return View(probniTrening);
        }

        // POST: ProbniTreninzi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var probniTrening = await _context.ProbniTreninzi.FindAsync(id);
            if (probniTrening != null)
            {
                _context.ProbniTreninzi.Remove(probniTrening);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProbniTreningExists(int id)
        {
            return _context.ProbniTreninzi.Any(e => e.Id == id);
        }
    }
}
