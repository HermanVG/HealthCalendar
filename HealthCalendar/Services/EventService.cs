using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthCalendar.Models;
using HealthCalendar.Shared;
using HealthCalendar.DAL;

namespace HealthCalendar.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepo _eventRepo;
        private readonly IPatientRepo _patientRepo;
        private readonly IWorkerAvailabilityRepo _availabilityRepo;
        private readonly ILogger<EventRepo> _logger;
        public EventService(IEventRepo eventRepo, IPatientRepo patientRepo,
                            IWorkerAvailabilityRepo availabilityRepo, ILogger<EventRepo> logger)
        {
            _eventRepo = eventRepo;
            _patientRepo = patientRepo;
            _availabilityRepo = availabilityRepo;
            _logger = logger;
        }

        public async Task<(List<Event>?, List<Patient>?, OperationStatus)> GetAssignedEvents(int workerId)
        {
            try
            {
                (List<Patient>? assignedPatients, OperationStatus patientOperationStatus) =
                    await _patientRepo.GetAssignedPatients(workerId);

                if (patientOperationStatus == OperationStatus.Success && assignedPatients != null)
                {
                    List<Event> assignedEvents = new List<Event>();

                    foreach (Patient patient in assignedPatients)
                    {
                        (List<Event>? events, OperationStatus eventOperationStatus) =
                            await _eventRepo.GetEvents(patient.PatientId);

                        if (eventOperationStatus == OperationStatus.Error || events == null)
                        {
                            _logger.LogError("[PatientService] Something Something went " +
                                             "wrong in EventRepo when when GetEvents() " +
                                            $"with parameter patientId = {patient.PatientId} " +
                                             "was called.");
                        }
                        else events.ForEach(e => assignedEvents.Add(e));
                    }

                    return (assignedEvents, assignedPatients, OperationStatus.Success);
                }

                if (patientOperationStatus == OperationStatus.NotFound) return ([], [], OperationStatus.NotFound);

                _logger.LogError("[PatientService] Something went wrong in PatientRepo when " +
                                $"GetAssignedPatients() with parameter workerId = {workerId} " +
                                 "was called.");
                return ([], [], OperationStatus.Error);
            }
            catch (Exception e)
            {
                _logger.LogError("[PatientService] GetAssignedEvents() failed to get " +
                                 "Events from Patients assigned to Worker with " +
                                $"WorkerId = {workerId}, error message: {e.Message}");
                return ([], [], OperationStatus.Error);
            }
        }


        public async Task<(List<WorkerAvailability>?, OperationStatus)> AddEvent(int workerId)
        {
            try
            {
                (List<WorkerAvailability>? availability, OperationStatus operationStatus) =
                    await _availabilityRepo.GetAvailability(workerId);

                if (operationStatus == OperationStatus.Success) return (availability, OperationStatus.Success);

                _logger.LogError("[PatientService] Something went wrong in WorkerAvailabilityRepo " +
                                 "when when GetEvents() GetAvailability() with parameter workerId = " +
                                $"{workerId} was called.");
                return ([], OperationStatus.Error);
            }
            catch (Exception e)
            {
                _logger.LogError("[PatientService] AddEvent() failed to get " +
                                 "WorkerAvailability from Worker with WorkerId = " +
                                $"{workerId}, error message: {e.Message}");
                return ([], OperationStatus.Error);
            }
        }

        public async Task<OperationStatus> AddEvent(Event eventt)
        {
            try
            {
                OperationStatus validationStatus = await ValidateEvent(eventt);

                if (validationStatus == OperationStatus.Success)
                    return await _eventRepo.AddEvent(eventt);

                if (validationStatus == OperationStatus.NotAcceptable)
                    return OperationStatus.NotAcceptable;

                _logger.LogError("[PatientService] Something went wrong when " +
                                $"ValidateEvent() with parameter eventt = {@eventt} " +
                                 "was called.");
                return OperationStatus.Error;
            }
            catch (Exception e)
            {
                _logger.LogError("[PatientService] AddEvent() failed to add " +
                                $"Event {@eventt} into database, error message: {e.Message}");
                return OperationStatus.Error;
            }
        }


        public async Task<(Event?, List<WorkerAvailability>?, OperationStatus)> UpdateEvent(int eventId, int workerId)
        {
            try
            {
    				(Event? eventt, OperationStatus eventOperationStatus) = await _eventRepo.GetEvent(eventId);
    				if (eventOperationStatus == OperationStatus.Error || eventt == null)
    				{
    					_logger.LogError("[PatientService] Something went wrong in EventRepo when " +
    						$"GetEvent() with parameter eventId = {eventId} " +
    						 "was called.");
    					return (null, [], OperationStatus.Error);
    				}

                (List<WorkerAvailability> availability, OperationStatus availabilityOperationStatus) =
                    await _availabilityRepo.GetAvailability(workerId);
                if (availabilityOperationStatus == OperationStatus.Error || availability == null)
                {
                    _logger.LogError("[PatientService] Something went wrong in WorkerAvailabilityRepo " +
                                    $"when GetEvent() with parameter eventId = {eventId} " +
                                     "was called.");
                    return (null, [], OperationStatus.Error);
                }

                return (eventt, availability, OperationStatus.Success);
            }
            catch (Exception e)
            {
                _logger.LogError("[PatientService] UpdateEvent() failed to get Event with " +
                                $"EventId = {eventId} and WorkerAvailability from Worker " +
                                $"with WorkerId = {workerId}, error message: {e.Message}");
                return (null, [], OperationStatus.Error);
            }
        }


        public async Task<OperationStatus> UpdateEvent(Event eventt)
        {
            try
            {
                OperationStatus validationStatus = await ValidateEvent(eventt);
                if (validationStatus == OperationStatus.Success)
                    return await _eventRepo.UpdateEvent(eventt);
                if (validationStatus == OperationStatus.NotAcceptable)
                    return OperationStatus.NotAcceptable;
                _logger.LogError("[PatientService] Something went wrong when " +
                                $"ValidateEvent() with parameter eventt = {@eventt} " +
                                 "was called.");
                return OperationStatus.Error;
            }
            catch (Exception e)
            {
                _logger.LogError("[PatientService] UpdateEvent() failed to update " +
                                $"database with Event {@eventt}, error message: {e.Message}");
                return OperationStatus.Error;
            }
        }

        public async Task<OperationStatus> UpdateEventPartial(Event updatedEvent, Event originalEvent)
        {
            try
            {
                // Sjekk om tid/dato er endret
                bool timeChanged =
                    updatedEvent.Date != originalEvent.Date ||
                    updatedEvent.Start != originalEvent.Start ||
                    updatedEvent.End != originalEvent.End;

                if (timeChanged)
                {
                    OperationStatus validationStatus = await ValidateEvent(updatedEvent);
                    if (validationStatus != OperationStatus.Success)
                        return validationStatus;
                }

                // Oppdater kun felter som kan endres
                originalEvent.Description = updatedEvent.Description;
                originalEvent.Location = updatedEvent.Location;
                originalEvent.Date = updatedEvent.Date;
                originalEvent.Start = updatedEvent.Start;
                originalEvent.End = updatedEvent.End;

                return await _eventRepo.UpdateEvent(originalEvent);
            }
            catch (Exception e)
            {
                _logger.LogError("[PatientService] UpdateEventPartial() failed to update " +
                                $"database with Event {@updatedEvent}, error message: {e.Message}");
                return OperationStatus.Error;
            }
        }

        private async Task<OperationStatus> ValidateEvent(Event eventt)
        {
            try
            {
                // Sjekk at starttid er før sluttid
                if (eventt.Start >= eventt.End)
                {
                    _logger.LogInformation($"[PatientService] Event {@eventt} is Not Acceptable (start >= end).");
                    return OperationStatus.NotAcceptable;
                }

                int patientId = eventt.PatientId;
                DateOnly date = eventt.Date;

                (List<Event>? existingEvents, OperationStatus operationStatus) =
                    await _eventRepo.GetEventsForDate(date);

                if (operationStatus == OperationStatus.Success && existingEvents != null)
                {
                    foreach (Event existingEvent in existingEvents)
                    {
                        if (eventt.Start < existingEvent.End && existingEvent.Start < eventt.End ||
                            eventt.Start == existingEvent.Start || eventt.End == existingEvent.End)
                        {
                            _logger.LogInformation($"[PatientService] Event {@eventt} is Not Acceptable (overlap detected).");
                            return OperationStatus.NotAcceptable;
                        }
                    }
                    return OperationStatus.Success;
                }

                _logger.LogError("[PatientService] Something went wrong in " +
                                 "EventRepo when GetEvents() with parameters " +
                                $"patientId = {patientId} and date = {date} was called.");
                return OperationStatus.Error;
            }
            catch (Exception e)
            {
                _logger.LogError("[PatientService] ValidateEvent() failed " +
                                $"to validate Event {@eventt}, error message: {e.Message}");
                return OperationStatus.Error;
            }
        }

        public async Task<(List<Event>?, OperationStatus)> GetEventsForPatient(int patientId)
        {
            return await _eventRepo.GetEvents(patientId);
        }

        public async Task<OperationStatus> DeleteEvent(int eventId)
        {
            try
            {
                var (eventt, status) = await _eventRepo.GetEvent(eventId);
                if (status != OperationStatus.Success || eventt == null)
                {
                    _logger.LogError($"[EventService] Event with ID {eventId} not found for deletion.");
                    return OperationStatus.NotFound;
                }
                var deleteStatus = await _eventRepo.DeleteEvent(eventt);
                if (deleteStatus == OperationStatus.Success)
                {
                    return OperationStatus.Success;
                }
                _logger.LogError($"[EventService] Failed to delete event with ID {eventId}.");
                return OperationStatus.Error;
            }
            catch (Exception e)
            {
                _logger.LogError($"[EventService] Exception in DeleteEvent: {e.Message}");
                return OperationStatus.Error;
            }
        }
    }
}