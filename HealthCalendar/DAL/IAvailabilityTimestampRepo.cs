using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IAvailabilityTimestampRepo
{
    Task<(List<DateOnly>, RepoStatus)> GetAvailability(int providerId);
    //Task<(DateOnly?, RepoStatus)> GetDateAvailability(int id, DateOnly date);
    Task<(List<DateOnly>, RepoStatus)> GetWeekAvailability(int providerId, DateOnly date);
    Task<(List<DateOnly>, RepoStatus)> GetMonthAvailability(int providerId, DateOnly date);
    Task<RepoStatus> AddAvailabilityTimestamp(AvailabilityTimestamp availabilityTimestamp);
}