using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IAvailabilityTimestampRepo
{
    Task<(List<AvailabilityTimestamp>, RepoStatus)> GetAvailability(int providerId);
    Task<(List<AvailabilityTimestamp>?, RepoStatus)> GetDateAvailability(int providerId, DateOnly date);
    Task<(List<AvailabilityTimestamp>, RepoStatus)> GetMonthAvailability(int providerId, DateOnly date);
    Task<(List<AvailabilityTimestamp>, RepoStatus)> 
        GetWeekAvailability(List<AvailabilityTimestamp> monthAvailability, DateOnly date);
    Task<RepoStatus> AddAvailabilityTimestamp(AvailabilityTimestamp availabilityTimestamp);
}