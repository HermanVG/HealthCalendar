using System;
using HealthCalendar.Models;
using HealthCalendar.Shared;
using Microsoft.EntityFrameworkCore;

namespace HealthCalendar.DAL;

public class EventRepo : IEventRepo
{
    private readonly DatabaseContext _database;
    private readonly ILogger<EventRepo> _logger;
    public EventRepo(DatabaseContext database, ILogger<EventRepo> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task<(Event?, OperationStatus)> GetEvent(int eventId)
    {
        try
        {
            Event? eventt = await _database.Events.FindAsync(eventId);
            if (eventt == null)
            {
                _logger.LogWarning("[EventRepo] GetEvent() Event " +
                                  $"with EventId = {eventId} was not found.");
                return (null, OperationStatus.NotFound);
            }
            return (eventt, OperationStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[EventRepo] GetEvent() failed " +
                            $"when FindAsync() was called, error message: {e.Message}");
            return (null, OperationStatus.Error);
        }
    }

    public async Task<(List<Event>?, OperationStatus)> GetEvents(int patientId)
    {
        try
        {
            List<Event> eventsForDate = await _database.Events
                .Where(ev => ev.PatientId == patientId)
                .ToListAsync();
            return (eventsForDate, OperationStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[EventRepo] GetEvents() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], OperationStatus.Error);
        }
    }

    public async Task<(List<Event>?, OperationStatus)> GetEventsForDate(int patientId, DateOnly date)
    {
        try
        {
            List<Event> eventsForDate = await _database.Events
                .Where(ev => ev.PatientId == patientId && ev.Date.Equals(date))
                .ToListAsync();
            return (eventsForDate, OperationStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[EventRepo] GetAssignedPatients() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], OperationStatus.Error);
        }
    }

    public async Task<OperationStatus> AddEvent(Event eventt)
    {
        try
        {
            _database.Events.Add(eventt);
            await _database.SaveChangesAsync();
            return OperationStatus.Success;
        }
        catch (Exception e)
        {
            _logger.LogError("[EventRepo] GetAssignedPatients() failed to create new " +
                            $"Event {@eventt}, error message: {e.Message}");
            return OperationStatus.Error;
        }
    }

    public async Task<OperationStatus> UpdateEvent(Event eventt)
    {
        try
        {
            _database.Events.Update(eventt);
            await _database.SaveChangesAsync();
            return OperationStatus.Success;
        }
        catch (Exception e)
        {
            _logger.LogError("[EventRepo] UpdateEvent() failed to update previous " +
                            $"Event to {@eventt}, error message: {e.Message}");
            return OperationStatus.Error;
        }
    }

    public async Task<OperationStatus> DeleteEvent(Event eventt)
    {
        try
        {
            _database.Events.Remove(eventt);
            await _database.SaveChangesAsync();
            return OperationStatus.Success;
        }
        catch (Exception e)
        {
            _logger.LogError("[EventRepo] DeleteEvent() failed to remove " +
                            $"Event {@eventt} from database, error message: {e.Message}");
            return OperationStatus.Error;
        }
    }

    /*public async Task<(List<Event>?, OperationStatus)> GetEventsForMonth(int patientId, DateOnly date)
    {
        try
        {
            List<Event> monthEvents = await _database.Events
                .Where(ev => ev.PatientId == patientId && ev.Date.Month == date.Month)
                .ToListAsync();
            return (monthEvents, OperationStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[EventRepo] GetEventsForMonth() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], OperationStatus.Error);
        }
    }

    public async Task<(List<Event>?, OperationStatus)> GetNewEvents(int patientId, DateTime lastLogin)
    {
        try
        {
            List<Event> newEvents = await _database.Events
                .Where(ev => ev.PatientId == patientId && lastLogin.CompareTo(ev.CreationTimestamp) <= 0)
                .ToListAsync();
            return (newEvents, OperationStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[EventRepo] getNewEvents() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], OperationStatus.Error);
        }
    }*/
}