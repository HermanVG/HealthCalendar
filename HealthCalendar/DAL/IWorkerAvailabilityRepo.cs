using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IWorkerAvailabilityRepo
{
    Task<(WorkerAvailability?, RepoStatus)> GetTimestampAvailability(int availabilityId);
    Task<(List<WorkerAvailability>, RepoStatus)> GetAvailability(int workerId);
    Task<(List<WorkerAvailability>?, RepoStatus)> GetDateAvailability(int workerId, DateOnly date);
    Task<(List<WorkerAvailability>, RepoStatus)> GetMonthAvailability(int workerId, DateOnly date);
    Task<RepoStatus> AddTimestampAvailability(WorkerAvailability availabilityTimestamp);
    Task<RepoStatus> UpdateTimestampAvailability(WorkerAvailability timestampAvailability);
    //Task<RepoStatus> DeleteTimestampAvailability();
}