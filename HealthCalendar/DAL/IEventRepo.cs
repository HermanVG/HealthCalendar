using System;
using HealthCalendar.Models;
using HealthCalendar.Shared;

namespace HealthCalendar.DAL;

public interface IEventRepo
{
    Task<(Event?, OperationStatus)> GetEvent(int eventId);
    Task<(List<Event>?, OperationStatus)> GetEvents(int patientId);
    Task<(List<Event>?, OperationStatus)> GetEventsForDate(DateOnly date);
    Task<OperationStatus> AddEvent(Event eventt);
    Task<OperationStatus> UpdateEvent(Event eventt);
    Task<OperationStatus> DeleteEvent(Event eventt);
    //Task<(List<Event>?, OperationStatus)> GetEventsForMonth(int patientId, DateOnly date);
    //Task<(List<Event>?, OperationStatus)> GetNewEvents(int patientId, DateTime lastLogin);
}