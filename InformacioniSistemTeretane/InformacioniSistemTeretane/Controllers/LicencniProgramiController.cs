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
    public class LicencniProgramiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LicencniProgramiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LicencniProgrami
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.LicencniProgram.ToListAsync());
        }

        // GET: LicencniProgrami/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licencniProgram = await _context.LicencniProgram
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licencniProgram == null)
            {
                return NotFound();
            }

            return View(licencniProgram);
        }

        // GET: LicencniProgrami/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: LicencniProgrami/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Opis,TrajanjeDana,Cijena")] LicencniProgram licencniProgram)
        {
            if (ModelState.IsValid)
            {
                _context.Add(licencniProgram);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(licencniProgram);
        }

        // GET: LicencniProgrami/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licencniProgram = await _context.LicencniProgram.FindAsync(id);
            if (licencniProgram == null)
            {
                return NotFound();
            }
            return View(licencniProgram);
        }

        // POST: LicencniProgrami/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,TrajanjeDana,Cijena")] LicencniProgram licencniProgram)
        {
            if (id != licencniProgram.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(licencniProgram);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LicencniProgramExists(licencniProgram.Id))
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
            return View(licencniProgram);
        }

        // GET: LicencniProgrami/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licencniProgram = await _context.LicencniProgram
                .FirstOrDefaultAsync(m => m.Id == id);
            if (licencniProgram == null)
            {
                return NotFound();
            }

            return View(licencniProgram);
        }

        // POST: LicencniProgrami/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var licencniProgram = await _context.LicencniProgram.FindAsync(id);
            if (licencniProgram != null)
            {
                _context.LicencniProgram.Remove(licencniProgram);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LicencniProgramExists(int id)
        {
            return _context.LicencniProgram.Any(e => e.Id == id);
        }
    }
}
