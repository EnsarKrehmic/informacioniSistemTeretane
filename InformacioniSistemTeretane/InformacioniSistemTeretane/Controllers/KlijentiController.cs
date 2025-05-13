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
    public class KlijentiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KlijentiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Klijenti
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Klijent.Include(k => k.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Klijenti/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klijent = await _context.Klijent
                .Include(k => k.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (klijent == null)
            {
                return NotFound();
            }

            return View(klijent);
        }

        // GET: Klijenti/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Klijenti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,Ime,Prezime,DatumRodjenja,UserId")] Klijent klijent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(klijent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", klijent.UserId);
            return View(klijent);
        }

        // GET: Klijenti/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klijent = await _context.Klijent.FindAsync(id);
            if (klijent == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", klijent.UserId);
            return View(klijent);
        }

        // POST: Klijenti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Prezime,DatumRodjenja,UserId")] Klijent klijent)
        {
            if (id != klijent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(klijent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KlijentExists(klijent.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", klijent.UserId);
            return View(klijent);
        }

        // GET: Klijenti/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klijent = await _context.Klijent
                .Include(k => k.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (klijent == null)
            {
                return NotFound();
            }

            return View(klijent);
        }

        // POST: Klijenti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var klijent = await _context.Klijent.FindAsync(id);
            if (klijent != null)
            {
                _context.Klijent.Remove(klijent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KlijentExists(int id)
        {
            return _context.Klijent.Any(e => e.Id == id);
        }
    }
}
