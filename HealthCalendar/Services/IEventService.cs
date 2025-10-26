using System;
using HealthCalendar.Models;
using HealthCalendar.Shared;

namespace HealthCalendar.Services;

public interface IEventService
{
    Task<(List<Event>?, List<Patient>?, OperationStatus)> GetAssignedEvents(int workerId);
    Task<(List<WorkerAvailability>?, OperationStatus)> AddEvent(int workerId);
    Task<OperationStatus> AddEvent(Event eventt);
    Task<(Event?, List<WorkerAvailability>?, OperationStatus)> UpdateEvent(int eventId, int workerId);
    Task<OperationStatus> UpdateEvent(Event eventt);
}