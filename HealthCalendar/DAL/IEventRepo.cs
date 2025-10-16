using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IEventRepo
{
    Task<(List<Event>?, RepoStatus)> GetEventsForDate(int patientId, DateOnly date);
    Task<RepoStatus> AddEvent(Event eventt);
}