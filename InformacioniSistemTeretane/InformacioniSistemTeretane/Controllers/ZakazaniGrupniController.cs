using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace InformacioniSistemTeretane.Controllers
{
    [Authorize]
    public class ZakazaniGrupniController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ZakazaniGrupniController(ApplicationDbContext ctx) => _context = ctx;

        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            var q = _context.ZakazaniGrupni
                .Include(z => z.GrupniTrening)
                .Include(z => z.Klijent);
            return View(await q.ToListAsync());
        }

        [HttpGet]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var zap = await _context.ZakazaniGrupni
                .Include(z => z.GrupniTrening)
                .Include(z => z.Klijent)
                .FirstOrDefaultAsync(z => z.Id == id);
            if (zap == null) return NotFound();
            return View(zap);
        }

        [HttpGet]
        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            ViewData["GrupniTreningId"] = new SelectList(_context.GrupniTreninzi, "Id", "Naziv");
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Create([Bind("GrupniTreningId,KlijentId,DatumPrijave")] ZakazaniGrupni z)
        {
            if (ModelState.IsValid)
            {
                _context.Add(z);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GrupniTreningId"] = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", z.GrupniTreningId);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", z.KlijentId);
            return View(z);
        }

        [HttpGet]
        [Route("[Controller]/[Action]/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var z = await _context.ZakazaniGrupni.FindAsync(id);
            if (z == null) return NotFound();
            ViewData["GrupniTreningId"] = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", z.GrupniTreningId);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", z.KlijentId);
            return View(z);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GrupniTreningId,KlijentId,DatumPrijave")] ZakazaniGrupni z)
        {
            if (id != z.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(z);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GrupniTreningId"] = new SelectList(_context.GrupniTreninzi, "Id", "Naziv", z.GrupniTreningId);
            ViewData["KlijentId"] = new SelectList(_context.Klijenti, "Id", "Prezime", z.KlijentId);
            return View(z);
        }

        [HttpGet]
        [Route("[Controller]/[Action]/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var z = await _context.ZakazaniGrupni
                .Include(zg => zg.GrupniTrening)
                .Include(zg => zg.Klijent)
                .FirstOrDefaultAsync(zg => zg.Id == id);
            if (z == null) return NotFound();
            return View(z);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var z = await _context.ZakazaniGrupni.FindAsync(id);
            _context.ZakazaniGrupni.Remove(z);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}