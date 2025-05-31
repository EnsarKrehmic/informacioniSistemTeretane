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
    [Authorize]
    public class IgraoniceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IgraoniceController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public IgraoniceController(
            ApplicationDbContext context,
            ILogger<IgraoniceController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: Igraonice/Index
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Korisnik {UserName} pregleda igraonice", User.Identity.Name);

            var igraonice = await _context.Igraonice
                .Include(i => i.Lokacija)
                .ToListAsync();

            return View(igraonice);
        }

        // GET: Igraonice/Details/{id}
        [HttpGet]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj pristupa detaljima igraonice bez ID-a (korisnik: {UserName})", User.Identity.Name);
                return NotFound();
            }

            var igraonica = await _context.Igraonice
                .Include(i => i.Lokacija)
                .Include(i => i.Ponude)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (igraonica == null)
            {
                _logger.LogWarning("Igraonica sa ID-om {IgraonicaId} nije pronađena (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} pregleda detalje igraonice: {IgraonicaNaziv}",
                User.Identity.Name, igraonica.Naziv);

            return View(igraonica);
        }

        // GET: Igraonice/Create
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            _logger.LogInformation("Korisnik {UserName} pokreće kreiranje nove igraonice", User.Identity.Name);

            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv");
            return View();
        }

        // POST: Igraonice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Create([Bind("Naziv,Kapacitet,LokacijaId")] Igraonica igraonica)
        {
            _logger.LogInformation("Korisnik {UserName} kreira novu igraonicu", User.Identity.Name);
            _logger.LogDebug("Parametri: Naziv={Naziv}, Kapacitet={Kapacitet}, LokacijaId={LokacijaId}",
                igraonica.Naziv, igraonica.Kapacitet, igraonica.LokacijaId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Neuspješna validacija za novu igraonicu (korisnik: {UserName})", User.Identity.Name);

                ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", igraonica.LokacijaId);
                return View(igraonica);
            }

            try
            {
                _context.Add(igraonica);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Igraonica '{Naziv}' uspješno kreirana (ID: {IgraonicaId}) od strane {UserName}",
                    igraonica.Naziv, igraonica.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Igraonica '{igraonica.Naziv}' je uspješno kreirana!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri čuvanju igraonice u bazu (korisnik: {UserName})", User.Identity.Name);
                ModelState.AddModelError("", "Došlo je do greške pri čuvanju podataka. Pokušajte ponovo.");

                ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", igraonica.LokacijaId);
                return View(igraonica);
            }
        }

        // GET: Igraonice/Edit/{id}
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj uređivanja igraonice bez ID-a (korisnik: {UserName})", User.Identity.Name);
                return NotFound();
            }

            var igraonica = await _context.Igraonice.FindAsync(id);
            if (igraonica == null)
            {
                _logger.LogWarning("Igraonica sa ID-om {IgraonicaId} nije pronađena (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} uređuje igraonicu: {IgraonicaNaziv} (ID: {IgraonicaId})",
                User.Identity.Name, igraonica.Naziv, igraonica.Id);

            ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", igraonica.LokacijaId);
            return View(igraonica);
        }

        // POST: Igraonice/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Kapacitet,LokacijaId")] Igraonica igraonica)
        {
            _logger.LogInformation("Korisnik {UserName} ažurira igraonicu ID: {IgraonicaId}", User.Identity.Name, id);
            _logger.LogDebug("Parametri: Naziv={Naziv}, Kapacitet={Kapacitet}, LokacijaId={LokacijaId}",
                igraonica.Naziv, igraonica.Kapacitet, igraonica.LokacijaId);

            if (id != igraonica.Id)
            {
                _logger.LogWarning("ID u putanji ({PutanjaId}) i ID igraonice ({IgraonicaId}) se ne podudaraju (korisnik: {UserName})",
                    id, igraonica.Id, User.Identity.Name);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Neuspješna validacija za ažuriranje igraonice (korisnik: {UserName})", User.Identity.Name);

                ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", igraonica.LokacijaId);
                return View(igraonica);
            }

            try
            {
                _context.Update(igraonica);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Igraonica '{Naziv}' (ID: {IgraonicaId}) uspješno ažurirana od strane {UserName}",
                    igraonica.Naziv, igraonica.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Igraonica '{igraonica.Naziv}' je uspješno ažurirana!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_context.Igraonice.Any(e => e.Id == igraonica.Id))
                {
                    _logger.LogWarning("Igraonica sa ID-om {IgraonicaId} nije pronađena (korisnik: {UserName})",
                        igraonica.Id, User.Identity.Name);
                    return NotFound();
                }

                _logger.LogError(ex, "Greška pri ažuriranju igraonice (ID: {IgraonicaId})", igraonica.Id);
                ModelState.AddModelError("", "Došlo je do greške pri čuvanju izmjena. Pokušajte ponovo.");

                ViewData["LokacijaId"] = new SelectList(_context.Lokacije, "Id", "Naziv", igraonica.LokacijaId);
                return View(igraonica);
            }
        }

        // GET: Igraonice/Delete/{id}
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj brisanja igraonice bez ID-a (korisnik: {UserName})", User.Identity.Name);
                return NotFound();
            }

            var igraonica = await _context.Igraonice
                .Include(i => i.Lokacija)
                .Include(i => i.Ponude)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (igraonica == null)
            {
                _logger.LogWarning("Igraonica sa ID-om {IgraonicaId} nije pronađena (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} pokreće brisanje igraonice: {IgraonicaNaziv} (ID: {IgraonicaId})",
                User.Identity.Name, igraonica.Naziv, igraonica.Id);

            return View(igraonica);
        }

        // POST: Igraonice/DeleteConfirmed/{id}
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var igraonica = await _context.Igraonice
                .Include(i => i.Ponude)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (igraonica == null)
            {
                _logger.LogWarning("Igraonica sa ID-om {IgraonicaId} nije pronađena za brisanje (korisnik: {UserName})",
                    id, User.Identity.Name);
                return NotFound();
            }

            if (igraonica.Ponude?.Any() == true)
            {
                _logger.LogWarning("Pokušaj brisanja igraonice '{IgraonicaNaziv}' koja ima povezane ponude (korisnik: {UserName})",
                    igraonica.Naziv, User.Identity.Name);

                TempData["Greska"] = $"Ne možete obrisati igraonicu '{igraonica.Naziv}' jer ima povezane ponude!";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Igraonice.Remove(igraonica);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Igraonica '{Naziv}' (ID: {IgraonicaId}) uspješno obrisana od strane {UserName}",
                    igraonica.Naziv, igraonica.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Igraonica '{igraonica.Naziv}' je uspješno obrisana!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju igraonice ID: {IgraonicaId} (korisnik: {UserName})",
                    id, User.Identity.Name);

                TempData["Greska"] = $"Došlo je do greške pri brisanju igraonice '{igraonica.Naziv}'!";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
