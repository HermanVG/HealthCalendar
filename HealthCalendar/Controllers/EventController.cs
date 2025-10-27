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
		   // POST: Event/Create
		   [HttpPost]
		   [ValidateAntiForgeryToken]
		   public async Task<IActionResult> Create(HealthCalendar.ViewModels.EventFormViewModel model)
		   {

			   // Ekstra validering: Start < End
			   if (model.Event.Start >= model.Event.End)
			   {
				   // Repopuler AvailableDates fra service hvis validering feiler
				   if (HttpContext.Session.GetInt32("PatientId") is int pid)
				   {
					   var (availability, availStatus) = await _eventService.AddEvent(pid);
					   model.AvailableDates = availability?.Select(a => a.Date).ToList() ?? new List<DateOnly>();
				   }
				   else
				   {
					   model.AvailableDates = new List<DateOnly>();
				   }
				   return View("CreateEvent", model);
			   }

			   if (!ModelState.IsValid)
			   {
				   // Repopuler AvailableDates fra service hvis validering feiler
				   if (HttpContext.Session.GetInt32("PatientId") is int pid)
				   {
					   var (availability, availStatus) = await _eventService.AddEvent(pid);
					   model.AvailableDates = availability?.Select(a => a.Date).ToList() ?? new List<DateOnly>();
				   }
				   else
				   {
					   model.AvailableDates = new List<DateOnly>();
				   }
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
				   TempData["SuccessMessage"] = "Event created successfully!";
				   return RedirectToAction("PatientEvents");
			   }

			   ModelState.AddModelError("", "Could not create event. There must be no overlap with existing events.");
			   // Hent tilgjengelige datoer fra service også ved lagringsfeil
			   if (HttpContext.Session.GetInt32("PatientId") is int patientIdForDates)
			   {
				   var (availability, availStatus) = await _eventService.AddEvent(patientIdForDates);
				   model.AvailableDates = availability?.Select(a => a.Date).ToList() ?? new List<DateOnly>();
			   }
			   else
			   {
				   model.AvailableDates = new List<DateOnly>();
			   }
			   return View("CreateEvent", model);
		   }
		private readonly IEventService _eventService;
		private readonly IWorkerAvailabilityRepo _availabilityRepo;

		public EventController(IEventService eventService, IWorkerAvailabilityRepo availabilityRepo)
		{
			_eventService = eventService;
			_availabilityRepo = availabilityRepo;
		}

		// Viser alle events for en pasient
		public async Task<IActionResult> PatientEvents()
		{
			   // Sjekk at pasient er innlogget
			if (HttpContext.Session.GetInt32("PatientId") is not int patientId)
			{
				return RedirectToAction("Login", "Patient");
			}

			   // Hent events for pasienten
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
			   // Sjekk at pasient er innlogget
			if (HttpContext.Session.GetInt32("PatientId") is not int patientId)
			{
				return RedirectToAction("Login", "Patient");
			}

			   // Hent alle tilgjengelige datoer fra service
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
			   // Sjekk at ansatt er innlogget
			if (HttpContext.Session.GetInt32("WorkerId") is not int workerId)
			{
				return RedirectToAction("Login", "Worker");
			}

			   // Hent tilgjengelighet for ansatt
			var (availability, status) = await _availabilityRepo.GetAvailability(workerId);

			   // Returner tilgjengelighet hvis OK
			if (status == OperationStatus.Success) return View(availability);

			   // Returner tom liste ved feil
			return View(new List<DateOnly>());
		}
		
		// POST: Event/DeleteAvailability
		[HttpPost]
		public async Task<IActionResult> DeleteAvailability(int availabilityId)
		{
			   // Sjekk at ansatt er innlogget
			if (HttpContext.Session.GetInt32("WorkerId") is not int workerId)
			{
				return RedirectToAction("Login", "Worker");
			}

			   // Hent enkelt tilgjengelighetsobjekt
			var (availability, status) = await _availabilityRepo.GetSignularAvailability(availabilityId);

			   // Slett tilgjengelighet hvis funnet
			if (status == OperationStatus.Success && availability != null)
			{
				await _availabilityRepo.DeleteAvailability(availability);
			}

			return RedirectToAction(nameof(WorkerAvailability));
		}

		// POST: Event/AddAvailability
		[HttpPost]
		public async Task<IActionResult> AddAvailability(DateTime date)
		{
			   // Sjekk at ansatt er innlogget
			var workerId = HttpContext.Session.GetInt32("WorkerId");
			if (!workerId.HasValue)
			{
				return RedirectToAction("Login", "Worker");
			}

			   // Opprett tilgjengelighetsobjekt
			var availability = new WorkerAvailability
			{
				WorkerId = workerId.Value,
                Date = DateOnly.FromDateTime(date)
			};

			await _availabilityRepo.AddAvailability(availability);
			return RedirectToAction("WorkerAvailability");
		}

		// GET: Event/EditEvent/{id}
		[HttpGet]
		public async Task<IActionResult> EditEvent(int id)
		{
			// Sjekk at pasient er innlogget
			if (HttpContext.Session.GetInt32("PatientId") is not int patientId)
			{
				return RedirectToAction("Login", "Patient");
			}

			// Hent pasientens WorkerId fra pasientobjektet (kan evt. lagres i session)
			// For enkelhet bruker vi patientId for å hente event og tilgjengelighet
			// (EventService håndterer dette)
			var (eventt, availableDates, status) = await _eventService.UpdateEvent(id, patientId);
			if (status != OperationStatus.Success || eventt == null)
			{
				return RedirectToAction("PatientEvents");
			}

			var model = new HealthCalendar.ViewModels.EventFormViewModel
			{
				Event = eventt,
				AvailableDates = availableDates?.Select(a => a.Date).ToList() ?? new List<DateOnly>()
			};
			return View("EditEvent", model);
		}


		// POST: Event/EditEvent/{id}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditEvent(int id, HealthCalendar.ViewModels.EventFormViewModel model)
		{


			   if (!ModelState.IsValid)
			   {
				   // Repopuler AvailableDates fra service hvis validering feiler
				   if (HttpContext.Session.GetInt32("PatientId") is int pid)
				   {
					   var (availability, availStatus) = await _eventService.AddEvent(pid);
					   model.AvailableDates = availability?.Select(a => a.Date).ToList() ?? new List<DateOnly>();
				   }
				   else
				   {
					   model.AvailableDates = new List<DateOnly>();
				   }
				   return View("EditEvent", model);
			   }

			   // Sjekk at pasient er innlogget
			   if (HttpContext.Session.GetInt32("PatientId") is not int editPatientId)
			   {
				   return RedirectToAction("Login", "Patient");
			   }

			   // Hent originalt event fra DB for å sammenligne felter og bruke EF-tracking
			   var (originalEvent, _, getStatus) = await _eventService.UpdateEvent(id, editPatientId);
			   if (getStatus != OperationStatus.Success || originalEvent == null)
			   {
				   return RedirectToAction("PatientEvents");
			   }

			   model.Event.PatientId = editPatientId;

			   // Ekstra validering: Start < End hvis tid endres
			   bool timeChanged =
				   model.Event.Date != originalEvent.Date ||
				   model.Event.Start != originalEvent.Start ||
				   model.Event.End != originalEvent.End;
			   if (timeChanged && model.Event.Start >= model.Event.End)
			   {
				   if (HttpContext.Session.GetInt32("PatientId") is int pidForStartEnd)
				   {
					   var (availability, availStatus) = await _eventService.AddEvent(pidForStartEnd);
					   model.AvailableDates = availability?.Select(a => a.Date).ToList() ?? new List<DateOnly>();
				   }
				   else
				   {
					   model.AvailableDates = new List<DateOnly>();
				   }
				   ModelState.AddModelError("", "Starttid må være før sluttid.");
				   return View("EditEvent", model);
			   }

			   var updateStatus = await _eventService.UpdateEventPartial(model.Event, originalEvent);
			   if (updateStatus == OperationStatus.Success)
			   {
				   TempData["SuccessMessage"] = "Event updated successfully!";
				   return RedirectToAction("PatientEvents");
			   }

			   ModelState.AddModelError("", timeChanged
				   ? "Could not update event. Det finnes overlapp eller ugyldig tid."
				   : "Could not update event. Please try again.");
			   // Repopuler AvailableDates fra service også ved lagringsfeil
			   if (HttpContext.Session.GetInt32("PatientId") is int pidForDates)
			   {
				   var (availability, availStatus) = await _eventService.AddEvent(pidForDates);
				   model.AvailableDates = availability?.Select(a => a.Date).ToList() ?? new List<DateOnly>();
			   }
			   else
			   {
				   model.AvailableDates = new List<DateOnly>();
			   }
			   return View("EditEvent", model);
		}
	}
}