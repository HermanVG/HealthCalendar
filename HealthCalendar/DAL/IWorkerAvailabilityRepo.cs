using System;
using HealthCalendar.Models;
using HealthCalendar.Shared;

namespace HealthCalendar.DAL;

public interface IWorkerAvailabilityRepo
{
    Task<(List<WorkerAvailability>, OperationStatus)> GetAvailability(int workerId);
    Task<OperationStatus> AddAvailability(WorkerAvailability availabilityTimestamp);
    Task<OperationStatus> DeleteAvailability(WorkerAvailability timestampAvailability);
    //Task<(WorkerAvailability?, OperationStatus)> GetTimestampAvailability(int availabilityId);
    //Task<(List<WorkerAvailability>?, OperationStatus)> GetDateAvailability(int workerId, DateOnly date);
    //Task<(List<WorkerAvailability>, OperationStatus)> GetMonthAvailability(int workerId, DateOnly date);
    //Task<OperationStatus> UpdateAvailability(WorkerAvailability timestampAvailability);
}