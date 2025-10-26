using Microsoft.AspNetCore.Mvc;
using HealthCalendar.Services;
using HealthCalendar.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HealthCalendar.Controllers
{
	public class EventController : Controller
	{
		   // (keep only one field and constructor)

		   // (keep only one PatientEvents and AddEvent method)

		   // POST: Event/Create
		   [HttpPost]
		   [ValidateAntiForgeryToken]
		   public async Task<IActionResult> Create(HealthCalendar.ViewModels.EventFormViewModel model)
		   {
			   if (!ModelState.IsValid)
			   {
				   // Repopulate AvailableDates if validation fails
				   model.AvailableDates = new List<DateOnly> { DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(1)) };
				   return View("CreateEvent", model);
			   }

			   // Get patientId from session
			   if (HttpContext.Session.GetInt32("PatientId") is not int patientId)
			   {
				   return RedirectToAction("Login", "Patient");
			   }

			   model.Event.PatientId = patientId;
			   var status = await _eventService.AddEvent(model.Event);
			   if (status == HealthCalendar.Shared.OperationStatus.Success)
			   {
				   return RedirectToAction("PatientEvents");
			   }

			   ModelState.AddModelError("", "Could not create event. Please try again.");
			   model.AvailableDates = new List<DateOnly> { DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(1)) };
			   return View("CreateEvent", model);
		   }
		private readonly IEventService _eventService;
		public EventController(IEventService eventService)
		{
			_eventService = eventService;
		}

		// Viser alle events for en pasient (krever ekte innlogging/session for å hente patientId)
		public IActionResult PatientEvents()
		{
			// TODO: Hent patientId fra session eller brukercontext når innlogging er på plass
			return View(new List<Event>()); // Midlertidig: returnerer tom liste
		}

		// GET: Event/AddEvent
		[HttpGet]
		public IActionResult AddEvent()
		{
			// Populate with dummy dates for now, or fetch from service if needed
			var model = new HealthCalendar.ViewModels.EventFormViewModel
			{
				AvailableDates = new List<DateOnly> { DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(1)) }
			};
			return View("CreateEvent", model);
		}

		// GET: Event/WorkerEvents
		public async Task<IActionResult> WorkerEvents()
		{
			if (HttpContext.Session.GetInt32("WorkerId") is not int workerId)
			{
				return RedirectToAction("Login", "Worker");
			}

			var (events, patients, status) = await _eventService.GetAssignedEvents(workerId);

			if (status == Shared.OperationStatus.Success && events != null)
			{
				return View(events);
			}
			
			return View(new List<Event>());
		}

        // GET: Event/WorkerAvailability
        public IActionResult WorkerAvailability()
        {
            // Midlertidig test data
            var availableDates = new List<DateOnly>
            {
                new DateOnly(2025, 10, 25),
                new DateOnly(2025, 10, 26),
                new DateOnly(2025, 10, 28)
            };
            
            return View(availableDates);
        }
	}
}