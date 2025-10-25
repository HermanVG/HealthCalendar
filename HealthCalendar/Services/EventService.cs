using System;
using HealthCalendar.Models;
using HealthCalendar.Shared;
using HealthCalendar.DAL;

namespace HealthCalendar.Services;

public class EventService : IEventService
{
    private readonly EventRepo _eventRepo;
    private readonly PatientRepo _patientRepo;
    private readonly WorkerAvailabilityRepo _availabilityRepo;
    private readonly ILogger<EventRepo> _logger;
    public EventService(EventRepo eventRepo, PatientRepo patientRepo,
                        WorkerAvailabilityRepo availabilityRepo, ILogger<EventRepo> logger)
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

    private async Task<OperationStatus> ValidateEvent(Event eventt)
    {
        try
        {
            int patientId = eventt.PatientId;
            DateOnly date = eventt.Date;

            (List<Event>? existingEvents, OperationStatus operationStatus) =
                await _eventRepo.GetEventsForDate(patientId, date);

            if (operationStatus == OperationStatus.Success && existingEvents != null)
            {
                foreach (Event existingEvent in existingEvents)
                {
                    if (existingEvent.End.CompareTo(eventt.Start) > 0 &&
                        existingEvent.End.CompareTo(eventt.End) < 0 ||
                        existingEvent.Start.CompareTo(eventt.End) > 0 &&
                        existingEvent.Start.CompareTo(eventt.Start) < 0)
                    {
                        _logger.LogInformation($"[PatientService] Event {@eventt} is Not Acceptable.");
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
}