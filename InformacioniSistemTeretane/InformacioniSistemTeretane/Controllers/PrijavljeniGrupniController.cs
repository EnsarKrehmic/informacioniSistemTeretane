using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;

namespace InformacioniSistemTeretane.Controllers
{
    public class PrijavljeniGrupniController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PrijavljeniGrupniController(ApplicationDbContext context)
            => _context = context;

        // GET: PrijavljeniGrupni
        public async Task<IActionResult> Index()
        {
            var q = _context.PrijavljeniGrupni
                .Include(p => p.GrupniTrening)
                .Include(p => p.Klijent);
            return View(await q.ToListAsync());
        }

        // GET: PrijavljeniGrupni/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var p = await _context.PrijavljeniGrupni
                .Include(x => x.GrupniTrening)
                .Include(x => x.Klijent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (p == null) return NotFound();
            return View(p);
        }

        // GET: PrijavljeniGrupni/Create
        public IActionResult Create()
        {
            ViewBag.GrupniTreningId = new SelectList(_context.GrupniTreninzi, "Id", "Naziv");
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime");
            return View();
        }

        // POST: PrijavljeniGrupni/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GrupniTreningId,KlijentId,Prisutan,VrijemeDolaska")] PrijavljeniGrupni p)
        {
            if (ModelState.IsValid)
            {
                _context.Add(p);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.GrupniTreningId = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", p.GrupniTreningId);
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", p.KlijentId);
            return View(p);
        }

        // GET: PrijavljeniGrupni/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var p = await _context.PrijavljeniGrupni.FindAsync(id);
            if (p == null) return NotFound();
            ViewBag.GrupniTreningId = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", p.GrupniTreningId);
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", p.KlijentId);
            return View(p);
        }

        // POST: PrijavljeniGrupni/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GrupniTreningId,KlijentId,Prisutan,VrijemeDolaska")] PrijavljeniGrupni p)
        {
            if (id != p.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(p);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.PrijavljeniGrupni.Any(e => e.Id == p.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.GrupniTreningId = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", p.GrupniTreningId);
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", p.KlijentId);
            return View(p);
        }

        // GET: PrijavljeniGrupni/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var p = await _context.PrijavljeniGrupni
                .Include(x => x.GrupniTrening)
                .Include(x => x.Klijent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (p == null) return NotFound();
            return View(p);
        }

        // POST: PrijavljeniGrupni/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p = await _context.PrijavljeniGrupni.FindAsync(id);
            _context.PrijavljeniGrupni.Remove(p);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}