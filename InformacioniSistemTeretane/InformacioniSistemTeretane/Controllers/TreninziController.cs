// Controllers/TreninziController.cs
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;

public class TreninziController : Controller
{
    private readonly ApplicationDbContext _context;
    public TreninziController(ApplicationDbContext context)
        => _context = context;

    // GET: Treninzi
    public async Task<IActionResult> Index()
    {
        // Učitaj sve treninge bez obzira na vrstu
        var treninzi = await _context.Treninzi.ToListAsync();
        return View(treninzi);
    }

    // GET: Treninzi/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var trening = await _context.Treninzi
            .FirstOrDefaultAsync(t => t.Id == id);
        if (trening == null) return NotFound();
        return View(trening);
    }
}
