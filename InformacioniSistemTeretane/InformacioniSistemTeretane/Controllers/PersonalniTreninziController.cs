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
            => _context = context;

        // GET: PersonalniTreninzi
        public async Task<IActionResult> Index()
        {
            var q = _context.PersonalniTreninzi
                .Include(p => p.Trener).ThenInclude(t => t.Zaposlenik)
                .Include(p => p.Klijent);
            return View(await q.ToListAsync());
        }

        // GET: PersonalniTreninzi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var p = await _context.PersonalniTreninzi
                .Include(x => x.Trener).ThenInclude(t => t.Zaposlenik)
                .Include(x => x.Klijent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (p == null) return NotFound();
            return View(p);
        }

        // GET: PersonalniTreninzi/Create
        public IActionResult Create()
        {
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                "Id", "Name"
            );
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime");
            return View();
        }

        // POST: PersonalniTreninzi/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,Opis,TrenerId,KlijentId,Datum,Vrijeme,Napredak")] PersonalniTrening p)
        {
            if (ModelState.IsValid)
            {
                _context.Add(p);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                "Id", "Name", p.TrenerId
            );
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", p.KlijentId);
            return View(p);
        }

        // GET: PersonalniTreninzi/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var p = await _context.PersonalniTreninzi.FindAsync(id);
            if (p == null) return NotFound();
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                "Id", "Name", p.TrenerId
            );
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", p.KlijentId);
            return View(p);
        }

        // POST: PersonalniTreninzi/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,TrenerId,KlijentId,Datum,Vrijeme,Napredak")] PersonalniTrening p)
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
                    if (!_context.PersonalniTreninzi.Any(e => e.Id == p.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrenerId = new SelectList(
                _context.Treneri
                    .Include(t => t.Zaposlenik)
                    .Select(t => new { t.Id, Name = t.Zaposlenik.Prezime + ", " + t.Zaposlenik.Ime }),
                "Id", "Name", p.TrenerId
            );
            ViewBag.KlijentId = new SelectList(_context.Klijenti, "Id", "Prezime", p.KlijentId);
            return View(p);
        }

        // GET: PersonalniTreninzi/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var p = await _context.PersonalniTreninzi
                .Include(x => x.Trener).ThenInclude(t => t.Zaposlenik)
                .Include(x => x.Klijent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (p == null) return NotFound();
            return View(p);
        }

        // POST: PersonalniTreninzi/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var p = await _context.PersonalniTreninzi.FindAsync(id);
            _context.PersonalniTreninzi.Remove(p);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
