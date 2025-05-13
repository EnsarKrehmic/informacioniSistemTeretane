using System.Diagnostics;
using InformacioniSistemTeretane.Models;
using Microsoft.AspNetCore.Mvc;

namespace InformacioniSistemTeretane.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        [Route("[Controller]/[Action]")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
