using Microsoft.AspNetCore.Mvc;
using profiiqus.dev.Models;
using System.Diagnostics;

namespace profiiqus.dev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private GitHubAPIModel _gitHubAPIModel;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _gitHubAPIModel = new GitHubAPIModel(configuration);
        }

        public IActionResult Index()
        {
            return View(_gitHubAPIModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}