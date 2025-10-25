using Microsoft.AspNetCore.Mvc;

namespace HealthCalendar.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}