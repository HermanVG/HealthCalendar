using System;
using HealthCalendar.Models;
using HealthCalendar.Shared;

namespace HealthCalendar.Services;

public interface IEventService
{
    //Task<(List<Event>?, OperationStatus)> GetAssignedEvents(int workerId);
    //Task<(Event, List<WorkerAvailability>, OperationStatus)> UpdateEvent(int eventId, int workerId);
    //Task<OperationStatus> AddEvent(Event eventt);
    Task<OperationStatus> UpdateEvent(Event eventt);
}