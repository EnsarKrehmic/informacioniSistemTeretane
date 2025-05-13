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
    public class PersonalniTreninziController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonalniTreninziController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PersonalniTreninzi
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PersonalniTrening.Include(p => p.Klijent).Include(p => p.Trener);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PersonalniTreninzi/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalniTrening = await _context.PersonalniTrening
                .Include(p => p.Klijent)
                .Include(p => p.Trener)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personalniTrening == null)
            {
                return NotFound();
            }

            return View(personalniTrening);
        }

        // GET: PersonalniTreninzi/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["KlijentId"] = new SelectList(_context.Klijent, "Id", "Ime");
            ViewData["TrenerId"] = new SelectList(_context.Trener, "Id", "Id");
            return View();
        }

        // POST: PersonalniTreninzi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("TrenerId,KlijentId,Datum,Vrijeme,Napredak,Id,Naziv,Opis,VrstaTreninga")] PersonalniTrening personalniTrening)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personalniTrening);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KlijentId"] = new SelectList(_context.Klijent, "Id", "Ime", personalniTrening.KlijentId);
            ViewData["TrenerId"] = new SelectList(_context.Trener, "Id", "Id", personalniTrening.TrenerId);
            return View(personalniTrening);
        }

        // GET: PersonalniTreninzi/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalniTrening = await _context.PersonalniTrening.FindAsync(id);
            if (personalniTrening == null)
            {
                return NotFound();
            }
            ViewData["KlijentId"] = new SelectList(_context.Klijent, "Id", "Ime", personalniTrening.KlijentId);
            ViewData["TrenerId"] = new SelectList(_context.Trener, "Id", "Id", personalniTrening.TrenerId);
            return View(personalniTrening);
        }

        // POST: PersonalniTreninzi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("TrenerId,KlijentId,Datum,Vrijeme,Napredak,Id,Naziv,Opis,VrstaTreninga")] PersonalniTrening personalniTrening)
        {
            if (id != personalniTrening.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personalniTrening);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonalniTreningExists(personalniTrening.Id))
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
            ViewData["KlijentId"] = new SelectList(_context.Klijent, "Id", "Ime", personalniTrening.KlijentId);
            ViewData["TrenerId"] = new SelectList(_context.Trener, "Id", "Id", personalniTrening.TrenerId);
            return View(personalniTrening);
        }

        // GET: PersonalniTreninzi/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalniTrening = await _context.PersonalniTrening
                .Include(p => p.Klijent)
                .Include(p => p.Trener)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personalniTrening == null)
            {
                return NotFound();
            }

            return View(personalniTrening);
        }

        // POST: PersonalniTreninzi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personalniTrening = await _context.PersonalniTrening.FindAsync(id);
            if (personalniTrening != null)
            {
                _context.PersonalniTrening.Remove(personalniTrening);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonalniTreningExists(int id)
        {
            return _context.PersonalniTrening.Any(e => e.Id == id);
        }
    }
}
