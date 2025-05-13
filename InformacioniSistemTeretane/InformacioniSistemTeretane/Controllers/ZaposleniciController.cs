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
    public class ZaposleniciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ZaposleniciController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Zaposlenici
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Zaposlenik.Include(z => z.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Zaposlenici/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zaposlenik = await _context.Zaposlenik
                .Include(z => z.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zaposlenik == null)
            {
                return NotFound();
            }

            return View(zaposlenik);
        }

        // GET: Zaposlenici/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Zaposlenici/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,Ime,Prezime,Pozicija,Telefon,UserId")] Zaposlenik zaposlenik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zaposlenik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", zaposlenik.UserId);
            return View(zaposlenik);
        }

        // GET: Zaposlenici/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zaposlenik = await _context.Zaposlenik.FindAsync(id);
            if (zaposlenik == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", zaposlenik.UserId);
            return View(zaposlenik);
        }

        // POST: Zaposlenici/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Prezime,Pozicija,Telefon,UserId")] Zaposlenik zaposlenik)
        {
            if (id != zaposlenik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zaposlenik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZaposlenikExists(zaposlenik.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", zaposlenik.UserId);
            return View(zaposlenik);
        }

        // GET: Zaposlenici/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zaposlenik = await _context.Zaposlenik
                .Include(z => z.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zaposlenik == null)
            {
                return NotFound();
            }

            return View(zaposlenik);
        }

        // POST: Zaposlenici/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zaposlenik = await _context.Zaposlenik.FindAsync(id);
            if (zaposlenik != null)
            {
                _context.Zaposlenik.Remove(zaposlenik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZaposlenikExists(int id)
        {
            return _context.Zaposlenik.Any(e => e.Id == id);
        }
    }
}
