using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;
using Microsoft.AspNetCore.Authorization;

namespace InformacioniSistemTeretane.Controllers
{
    [Authorize]
    public class UplateController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UplateController> _logger;

        public UplateController(ApplicationDbContext context, ILogger<UplateController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Uplate
        [HttpGet]
        [Route("")]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation(
                    "Korisnik {Korisnik} pregleda listu uplata",
                    User.Identity.Name
                );

                var uplate = await _context.Uplate
                    .Include(u => u.Klijent)
                    .Include(u => u.Paket)
                    .AsNoTracking()
                    .ToListAsync();

                return View(uplate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška prilikom učitavanja uplata");
                TempData["Greska"] = "Došlo je do greške prilikom učitavanja uplata.";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Uplate/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning(
                    "Pokušaj pregleda detalja uplate bez ID-a od strane {Korisnik}",
                    User.Identity.Name
                );
                return NotFound();
            }

            try
            {
                _logger.LogInformation(
                    "Korisnik {Korisnik} pregleda detalje uplate ID: {UplataId}",
                    User.Identity.Name,
                    id
                );

                var uplata = await _context.Uplate
                    .Include(u => u.Klijent)
                    .Include(u => u.Paket)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (uplata == null)
                {
                    _logger.LogWarning(
                        "Uplata ID {UplataId} nije pronađena. Korisnik: {Korisnik}",
                        id,
                        User.Identity.Name
                    );
                    TempData["Greska"] = "Uplata nije pronađena.";
                    return NotFound();
                }

                return View(uplata);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Greška prilikom učitavanja detalja uplate ID: {UplataId}. Korisnik: {Korisnik}",
                    id,
                    User.Identity.Name
                );
                TempData["Greska"] = "Došlo je do greške prilikom učitavanja detalja uplate.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Uplate/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public IActionResult Create()
        {
            try
            {
                ViewData["KlijentId"] = new SelectList(
                    _context.Klijenti.Select(k => new {
                        k.Id,
                        ImePrezime = $"{k.Ime} {k.Prezime}"
                    }),
                    "Id",
                    "ImePrezime"
                );

                ViewData["PaketId"] = new SelectList(
                    _context.Paketi,
                    "Id",
                    "Naziv"
                );

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška prilikom pripreme forme za kreiranje uplate");
                TempData["Greska"] = "Došlo je do greške prilikom pripreme forme.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Uplate/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Create([Bind("KlijentId,PaketId,DatumUplate,NacinUplate,Iznos")] Uplata u)
        {
            _logger.LogInformation(
                "Korisnik {Korisnik} pokušava kreirati novu uplatu",
                User.Identity.Name
            );

            _logger.LogInformation("Parametri: KlijentId={KlijentId}, PaketId={PaketId}, " +
                "DatumUplate={Datum}, NacinUplate={Nacin}, Iznos={Iznos}",
                u.KlijentId, u.PaketId, u.DatumUplate, u.NacinUplate, u.Iznos);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Nevalidan model za kreiranje uplate");
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        _logger.LogWarning(" - {Key}: {ErrorMessage}", key, error.ErrorMessage);
                    }
                }

                try
                {
                    ViewData["KlijentId"] = new SelectList(
                        _context.Klijenti.Select(k => new {
                            k.Id,
                            ImePrezime = $"{k.Ime} {k.Prezime}"
                        }),
                        "Id",
                        "ImePrezime",
                        u.KlijentId
                    );

                    ViewData["PaketId"] = new SelectList(
                        _context.Paketi,
                        "Id",
                        "Naziv",
                        u.PaketId
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Greška prilikom ponovnog učitavanja padajućih lista");
                    TempData["Greska"] = "Došlo je do greške prilikom pripreme forme.";
                    return RedirectToAction(nameof(Index));
                }

                return View(u);
            }

            try
            {
                _context.Add(u);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Uspješno kreirana uplata ID: {UplataId}",
                    u.Id
                );

                TempData["Uspjeh"] = "Uplata je uspješno kreirana.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Greška prilikom čuvanja uplate u bazu. Korisnik: {Korisnik}",
                    User.Identity.Name
                );

                TempData["Greska"] = "Došlo je do greške prilikom čuvanja uplate u bazu.";

                ViewData["KlijentId"] = new SelectList(
                    _context.Klijenti.Select(k => new {
                        k.Id,
                        ImePrezime = $"{k.Ime} {k.Prezime}"
                    }),
                    "Id",
                    "ImePrezime",
                    u.KlijentId
                );

                ViewData["PaketId"] = new SelectList(
                    _context.Paketi,
                    "Id",
                    "Naziv",
                    u.PaketId
                );

                return View(u);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Neočekivana greška prilikom kreiranja uplate. Korisnik: {Korisnik}",
                    User.Identity.Name
                );

                TempData["Greska"] = "Došlo je do neočekivane greške prilikom kreiranja uplate.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Uplate/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning(
                    "Pokušaj uređivanja uplate bez ID-a od strane {Korisnik}",
                    User.Identity.Name
                );
                return NotFound();
            }

            try
            {
                var u = await _context.Uplate.FindAsync(id);
                if (u == null)
                {
                    _logger.LogWarning(
                        "Uplata ID {UplataId} nije pronađena za uređivanje. Korisnik: {Korisnik}",
                        id,
                        User.Identity.Name
                    );
                    TempData["Greska"] = "Uplata nije pronađena.";
                    return NotFound();
                }

                ViewData["KlijentId"] = new SelectList(
                    _context.Klijenti.Select(k => new {
                        k.Id,
                        ImePrezime = $"{k.Ime} {k.Prezime}"
                    }),
                    "Id",
                    "ImePrezime",
                    u.KlijentId
                );

                ViewData["PaketId"] = new SelectList(
                    _context.Paketi,
                    "Id",
                    "Naziv",
                    u.PaketId
                );

                return View(u);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Greška prilikom učitavanja uplate za uređivanje ID: {UplataId}. Korisnik: {Korisnik}",
                    id,
                    User.Identity.Name
                );

                TempData["Greska"] = "Došlo je do greške prilikom učitavanja uplate za uređivanje.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Uplate/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KlijentId,PaketId,DatumUplate,NacinUplate,Iznos")] Uplata u)
        {
            if (id != u.Id)
            {
                _logger.LogWarning(
                    "ID u putanji ({PutanjaId}) i ID modela ({ModelId}) se ne podudaraju. Korisnik: {Korisnik}",
                    id,
                    u.Id,
                    User.Identity.Name
                );

                return NotFound();
            }

            _logger.LogInformation(
                "Korisnik {Korisnik} pokušava urediti uplatu ID: {UplataId}",
                User.Identity.Name,
                id
            );

            _logger.LogInformation("Parametri: KlijentId={KlijentId}, PaketId={PaketId}, " +
                "DatumUplate={Datum}, NacinUplate={Nacin}, Iznos={Iznos}",
                u.KlijentId, u.PaketId, u.DatumUplate, u.NacinUplate, u.Iznos);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Nevalidan model za uređivanje uplate");
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        _logger.LogWarning(" - {Key}: {ErrorMessage}", key, error.ErrorMessage);
                    }
                }

                try
                {
                    ViewData["KlijentId"] = new SelectList(
                        _context.Klijenti.Select(k => new {
                            k.Id,
                            ImePrezime = $"{k.Ime} {k.Prezime}"
                        }),
                        "Id",
                        "ImePrezime",
                        u.KlijentId
                    );

                    ViewData["PaketId"] = new SelectList(
                        _context.Paketi,
                        "Id",
                        "Naziv",
                        u.PaketId
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Greška prilikom ponovnog učitavanja padajućih lista");
                    TempData["Greska"] = "Došlo je do greške prilikom pripreme forme.";
                    return RedirectToAction(nameof(Index));
                }

                return View(u);
            }

            try
            {
                _context.Update(u);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Uspješno ažurirana uplata ID: {UplataId}",
                    u.Id
                );

                TempData["Uspjeh"] = "Uplata je uspješno ažurirana.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!UplataExists(u.Id))
                {
                    _logger.LogWarning(
                        "Uplata ID {UplataId} više ne postoji u bazi. Korisnik: {Korisnik}",
                        u.Id,
                        User.Identity.Name
                    );

                    TempData["Greska"] = "Uplata više ne postoji u bazi.";
                    return NotFound();
                }
                else
                {
                    _logger.LogError(
                        ex,
                        "Greška prilikom čuvanja izmjena uplate ID: {UplataId}. Korisnik: {Korisnik}",
                        u.Id,
                        User.Identity.Name
                    );

                    TempData["Greska"] = "Došlo je do greške prilikom čuvanja izmjena. Podaci su možda mijenjani od strane drugog korisnika.";

                    ViewData["KlijentId"] = new SelectList(
                        _context.Klijenti.Select(k => new {
                            k.Id,
                            ImePrezime = $"{k.Ime} {k.Prezime}"
                        }),
                        "Id",
                        "ImePrezime",
                        u.KlijentId
                    );

                    ViewData["PaketId"] = new SelectList(
                        _context.Paketi,
                        "Id",
                        "Naziv",
                        u.PaketId
                    );

                    return View(u);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Neočekivana greška prilikom ažuriranja uplate ID: {UplataId}. Korisnik: {Korisnik}",
                    u.Id,
                    User.Identity.Name
                );

                TempData["Greska"] = "Došlo je do neočekivane greške prilikom ažuriranja uplate.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Uplate/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning(
                    "Pokušaj brisanja uplate bez ID-a od strane {Korisnik}",
                    User.Identity.Name
                );
                return NotFound();
            }

            try
            {
                var u = await _context.Uplate
                    .Include(x => x.Klijent)
                    .Include(x => x.Paket)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (u == null)
                {
                    _logger.LogWarning(
                        "Uplata ID {UplataId} nije pronađena za brisanje. Korisnik: {Korisnik}",
                        id,
                        User.Identity.Name
                    );
                    TempData["Greska"] = "Uplata nije pronađena.";
                    return NotFound();
                }

                return View(u);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Greška prilikom učitavanja uplate za brisanje ID: {UplataId}. Korisnik: {Korisnik}",
                    id,
                    User.Identity.Name
                );

                TempData["Greska"] = "Došlo je do greške prilikom učitavanja uplate za brisanje.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Uplate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]/{id}")]
        [Authorize(Roles = "Admin,Zaposlenik")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var u = await _context.Uplate.FindAsync(id);
                if (u == null)
                {
                    _logger.LogWarning(
                        "Uplata ID {UplataId} nije pronađena za brisanje. Korisnik: {Korisnik}",
                        id,
                        User.Identity.Name
                    );

                    TempData["Greska"] = "Uplata nije pronađena.";
                    return NotFound();
                }

                _context.Uplate.Remove(u);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Uspješno obrisana uplata ID: {UplataId}. Korisnik: {Korisnik}",
                    id,
                    User.Identity.Name
                );

                TempData["Uspjeh"] = "Uplata je uspješno obrisana.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(
                    ex,
                    "Greška prilikom brisanja uplate ID: {UplataId}. Korisnik: {Korisnik}",
                    id,
                    User.Identity.Name
                );

                TempData["Greska"] = "Došlo je do greške prilikom brisanja uplate. Provjerite da li postoje zavisni podaci.";
                return RedirectToAction(nameof(Delete), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Neočekivana greška prilikom brisanja uplate ID: {UplataId}. Korisnik: {Korisnik}",
                    id,
                    User.Identity.Name
                );

                TempData["Greska"] = "Došlo je do neočekivane greške prilikom brisanja uplate.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool UplataExists(int id)
        {
            return _context.Uplate.Any(e => e.Id == id);
        }
    }
}