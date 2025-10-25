using Microsoft.AspNetCore.Mvc;

namespace HealthCalendar.Controllers
{
	public class WorkerController : Controller
	{
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
	}
}
