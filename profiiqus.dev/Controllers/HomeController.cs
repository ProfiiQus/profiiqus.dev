using Microsoft.AspNetCore.Mvc;
using profiiqus.dev.Models;
using System.Diagnostics;

namespace profiiqus.dev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IndexModel _indexModel;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _indexModel = new IndexModel(configuration);
        }

        public IActionResult Index()
        {
            return View(_indexModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}