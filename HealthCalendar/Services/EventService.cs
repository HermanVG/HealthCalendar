using System;
using HealthCalendar.Models;
using HealthCalendar.Shared;
using HealthCalendar.DAL;

namespace HealthCalendar.Services;

public class EventService : IEventService
{
    private readonly EventRepo _eventRepo;
    private readonly PatientRepo _patientRepo;
    private readonly ILogger<EventRepo> _logger;
    public EventService(EventRepo eventRepo, PatientRepo patientRepo, ILogger<EventRepo> logger)
    {
        _logger = logger;
        _eventRepo = eventRepo;
        _patientRepo = patientRepo;
    }

    public async Task<OperationStatus> UpdateEvent(Event eventt)
    {
        try
        {
            OperationStatus validationStatus = await ValidateEvent(eventt);

            if (validationStatus == OperationStatus.Success)
                return await _eventRepo.UpdateEvent(eventt);
            else if (validationStatus == OperationStatus.NotAcceptable)
                return OperationStatus.NotAcceptable;
            
            _logger.LogError("[PatientService] Something went wrong with " +
                            $"ValidateEvent() with parameter eventt = {@eventt} " +
                            $"was called.");
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

            (List<Event>? eventList, OperationStatus operationStatus) =
                await _eventRepo.GetEventsForDate(patientId, date);

            if (operationStatus == OperationStatus.Success && eventList != null)
            {
                foreach (Event existingEvent in eventList)
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

            _logger.LogError("[PatientService] Something went wrong in" +
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