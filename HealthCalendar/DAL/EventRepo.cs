using System;
using HealthCalendar.Models;
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

    public async Task<(List<Event>?, RepoStatus)> GetEventsForDate(int patientId, DateOnly date)
    {
        try
        {
            List<Event> eventsForDate = await _database.Events
                .Where(ev => ev.PatientId == patientId && ev.Date.Equals(date))
                .ToListAsync();
            return (eventsForDate, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[EventRepo] GetAssignedPatients() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }

    public async Task<(List<Event>?, RepoStatus)> GetEventsForMonth(int patientId, DateOnly date)
    {
        try
        {
            List<Event> monthEvents = await _database.Events
                .Where(ev => ev.PatientId == patientId && ev.Date.Month == date.Month)
                .ToListAsync();
            return (monthEvents, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[EventRepo] GetEventsForMonth() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }

    public async Task<(List<Event>?, RepoStatus)> GetNewEvents(int patientId, DateTime lastLogin)
    {
        try
        {
            List<Event> newEvents = await _database.Events
                .Where(ev => ev.PatientId == patientId && lastLogin.CompareTo(ev.CreationTimestamp) <= 0)
                .ToListAsync();
            return (newEvents, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[EventRepo] getNewEvents() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }

    public async Task<RepoStatus> AddEvent(Event eventt)
    {
        try
        {
            _database.Events.Add(eventt);
            await _database.SaveChangesAsync();
            return RepoStatus.Success;
        }
        catch (Exception e)
        {
            _logger.LogError("[EventRepo] GetAssignedPatients() failed to create new " +
                            $"availableDate {@eventt}, this is the error message: {e.Message}");
            return RepoStatus.Error;
        }
    }
}