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
            => _context = context;

        // GET: ProbniTreninzi
        public async Task<IActionResult> Index()
        {
            var q = _context.ProbniTreninzi
                .Include(p => p.Klijent)
                .Include(p => p.Trener)
                    .ThenInclude(t => t.Zaposlenik);
            return View(await q.ToListAsync());
        }

        // GET: ProbniTreninzi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var p = await _context.ProbniTreninzi
                .Include(x => x.Klijent)
                .Include(x => x.Trener)
                    .ThenInclude(t => t.Zaposlenik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (p == null) return NotFound();
            return View(p);
        }

        // GET: ProbniTreninzi/Create
        public IActionResult Create()
        {
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime");
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                "Id", "Name"
            );
            return View();
        }

        // POST: ProbniTreninzi/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,Opis,KlijentId,TrenerId,Datum,Ocjena")] ProbniTrening p)
        {
            if (ModelState.IsValid)
            {
                _context.Add(p);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", p.KlijentId);
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                "Id", "Name", p.TrenerId
            );
            return View(p);
        }

        // GET: ProbniTreninzi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var p = await _context.ProbniTreninzi.FindAsync(id);
            if (p == null) return NotFound();
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", p.KlijentId);
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                "Id", "Name", p.TrenerId
            );
            return View(p);
        }

        // POST: ProbniTreninzi/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,KlijentId,TrenerId,Datum,Ocjena")] ProbniTrening p)
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
                    if (!_context.ProbniTreninzi.Any(e => e.Id == p.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", p.KlijentId);
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                "Id", "Name", p.TrenerId
            );
            return View(p);
        }

        // GET: ProbniTreninzi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var p = await _context.ProbniTreninzi
                .Include(x => x.Klijent)
                .Include(x => x.Trener)
                    .ThenInclude(t => t.Zaposlenik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (p == null) return NotFound();
            return View(p);
        }

        // POST: ProbniTreninzi/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p = await _context.ProbniTreninzi.FindAsync(id);
            _context.ProbniTreninzi.Remove(p);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
