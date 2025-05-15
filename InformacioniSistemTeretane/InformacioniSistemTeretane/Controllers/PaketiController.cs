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
    public class PaketiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaketiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Paketi
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Paketi.ToListAsync());
        }

        // GET: Paketi/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paket = await _context.Paketi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paket == null)
            {
                return NotFound();
            }

            return View(paket);
        }

        // GET: Paketi/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Paketi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Opis,Cijena,TrajanjeDana")] Paket paket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paket);
        }

        // GET: Paketi/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paket = await _context.Paketi.FindAsync(id);
            if (paket == null)
            {
                return NotFound();
            }
            return View(paket);
        }

        // POST: Paketi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,Cijena,TrajanjeDana")] Paket paket)
        {
            if (id != paket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaketExists(paket.Id))
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
            return View(paket);
        }

        // GET: Paketi/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paket = await _context.Paketi
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paket == null)
            {
                return NotFound();
            }

            return View(paket);
        }

        // POST: Paketi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paket = await _context.Paketi.FindAsync(id);
            if (paket != null)
            {
                _context.Paketi.Remove(paket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaketExists(int id)
        {
            return _context.Paketi.Any(e => e.Id == id);
        }
    }
}
