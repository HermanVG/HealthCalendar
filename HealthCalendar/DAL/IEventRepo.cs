using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IEventRepo
{
    Task<(Event?, RepoStatus)> GetEvent(int eventId);
    Task<(List<Event>?, RepoStatus)> GetEvents(int patientId);
    Task<(List<Event>?, RepoStatus)> GetEventsForDate(int patientId, DateOnly date);
    Task<RepoStatus> AddEvent(Event eventt);
    Task<RepoStatus> UpdateEvent(Event eventt);
    Task<RepoStatus> DeleteEvent(Event eventt);
    //Task<(List<Event>?, RepoStatus)> GetEventsForMonth(int patientId, DateOnly date);
    //Task<(List<Event>?, RepoStatus)> GetNewEvents(int patientId, DateTime lastLogin);
}