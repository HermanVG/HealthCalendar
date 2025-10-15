using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IAvailableDateRepo
{
    Task<(List<DateOnly>?, RepoStatus)> GetAvailableDates(int id, DateOnly date);
    Task<(DateOnly?, RepoStatus)> GetDateAvailability(int id, DateOnly date);
    Task<(List<DateOnly>?, RepoStatus)> GetWeekAvailability(int id, DateOnly date);
    Task<(List<DateOnly>?, RepoStatus)> GetMonthAvailability(int id, DateOnly date);
    Task<RepoStatus> AddDate(AvailableDate availableDate);
}