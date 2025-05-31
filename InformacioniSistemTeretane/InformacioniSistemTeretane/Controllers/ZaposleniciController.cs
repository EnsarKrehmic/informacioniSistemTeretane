using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;

namespace InformacioniSistemTeretane.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[Controller]/[Action]")]
    public class ZaposleniciController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ZaposleniciController> _logger;

        public ZaposleniciController(
            ApplicationDbContext context,
            ILogger<ZaposleniciController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation(
                    "Admin {Admin} pregleda listu zaposlenika",
                    User.Identity.Name
                );

                var zaposleni = await _context.Zaposlenici
                    .Include(z => z.User)
                    .AsNoTracking()
                    .ToListAsync();

                return View(zaposleni);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška prilikom učitavanja zaposlenika");
                TempData["Greska"] = "Došlo je do greške prilikom učitavanja podataka o zaposlenicima.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Route("{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning(
                    "Pokušaj pregleda detalja zaposlenika bez ID-a od strane {Admin}",
                    User.Identity.Name
                );
                return NotFound();
            }

            try
            {
                _logger.LogInformation(
                    "Admin {Admin} pregleda detalje zaposlenika ID: {ZaposlenikId}",
                    User.Identity.Name,
                    id
                );

                var zap = await _context.Zaposlenici
                    .Include(z => z.User)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (zap == null)
                {
                    _logger.LogWarning(
                        "Zaposlenik ID {ZaposlenikId} nije pronađen. Admin: {Admin}",
                        id,
                        User.Identity.Name
                    );
                    TempData["Greska"] = "Zaposlenik nije pronađen.";
                    return NotFound();
                }

                return View(zap);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Greška prilikom učitavanja detalja zaposlenika ID: {ZaposlenikId}. Admin: {Admin}",
                    id,
                    User.Identity.Name
                );
                TempData["Greska"] = "Došlo je do greške prilikom učitavanja detalja zaposlenika.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                // Prikaz samo korisnika koji još nisu zaposleni
                var slobodniKorisnici = _context.Users
                    .Where(u => !_context.Zaposlenici.Any(z => z.UserId == u.Id))
                    .Select(u => new {
                        u.Id,
                        KorisnickoIme = u.UserName
                    });

                ViewData["UserId"] = new SelectList(slobodniKorisnici, "Id", "KorisnickoIme");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška prilikom pripreme forme za dodavanje zaposlenika");
                TempData["Greska"] = "Došlo je do greške prilikom pripreme forme.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ime,Prezime,Pozicija,Telefon,UserId")] Zaposlenik zap)
        {
            _logger.LogInformation(
                "Admin {Admin} pokušava dodati novog zaposlenika",
                User.Identity.Name
            );

            _logger.LogInformation("Parametri: Ime={Ime}, Prezime={Prezime}, Pozicija={Pozicija}, Telefon={Telefon}, UserId={UserId}",
                zap.Ime, zap.Prezime, zap.Pozicija, zap.Telefon, zap.UserId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Nevalidan model za dodavanje zaposlenika");
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        _logger.LogWarning(" - {Key}: {ErrorMessage}", key, error.ErrorMessage);
                    }
                }

                try
                {
                    var slobodniKorisnici = _context.Users
                        .Where(u => !_context.Zaposlenici.Any(z => z.UserId == u.Id))
                        .Select(u => new {
                            u.Id,
                            KorisnickoIme = u.UserName
                        });

                    ViewData["UserId"] = new SelectList(slobodniKorisnici, "Id", "KorisnickoIme", zap.UserId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Greška prilikom ponovnog učitavanja padajućih lista");
                    TempData["Greska"] = "Došlo je do greške prilikom pripreme forme.";
                    return RedirectToAction(nameof(Index));
                }

                return View(zap);
            }

            try
            {
                // Provjera da li je korisnik već zaposlen
                if (await _context.Zaposlenici.AnyAsync(z => z.UserId == zap.UserId))
                {
                    ModelState.AddModelError("UserId", "Ovaj korisnik je već zaposlen.");

                    var slobodniKorisnici = _context.Users
                        .Where(u => !_context.Zaposlenici.Any(z => z.UserId == u.Id))
                        .Select(u => new {
                            u.Id,
                            KorisnickoIme = u.UserName
                        });

                    ViewData["UserId"] = new SelectList(slobodniKorisnici, "Id", "KorisnickoIme", zap.UserId);

                    TempData["Greska"] = "Ovaj korisnik je već zaposlen.";
                    return View(zap);
                }

                _context.Add(zap);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Uspješno dodan zaposlenik ID: {ZaposlenikId}",
                    zap.Id
                );

                TempData["Uspjeh"] = "Zaposlenik je uspješno dodan.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Greška prilikom čuvanja zaposlenika u bazu. Admin: {Admin}",
                    User.Identity.Name
                );

                TempData["Greska"] = "Došlo je do greške prilikom čuvanja podataka.";

                var slobodniKorisnici = _context.Users
                    .Where(u => !_context.Zaposlenici.Any(z => z.UserId == u.Id))
                    .Select(u => new {
                        u.Id,
                        KorisnickoIme = u.UserName
                    });

                ViewData["UserId"] = new SelectList(slobodniKorisnici, "Id", "KorisnickoIme", zap.UserId);

                return View(zap);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Neočekivana greška prilikom dodavanja zaposlenika. Admin: {Admin}",
                    User.Identity.Name
                );

                TempData["Greska"] = "Došlo je do neočekivane greške prilikom dodavanja zaposlenika.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Route("{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning(
                    "Pokušaj uređivanja zaposlenika bez ID-a od strane {Admin}",
                    User.Identity.Name
                );
                return NotFound();
            }

            try
            {
                var zap = await _context.Zaposlenici.FindAsync(id);
                if (zap == null)
                {
                    _logger.LogWarning(
                        "Zaposlenik ID {ZaposlenikId} nije pronađen za uređivanje. Admin: {Admin}",
                        id,
                        User.Identity.Name
                    );
                    TempData["Greska"] = "Zaposlenik nije pronađen.";
                    return NotFound();
                }

                // Prikaz svih korisnika, ali označavanje trenutnog
                var korisnici = _context.Users
                    .Select(u => new {
                        u.Id,
                        KorisnickoIme = u.UserName
                    });

                ViewData["UserId"] = new SelectList(korisnici, "Id", "KorisnickoIme", zap.UserId);
                return View(zap);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Greška prilikom učitavanja zaposlenika za uređivanje ID: {ZaposlenikId}. Admin: {Admin}",
                    id,
                    User.Identity.Name
                );

                TempData["Greska"] = "Došlo je do greške prilikom učitavanja podataka za uređivanje.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Prezime,Pozicija,Telefon,UserId")] Zaposlenik zap)
        {
            if (id != zap.Id)
            {
                _logger.LogWarning(
                    "ID u putanji ({PutanjaId}) i ID modela ({ModelId}) se ne podudaraju. Admin: {Admin}",
                    id,
                    zap.Id,
                    User.Identity.Name
                );

                return NotFound();
            }

            _logger.LogInformation(
                "Admin {Admin} pokušava urediti zaposlenika ID: {ZaposlenikId}",
                User.Identity.Name,
                id
            );

            _logger.LogInformation("Parametri: Ime={Ime}, Prezime={Prezime}, Pozicija={Pozicija}, Telefon={Telefon}, UserId={UserId}",
                zap.Ime, zap.Prezime, zap.Pozicija, zap.Telefon, zap.UserId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Nevalidan model za uređivanje zaposlenika");
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        _logger.LogWarning(" - {Key}: {ErrorMessage}", key, error.ErrorMessage);
                    }
                }

                try
                {
                    var korisnici = _context.Users
                        .Select(u => new {
                            u.Id,
                            KorisnickoIme = u.UserName
                        });

                    ViewData["UserId"] = new SelectList(korisnici, "Id", "KorisnickoIme", zap.UserId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Greška prilikom ponovnog učitavanja padajućih lista");
                    TempData["Greska"] = "Došlo je do greške prilikom pripreme forme.";
                    return RedirectToAction(nameof(Index));
                }

                return View(zap);
            }

            try
            {
                // Provjera da li je korisnik već zaposlen (osim trenutnog zaposlenika)
                if (await _context.Zaposlenici.AnyAsync(z => z.UserId == zap.UserId && z.Id != zap.Id))
                {
                    ModelState.AddModelError("UserId", "Ovaj korisnik je već zaposlen.");

                    var korisnici = _context.Users
                        .Select(u => new {
                            u.Id,
                            KorisnickoIme = u.UserName
                        });

                    ViewData["UserId"] = new SelectList(korisnici, "Id", "KorisnickoIme", zap.UserId);

                    TempData["Greska"] = "Ovaj korisnik je već zaposlen.";
                    return View(zap);
                }

                _context.Update(zap);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Uspješno ažuriran zaposlenik ID: {ZaposlenikId}",
                    zap.Id
                );

                TempData["Uspjeh"] = "Podaci o zaposleniku su uspješno ažurirani.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ZaposlenikExists(zap.Id))
                {
                    _logger.LogWarning(
                        "Zaposlenik ID {ZaposlenikId} više ne postoji u bazi. Admin: {Admin}",
                        zap.Id,
                        User.Identity.Name
                    );

                    TempData["Greska"] = "Zaposlenik više ne postoji u bazi.";
                    return NotFound();
                }
                else
                {
                    _logger.LogError(
                        ex,
                        "Greška prilikom čuvanja izmjena zaposlenika ID: {ZaposlenikId}. Admin: {Admin}",
                        zap.Id,
                        User.Identity.Name
                    );

                    TempData["Greska"] = "Došlo je do greške prilikom čuvanja izmjena. Podaci su možda mijenjani od strane drugog administratora.";

                    var korisnici = _context.Users
                        .Select(u => new {
                            u.Id,
                            KorisnickoIme = u.UserName
                        });

                    ViewData["UserId"] = new SelectList(korisnici, "Id", "KorisnickoIme", zap.UserId);

                    return View(zap);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Neočekivana greška prilikom ažuriranja zaposlenika ID: {ZaposlenikId}. Admin: {Admin}",
                    zap.Id,
                    User.Identity.Name
                );

                TempData["Greska"] = "Došlo je do neočekivane greške prilikom ažuriranja podataka o zaposleniku.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Route("{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning(
                    "Pokušaj brisanja zaposlenika bez ID-a od strane {Admin}",
                    User.Identity.Name
                );
                return NotFound();
            }

            try
            {
                var zap = await _context.Zaposlenici
                    .Include(z => z.User)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (zap == null)
                {
                    _logger.LogWarning(
                        "Zaposlenik ID {ZaposlenikId} nije pronađen za brisanje. Admin: {Admin}",
                        id,
                        User.Identity.Name
                    );
                    TempData["Greska"] = "Zaposlenik nije pronađen.";
                    return NotFound();
                }

                return View(zap);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Greška prilikom učitavanja zaposlenika za brisanje ID: {ZaposlenikId}. Admin: {Admin}",
                    id,
                    User.Identity.Name
                );

                TempData["Greska"] = "Došlo je do greške prilikom učitavanja podataka za brisanje.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var zap = await _context.Zaposlenici.FindAsync(id);
                if (zap == null)
                {
                    _logger.LogWarning(
                        "Zaposlenik ID {ZaposlenikId} nije pronađen za brisanje. Admin: {Admin}",
                        id,
                        User.Identity.Name
                    );

                    TempData["Greska"] = "Zaposlenik nije pronađen.";
                    return NotFound();
                }

                _context.Zaposlenici.Remove(zap);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Uspješno obrisan zaposlenik ID: {ZaposlenikId}. Admin: {Admin}",
                    id,
                    User.Identity.Name
                );

                TempData["Uspjeh"] = "Zaposlenik je uspješno obrisan.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Greška prilikom brisanja zaposlenika ID: {ZaposlenikId}. Admin: {Admin}",
                    id,
                    User.Identity.Name
                );

                TempData["Greska"] = "Došlo je do greške prilikom brisanja zaposlenika. Provjerite da li postoje zavisni podaci.";
                return RedirectToAction(nameof(Delete), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Neočekivana greška prilikom brisanja zaposlenika ID: {ZaposlenikId}. Admin: {Admin}",
                    id,
                    User.Identity.Name
                );

                TempData["Greska"] = "Došlo je do neočekivane greške prilikom brisanja zaposlenika.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ZaposlenikExists(int id)
        {
            return _context.Zaposlenici.Any(e => e.Id == id);
        }
    }
}