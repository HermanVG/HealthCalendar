using Microsoft.AspNetCore.Mvc;
using HealthCalendar.Services;
using HealthCalendar.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using HealthCalendar.DAL;
using HealthCalendar.Shared;

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
		private readonly IWorkerAvailabilityRepo _availabilityRepo;

		public EventController(IEventService eventService, IWorkerAvailabilityRepo availabilityRepo)
		{
			_eventService = eventService;
			_availabilityRepo = availabilityRepo;
		}

		// Viser alle events for en pasient (krever ekte innlogging/session for å hente patientId)
		public async Task<IActionResult> PatientEvents()
		{
			// Check if patient is logged in
			if (HttpContext.Session.GetInt32("PatientId") is not int patientId)
			{
				return RedirectToAction("Login", "Patient");
			}

			// Get events for the patient
			var (events, status) = await _eventService.GetEventsForPatient(patientId);
			
			if (status == OperationStatus.Success && events != null)
			{
				return View(events);
			}
			
			return View(new List<Event>());
		}

		// GET: Event/AddEvent
		[HttpGet]
		public async Task<IActionResult> AddEvent()
		{
			// Check for PatientId in session
			if (HttpContext.Session.GetInt32("PatientId") is not int patientId)
			{
				return RedirectToAction("Login", "Patient");
			}

			// Get all worker availabilities from db
			var (availability, status) = await _eventService.AddEvent(patientId);
			
			var model = new HealthCalendar.ViewModels.EventFormViewModel
			{
				AvailableDates = availability?.Select(a => a.Date).ToList() ?? new List<DateOnly>()
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
		public async Task<IActionResult> WorkerAvailability()
		{
			// Check if worker is logged in
			if (HttpContext.Session.GetInt32("WorkerId") is not int workerId)
			{
				return RedirectToAction("Login", "Worker");
			}

			// Get worker availability from repository
			var (availability, status) = await _availabilityRepo.GetAvailability(workerId);

			// If success, return availability list
			if (status == OperationStatus.Success) return View(availability);

			// If error occurred, return empty list
			return View(new List<DateOnly>());
		}
		
		// GET: Event/DeleteAvailability
		// TODO: Mangler logging / error handling på flere av funksjonene her
		[HttpPost]
		public async Task<IActionResult> DeleteAvailability(int availabilityId)
		{
			// Check if worker is logged in
			if (HttpContext.Session.GetInt32("WorkerId") is not int workerId)
			{
				return RedirectToAction("Login", "Worker");
			}

			// Call repo to get singular availability
			var (availability, status) = await _availabilityRepo.GetSignularAvailability(availabilityId);

			// Call repo to delete availability
			if (status == OperationStatus.Success && availability != null)
			{
				await _availabilityRepo.DeleteAvailability(availability);
			}

			return RedirectToAction(nameof(WorkerAvailability));
		}

		// GET: Event/AddAvailability
		[HttpPost]
		public async Task<IActionResult> AddAvailability(DateTime date)
		{
			// Check if worker is logged in
			var workerId = HttpContext.Session.GetInt32("WorkerId");
			if (!workerId.HasValue)
			{
				return RedirectToAction("Login", "Worker");
			}

			// Creates availability object with date
			var availability = new WorkerAvailability
			{
				WorkerId = workerId.Value,
				Date = DateOnly.FromDateTime(date)
			};

			await _availabilityRepo.AddAvailability(availability);
			return RedirectToAction("WorkerAvailability");
		}
	}
}