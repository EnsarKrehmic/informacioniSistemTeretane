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
    public class IgraonicaPonudeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IgraonicaPonudeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IgraonicaPonude
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.IgraonicaPonuda.Include(i => i.Igraonica);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: IgraonicaPonude/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var igraonicaPonuda = await _context.IgraonicaPonuda
                .Include(i => i.Igraonica)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (igraonicaPonuda == null)
            {
                return NotFound();
            }

            return View(igraonicaPonuda);
        }

        // GET: IgraonicaPonude/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["IgraonicaId"] = new SelectList(_context.Igraonica, "Id", "Naziv");
            return View();
        }

        // POST: IgraonicaPonude/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,IgraonicaId,OpisUsluge,Cijena,Trajanje")] IgraonicaPonuda igraonicaPonuda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(igraonicaPonuda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IgraonicaId"] = new SelectList(_context.Igraonica, "Id", "Naziv", igraonicaPonuda.IgraonicaId);
            return View(igraonicaPonuda);
        }

        // GET: IgraonicaPonude/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var igraonicaPonuda = await _context.IgraonicaPonuda.FindAsync(id);
            if (igraonicaPonuda == null)
            {
                return NotFound();
            }
            ViewData["IgraonicaId"] = new SelectList(_context.Igraonica, "Id", "Naziv", igraonicaPonuda.IgraonicaId);
            return View(igraonicaPonuda);
        }

        // POST: IgraonicaPonude/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IgraonicaId,OpisUsluge,Cijena,Trajanje")] IgraonicaPonuda igraonicaPonuda)
        {
            if (id != igraonicaPonuda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(igraonicaPonuda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IgraonicaPonudaExists(igraonicaPonuda.Id))
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
            ViewData["IgraonicaId"] = new SelectList(_context.Igraonica, "Id", "Naziv", igraonicaPonuda.IgraonicaId);
            return View(igraonicaPonuda);
        }

        // GET: IgraonicaPonude/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var igraonicaPonuda = await _context.IgraonicaPonuda
                .Include(i => i.Igraonica)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (igraonicaPonuda == null)
            {
                return NotFound();
            }

            return View(igraonicaPonuda);
        }

        // POST: IgraonicaPonude/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var igraonicaPonuda = await _context.IgraonicaPonuda.FindAsync(id);
            if (igraonicaPonuda != null)
            {
                _context.IgraonicaPonuda.Remove(igraonicaPonuda);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IgraonicaPonudaExists(int id)
        {
            return _context.IgraonicaPonuda.Any(e => e.Id == id);
        }
    }
}
