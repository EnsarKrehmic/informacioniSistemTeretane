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
    public class IgraoniceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IgraoniceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Igraonice
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Igraonica.Include(i => i.Lokacija);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Igraonice/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var igraonica = await _context.Igraonica
                .Include(i => i.Lokacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (igraonica == null)
            {
                return NotFound();
            }

            return View(igraonica);
        }

        // GET: Igraonice/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["LokacijaId"] = new SelectList(_context.Lokacija, "Id", "Adresa");
            return View();
        }

        // POST: Igraonice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,LokacijaId,Naziv,Kapacitet")] Igraonica igraonica)
        {
            if (ModelState.IsValid)
            {
                _context.Add(igraonica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LokacijaId"] = new SelectList(_context.Lokacija, "Id", "Adresa", igraonica.LokacijaId);
            return View(igraonica);
        }

        // GET: Igraonice/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var igraonica = await _context.Igraonica.FindAsync(id);
            if (igraonica == null)
            {
                return NotFound();
            }
            ViewData["LokacijaId"] = new SelectList(_context.Lokacija, "Id", "Adresa", igraonica.LokacijaId);
            return View(igraonica);
        }

        // POST: Igraonice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LokacijaId,Naziv,Kapacitet")] Igraonica igraonica)
        {
            if (id != igraonica.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(igraonica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IgraonicaExists(igraonica.Id))
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
            ViewData["LokacijaId"] = new SelectList(_context.Lokacija, "Id", "Adresa", igraonica.LokacijaId);
            return View(igraonica);
        }

        // GET: Igraonice/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var igraonica = await _context.Igraonica
                .Include(i => i.Lokacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (igraonica == null)
            {
                return NotFound();
            }

            return View(igraonica);
        }

        // POST: Igraonice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var igraonica = await _context.Igraonica.FindAsync(id);
            if (igraonica != null)
            {
                _context.Igraonica.Remove(igraonica);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IgraonicaExists(int id)
        {
            return _context.Igraonica.Any(e => e.Id == id);
        }
    }
}
