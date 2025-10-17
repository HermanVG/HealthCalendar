using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IWorkerAvailabilityRepo
{
    Task<(List<WorkerAvailability>, RepoStatus)> GetAvailability(int workerId);
    Task<(List<WorkerAvailability>?, RepoStatus)> GetDateAvailability(int workerId, DateOnly date);
    Task<(List<WorkerAvailability>, RepoStatus)> GetMonthAvailability(int workerId, DateOnly date);
    Task<RepoStatus> AddAvailability(WorkerAvailability availabilityTimestamp);
    //Task<RepoStatus> DeleteAvailabilityTimestamp();
}