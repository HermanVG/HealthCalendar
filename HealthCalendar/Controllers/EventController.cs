using Microsoft.AspNetCore.Mvc;

using HealthCalendar.ViewModels;
using System.Collections.Generic;
using System;

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

		// GET: /Event/AddEvent
		[HttpGet]
		public IActionResult AddEvent()
		{
			// Dummy available dates for now
			var model = new EventFormViewModel
			{
				AvailableDates = new List<DateOnly> { DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(1)) }
			};
			return View("~/Views/Event/CreateEvent.cshtml", model);
		}
	}
}
