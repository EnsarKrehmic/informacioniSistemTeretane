using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;

namespace InformacioniSistemTeretane.Controllers
{
    public class MapaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MapaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Mapa/Prikazi/{id} (za prikaz pojedinačne mape)
        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Prikazi(int id)
        {
            var mapa = await _context.Mape.FindAsync(id);
            if (mapa == null)
                return NotFound();

            return View(mapa);
        }
    }
}