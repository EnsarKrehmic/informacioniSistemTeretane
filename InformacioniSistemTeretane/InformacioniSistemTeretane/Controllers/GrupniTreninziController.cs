using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace InformacioniSistemTeretane.Controllers
{
    public class GrupniTreninziController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GrupniTreninziController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public GrupniTreninziController(
            ApplicationDbContext context,
            ILogger<GrupniTreninziController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: GrupniTreninzi
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Korisnik {UserName} pregleda grupne treninge", User.Identity.Name);

            var treninzi = await _context.Set<GrupniTrening>()
                .Include(g => g.Sala)
                .Include(g => g.Trener).ThenInclude(t => t.Zaposlenik)
                .ToListAsync();

            return View(treninzi);
        }

        // GET: GrupniTreninzi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj pristupa detaljima treninga bez ID-a (korisnik: {UserName})", User.Identity.Name);
                return NotFound();
            }

            var trening = await _context.Set<GrupniTrening>()
                .Include(g => g.Sala)
                .Include(g => g.Trener).ThenInclude(t => t.Zaposlenik)
                .Include(g => g.Rezervacije).ThenInclude(r => r.Klijent)
                .Include(g => g.Prisustva)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trening == null)
            {
                _logger.LogWarning("Grupni trening sa ID-om {TreningId} nije pronađen (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} pregleda detalje treninga: {TreningNaziv}",
                User.Identity.Name, trening.Naziv);

            return View(trening);
        }

        // GET: GrupniTreninzi/Create
        [Authorize(Roles = "Admin, Zaposlenik")]
        public IActionResult Create()
        {
            _logger.LogInformation("Korisnik {UserName} pokreće kreiranje novog grupnog treninga", User.Identity.Name);

            ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv");
            ViewData["TrenerId"] = new SelectList(_context.Treneri
                .Include(t => t.Zaposlenik)
                .Select(t => new {
                    t.Id,
                    Name = $"{t.Zaposlenik.Prezime}, {t.Zaposlenik.Ime}"
                }), "Id", "Name");

            return View();
        }

        // POST: GrupniTreninzi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Zaposlenik")]
        public async Task<IActionResult> Create(GrupniTrening g)
        {
            _logger.LogInformation("Korisnik {UserName} kreira novi grupni trening", User.Identity.Name);
            _logger.LogDebug("Parametri: Naziv={Naziv}, SalaId={SalaId}, TrenerId={TrenerId}, Datum={Datum}, Vrijeme={Vrijeme}, MaxUcesnika={MaxUcesnika}",
                g.Naziv, g.SalaId, g.TrenerId, g.Datum, g.Vrijeme, g.MaxUcesnika);

            g.VrstaTreninga = "Grupni";

            ModelState.Remove(nameof(g.VrstaTreninga));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Neuspješna validacija za novi grupni trening (korisnik: {UserName})", User.Identity.Name);

                // Napuni ViewData/SelectList ponovno
                ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv", g.SalaId);
                ViewData["TrenerId"] = new SelectList(
                    _context.Treneri.Include(t => t.Zaposlenik)
                        .Select(t => new { t.Id, Name = $"{t.Zaposlenik.Prezime}, {t.Zaposlenik.Ime}" }),
                    "Id", "Name", g.TrenerId);
                return View(g);
            }

            try
            {
                _context.Add(g);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Grupni trening {Naziv} uspješno kreiran (ID: {TreningId}) od strane {UserName}",
                    g.Naziv, g.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Grupni trening '{g.Naziv}' je uspješno kreiran!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri čuvanju grupnog treninga u bazu (korisnik: {UserName})", User.Identity.Name);
                ModelState.AddModelError("", "Došlo je do greške pri čuvanju podataka. Pokušajte ponovo.");

                ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv", g.SalaId);
                ViewData["TrenerId"] = new SelectList(
                    _context.Treneri.Include(t => t.Zaposlenik)
                        .Select(t => new { t.Id, Name = $"{t.Zaposlenik.Prezime}, {t.Zaposlenik.Ime}" }),
                    "Id", "Name", g.TrenerId);

                return View(g);
            }
        }

        // GET: GrupniTreninzi/Edit/5
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj uređivanja treninga bez ID-a (korisnik: {UserName})", User.Identity.Name);
                return NotFound();
            }

            var g = await _context.Set<GrupniTrening>().FindAsync(id);
            if (g == null)
            {
                _logger.LogWarning("Grupni trening sa ID-om {TreningId} nije pronađen (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} uređuje grupni trening: {TreningNaziv} (ID: {TreningId})",
                User.Identity.Name, g.Naziv, g.Id);

            ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv", g.SalaId);
            ViewData["TrenerId"] = new SelectList(_context.Treneri
                .Include(t => t.Zaposlenik)
                .Select(t => new {
                    t.Id,
                    Name = $"{t.Zaposlenik.Prezime}, {t.Zaposlenik.Ime}"
                }), "Id", "Name", g.TrenerId);

            return View(g);
        }

        // POST: GrupniTreninzi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Opis,SalaId,TrenerId,Datum,Vrijeme,MaxUcesnika")] GrupniTrening g)
        {
            _logger.LogInformation("Korisnik {UserName} ažurira grupni trening ID: {TreningId}", User.Identity.Name, id);
            _logger.LogDebug("Parametri: Naziv={Naziv}, SalaId={SalaId}, TrenerId={TrenerId}, Datum={Datum}, Vrijeme={Vrijeme}, MaxUcesnika={MaxUcesnika}",
                g.Naziv, g.SalaId, g.TrenerId, g.Datum, g.Vrijeme, g.MaxUcesnika);

            if (id != g.Id)
            {
                _logger.LogWarning("ID u putanji ({PutanjaId}) i ID treninga ({TreningId}) se ne podudaraju (korisnik: {UserName})",
                    id, g.Id, User.Identity.Name);
                return NotFound();
            }

            g.VrstaTreninga = "Grupni";

            ModelState.Remove(nameof(g.VrstaTreninga));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Neuspješna validacija za ažuriranje treninga (korisnik: {UserName})", User.Identity.Name);

                ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv", g.SalaId);
                ViewData["TrenerId"] = new SelectList(
                    _context.Treneri.Include(t => t.Zaposlenik)
                        .Select(t => new { t.Id, Name = $"{t.Zaposlenik.Prezime}, {t.Zaposlenik.Ime}" }),
                    "Id", "Name", g.TrenerId);
                return View(g);
            }

            try
            {
                _context.Update(g);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Grupni trening {Naziv} (ID: {TreningId}) uspješno ažuriran od strane {UserName}",
                    g.Naziv, g.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Grupni trening '{g.Naziv}' je uspješno ažuriran!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_context.Set<GrupniTrening>().Any(e => e.Id == g.Id))
                {
                    _logger.LogWarning("Grupni trening sa ID-om {TreningId} nije pronađen (korisnik: {UserName})",
                        g.Id, User.Identity.Name);
                    return NotFound();
                }

                _logger.LogError(ex, "Greška pri ažuriranju treninga (ID: {TreningId})", g.Id);
                ModelState.AddModelError("", "Došlo je do greške pri čuvanju izmjena. Pokušajte ponovo.");

                ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv", g.SalaId);
                ViewData["TrenerId"] = new SelectList(
                    _context.Treneri.Include(t => t.Zaposlenik)
                        .Select(t => new { t.Id, Name = $"{t.Zaposlenik.Prezime}, {t.Zaposlenik.Ime}" }),
                    "Id", "Name", g.TrenerId);

                return View(g);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri ažuriranju treninga (ID: {TreningId})", g.Id);
                ModelState.AddModelError("", "Došlo je do greške pri čuvanju izmjena. Pokušajte ponovo.");

                ViewData["SalaId"] = new SelectList(_context.Sale, "Id", "Naziv", g.SalaId);
                ViewData["TrenerId"] = new SelectList(
                    _context.Treneri.Include(t => t.Zaposlenik)
                        .Select(t => new { t.Id, Name = $"{t.Zaposlenik.Prezime}, {t.Zaposlenik.Ime}" }),
                    "Id", "Name", g.TrenerId);

                return View(g);
            }
        }

        // GET: GrupniTreninzi/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj brisanja treninga bez ID-a (korisnik: {UserName})", User.Identity.Name);
                return NotFound();
            }

            var g = await _context.Set<GrupniTrening>()
                .Include(g => g.Sala)
                .Include(g => g.Trener).ThenInclude(t => t.Zaposlenik)
                .Include(g => g.Rezervacije)
                .Include(g => g.Prisustva)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (g == null)
            {
                _logger.LogWarning("Grupni trening sa ID-om {TreningId} nije pronađen (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} pokreće brisanje treninga: {TreningNaziv} (ID: {TreningId})",
                User.Identity.Name, g.Naziv, g.Id);

            return View(g);
        }

        // POST: GrupniTreninzi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var g = await _context.Set<GrupniTrening>()
                .Include(g => g.Rezervacije)
                .Include(g => g.Prisustva)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (g == null)
            {
                _logger.LogWarning("Grupni trening sa ID-om {TreningId} nije pronađen za brisanje (korisnik: {UserName})",
                    id, User.Identity.Name);
                return NotFound();
            }

            // Provjera zavisnosti
            if (g.Rezervacije?.Any() == true || g.Prisustva?.Any() == true)
            {
                _logger.LogWarning("Pokušaj brisanja treninga {TreningNaziv} sa vezanim rezervacijama ili prisustvima (korisnik: {UserName})",
                    g.Naziv, User.Identity.Name);

                TempData["Greska"] = $"Ne možete obrisati trening '{g.Naziv}' jer postoje vezane rezervacije ili evidencije prisustva!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Set<GrupniTrening>().Remove(g);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Grupni trening {Naziv} (ID: {TreningId}) uspješno obrisan od strane {UserName}",
                    g.Naziv, g.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Grupni trening '{g.Naziv}' je uspješno obrisan!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju treninga ID: {TreningId} (korisnik: {UserName})",
                    id, User.Identity.Name);

                TempData["Greska"] = $"Došlo je do greške pri brisanju treninga '{g.Naziv}'!";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}