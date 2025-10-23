using Microsoft.AspNetCore.Mvc;

namespace HealthCalendar.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();   // will render Views/Home/Index.cshtml (to see the homepage)
        }
    }
}
