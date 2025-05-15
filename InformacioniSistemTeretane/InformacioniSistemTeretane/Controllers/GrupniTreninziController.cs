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
    public class GrupniTreninziController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GrupniTreninziController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GrupniTreninzi
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GrupniTreninzi.Include(g => g.Sala).Include(g => g.Trener);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GrupniTreninzi/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupniTrening = await _context.GrupniTreninzi
                .Include(g => g.Sala)
                .Include(g => g.Trener)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grupniTrening == null)
            {
                return NotFound();
            }

            return View(grupniTrening);
        }

        // GET: GrupniTreninzi/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv");
            ViewData["TrenerId"] = new SelectList(_context.Treneri, "Id", "Id");
            return View();
        }

        // POST: GrupniTreninzi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("SalaId,TrenerId,Datum,Vrijeme,MaxUcesnika,Id,Naziv,Opis,VrstaTreninga")] GrupniTrening grupniTrening)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grupniTrening);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv", grupniTrening.SalaId);
            ViewData["TrenerId"] = new SelectList(_context.Treneri, "Id", "Id", grupniTrening.TrenerId);
            return View(grupniTrening);
        }

        // GET: GrupniTreninzi/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupniTrening = await _context.GrupniTreninzi.FindAsync(id);
            if (grupniTrening == null)
            {
                return NotFound();
            }
            ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv", grupniTrening.SalaId);
            ViewData["TrenerId"] = new SelectList(_context.Treneri, "Id", "Id", grupniTrening.TrenerId);
            return View(grupniTrening);
        }

        // POST: GrupniTreninzi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("SalaId,TrenerId,Datum,Vrijeme,MaxUcesnika,Id,Naziv,Opis,VrstaTreninga")] GrupniTrening grupniTrening)
        {
            if (id != grupniTrening.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grupniTrening);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrupniTreningExists(grupniTrening.Id))
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
            ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv", grupniTrening.SalaId);
            ViewData["TrenerId"] = new SelectList(_context.Treneri, "Id", "Id", grupniTrening.TrenerId);
            return View(grupniTrening);
        }

        // GET: GrupniTreninzi/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupniTrening = await _context.GrupniTreninzi
                .Include(g => g.Sala)
                .Include(g => g.Trener)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grupniTrening == null)
            {
                return NotFound();
            }

            return View(grupniTrening);
        }

        // POST: GrupniTreninzi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grupniTrening = await _context.GrupniTreninzi.FindAsync(id);
            if (grupniTrening != null)
            {
                _context.GrupniTreninzi.Remove(grupniTrening);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GrupniTreningExists(int id)
        {
            return _context.GrupniTreninzi.Any(e => e.Id == id);
        }
    }
}
