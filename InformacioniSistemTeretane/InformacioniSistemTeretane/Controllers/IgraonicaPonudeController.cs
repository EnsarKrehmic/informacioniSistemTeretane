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
    public class IgraonicaPonudeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IgraonicaPonudeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public IgraonicaPonudeController(
            ApplicationDbContext context,
            ILogger<IgraonicaPonudeController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: IgraonicaPonude/Index
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Korisnik {UserName} pregleda ponude igraonica", User.Identity.Name);

            var ponude = await _context.IgraonicaPonude
                .Include(p => p.Igraonica)
                .ToListAsync();

            return View(ponude);
        }

        // GET: IgraonicaPonude/Details/{id}
        [HttpGet]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj pristupa detaljima ponude bez ID-a (korisnik: {UserName})", User.Identity.Name);
                return NotFound();
            }

            var ponuda = await _context.IgraonicaPonude
                .Include(p => p.Igraonica)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ponuda == null)
            {
                _logger.LogWarning("Ponuda igraonice sa ID-om {PonudaId} nije pronađena (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} pregleda detalje ponude: {PonudaNaziv}",
                User.Identity.Name, ponuda.OpisUsluge);

            return View(ponuda);
        }

        // GET: IgraonicaPonude/Create
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public IActionResult Create()
        {
            _logger.LogInformation("Korisnik {UserName} pokreće kreiranje nove ponude za igraonicu", User.Identity.Name);

            ViewData["IgraonicaId"] = new SelectList(_context.Igraonice, "Id", "Naziv");
            return View();
        }

        // POST: IgraonicaPonude/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Create([Bind("IgraonicaId,OpisUsluge,Cijena,Trajanje")] IgraonicaPonuda ponuda)
        {
            _logger.LogInformation("Korisnik {UserName} kreira novu ponudu za igraonicu", User.Identity.Name);
            _logger.LogDebug("Parametri: IgraonicaId={IgraonicaId}, OpisUsluge={OpisUsluge}, Cijena={Cijena}, Trajanje={Trajanje}",
                ponuda.IgraonicaId, ponuda.OpisUsluge, ponuda.Cijena, ponuda.Trajanje);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Neuspješna validacija za novu ponudu (korisnik: {UserName})", User.Identity.Name);

                ViewData["IgraonicaId"] = new SelectList(_context.Igraonice, "Id", "Naziv", ponuda.IgraonicaId);
                return View(ponuda);
            }

            try
            {
                _context.Add(ponuda);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Ponuda '{OpisUsluge}' uspješno kreirana (ID: {PonudaId}) od strane {UserName}",
                    ponuda.OpisUsluge, ponuda.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Ponuda '{ponuda.OpisUsluge}' je uspješno kreirana!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri čuvanju ponude u bazu (korisnik: {UserName})", User.Identity.Name);
                ModelState.AddModelError("", "Došlo je do greške pri čuvanju podataka. Pokušajte ponovo.");

                ViewData["IgraonicaId"] = new SelectList(_context.Igraonice, "Id", "Naziv", ponuda.IgraonicaId);
                return View(ponuda);
            }
        }

        // GET: IgraonicaPonude/Edit/{id}
        [HttpGet]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj uređivanja ponude bez ID-a (korisnik: {UserName})", User.Identity.Name);
                return NotFound();
            }

            var ponuda = await _context.IgraonicaPonude.FindAsync(id);
            if (ponuda == null)
            {
                _logger.LogWarning("Ponuda igraonice sa ID-om {PonudaId} nije pronađena (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} uređuje ponudu: {PonudaNaziv} (ID: {PonudaId})",
                User.Identity.Name, ponuda.OpisUsluge, ponuda.Id);

            ViewData["IgraonicaId"] = new SelectList(_context.Igraonice, "Id", "Naziv", ponuda.IgraonicaId);
            return View(ponuda);
        }

        // POST: IgraonicaPonude/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Zaposlenik")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IgraonicaId,OpisUsluge,Cijena,Trajanje")] IgraonicaPonuda ponuda)
        {
            _logger.LogInformation("Korisnik {UserName} ažurira ponudu ID: {PonudaId}", User.Identity.Name, id);
            _logger.LogDebug("Parametri: IgraonicaId={IgraonicaId}, OpisUsluge={OpisUsluge}, Cijena={Cijena}, Trajanje={Trajanje}",
                ponuda.IgraonicaId, ponuda.OpisUsluge, ponuda.Cijena, ponuda.Trajanje);

            if (id != ponuda.Id)
            {
                _logger.LogWarning("ID u putanji ({PutanjaId}) i ID ponude ({PonudaId}) se ne podudaraju (korisnik: {UserName})",
                    id, ponuda.Id, User.Identity.Name);
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Neuspješna validacija za ažuriranje ponude (korisnik: {UserName})", User.Identity.Name);

                ViewData["IgraonicaId"] = new SelectList(_context.Igraonice, "Id", "Naziv", ponuda.IgraonicaId);
                return View(ponuda);
            }

            try
            {
                _context.Update(ponuda);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Ponuda '{OpisUsluge}' (ID: {PonudaId}) uspješno ažurirana od strane {UserName}",
                    ponuda.OpisUsluge, ponuda.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Ponuda '{ponuda.OpisUsluge}' je uspješno ažurirana!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_context.IgraonicaPonude.Any(e => e.Id == ponuda.Id))
                {
                    _logger.LogWarning("Ponuda sa ID-om {PonudaId} nije pronađena (korisnik: {UserName})",
                        ponuda.Id, User.Identity.Name);
                    return NotFound();
                }

                _logger.LogError(ex, "Greška pri ažuriranju ponude (ID: {PonudaId})", ponuda.Id);
                ModelState.AddModelError("", "Došlo je do greške pri čuvanju izmjena. Pokušajte ponovo.");

                ViewData["IgraonicaId"] = new SelectList(_context.Igraonice, "Id", "Naziv", ponuda.IgraonicaId);
                return View(ponuda);
            }
        }

        // GET: IgraonicaPonude/Delete/{id}
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Pokušaj brisanja ponude bez ID-a (korisnik: {UserName})", User.Identity.Name);
                return NotFound();
            }

            var ponuda = await _context.IgraonicaPonude
                .Include(p => p.Igraonica)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ponuda == null)
            {
                _logger.LogWarning("Ponuda igraonice sa ID-om {PonudaId} nije pronađena (korisnik: {UserName})", id, User.Identity.Name);
                return NotFound();
            }

            _logger.LogInformation("Korisnik {UserName} pokreće brisanje ponude: {PonudaNaziv} (ID: {PonudaId})",
                User.Identity.Name, ponuda.OpisUsluge, ponuda.Id);

            return View(ponuda);
        }

        // POST: IgraonicaPonude/DeleteConfirmed/{id}
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ponuda = await _context.IgraonicaPonude
                .FirstOrDefaultAsync(p => p.Id == id);

            if (ponuda == null)
            {
                _logger.LogWarning("Ponuda sa ID-om {PonudaId} nije pronađena za brisanje (korisnik: {UserName})",
                    id, User.Identity.Name);
                return NotFound();
            }

            try
            {
                _context.IgraonicaPonude.Remove(ponuda);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Ponuda '{OpisUsluge}' (ID: {PonudaId}) uspješno obrisana od strane {UserName}",
                    ponuda.OpisUsluge, ponuda.Id, User.Identity.Name);

                TempData["Uspjeh"] = $"Ponuda '{ponuda.OpisUsluge}' je uspješno obrisana!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Greška pri brisanju ponude ID: {PonudaId} (korisnik: {UserName})", id, User.Identity.Name);

                TempData["Greska"] = $"Došlo je do greške pri brisanju ponude '{ponuda.OpisUsluge}'!";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
