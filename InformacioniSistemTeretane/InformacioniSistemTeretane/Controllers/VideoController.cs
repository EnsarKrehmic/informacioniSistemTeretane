using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using InformacioniSistemTeretane.Data;
using InformacioniSistemTeretane.Models;

namespace InformacioniSistemTeretane.Controllers
{
    public class VideoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VideoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Video/Index (za listu videa)
        [HttpGet]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.VideoSadrzaji.ToListAsync());
        }

        // GET: Video/Embed/{id} (za embedovanje na druge stranice)
        [HttpGet]
        [Route("[controller]/[action]/{id}")]
        public async Task<IActionResult> Embed(int id)
        {
            var video = await _context.VideoSadrzaji.FindAsync(id);
            if (video == null)
                return NotFound();

            return View(video);
        }
    }
}