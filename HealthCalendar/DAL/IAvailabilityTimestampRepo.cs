using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IAvailabilityTimestampRepo
{
    Task<(List<AvailabilityTimestamp>, RepoStatus)> GetAvailability(int providerId);
    //Task<(DateOnly?, RepoStatus)> GetDateAvailability(int id, DateOnly date);
    Task<(List<AvailabilityTimestamp>, RepoStatus)> GetWeekAvailability(int providerId, DateOnly date);
    Task<(List<AvailabilityTimestamp>, RepoStatus)> GetMonthAvailability(int providerId, DateOnly date);
    Task<RepoStatus> AddAvailabilityTimestamp(AvailabilityTimestamp availabilityTimestamp);
}