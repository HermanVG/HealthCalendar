using Microsoft.AspNetCore.Mvc;

namespace HealthCalendar.Controllers
{
	public class EventController : Controller
	{
		// Action to show all events for the logged-in patient (no authentication required for now)
		public IActionResult PatientEvents()
		{
			// Normally you would fetch events for the logged-in patient here
			// For now, just return the view (empty or with test data)
			return View("~/Views/Event/PatientEvents.cshtml");
		}
	}
}
