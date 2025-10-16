using Microsoft.EntityFrameworkCore;
using HealthCalendar.Models;
using HealthCalendar.DAL;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace HealthCalendar.DAL;

public class AvailabilityTimestampRepo : IAvailabilityTimestampRepo
{
    private readonly DatabaseContext _database;
    private readonly ILogger<AvailabilityTimestampRepo> _logger;

    public AvailabilityTimestampRepo(DatabaseContext database, ILogger<AvailabilityTimestampRepo> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task<(List<DateOnly>, RepoStatus)> GetAvailability(int providerId)
    {
        try
        {
            List<AvailabilityTimestamp> availability = await _database.Availability
                .Where(av => av.ProviderId == providerId)
                .ToListAsync();
            if (!availability.Any()) return ([], RepoStatus.Success);

            List<DateOnly> dateList = new List<DateOnly>();
            foreach (AvailabilityTimestamp availabilityTimestamp in availability)
            {
                dateList.Add(availabilityTimestamp.Date);
            }
            dateList.Sort();

            return (dateList, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailabilityTimestampRepo] GetAvailability() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }

    /*
    public async Task<(DateOnly?, RepoStatus)> GetDateAvailability(int personellId, DateOnly date)
    {
        try
        {
            AvailableDate? availableDate = await _database.AvailableDates.FindAsync(personellId);
            if (availableDate == null) return (null, RepoStatus.Success);
            return (availableDate.Date, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableDateRepo] GetDateAvailability() failed " +
                            $"when FindAsync() was called, error message: {e.Message}");
            return (null, RepoStatus.Error);
        }
    }
    */

    public async Task<(List<DateOnly>, RepoStatus)> GetWeekAvailability(int providerId, DateOnly date)
    {
        try
        {
            List<AvailabilityTimestamp> availability = await _database.Availability
                .Where(av => av.ProviderId == providerId)
                .ToListAsync();
            if (!availability.Any()) return ([], RepoStatus.Success);

            int diffFromMonday = ((int)date.DayOfWeek + 6) % 7;
            DateOnly monday = date.AddDays(-diffFromMonday);

            List<DateOnly> week = new List<DateOnly>();
            for (int i = 0; i < 7; i++)
            {
                week.Add(monday.AddDays(i));
            }

            List<DateOnly> dateList = new List<DateOnly>();
            foreach (AvailabilityTimestamp availabilityTimestamp in availability)
            {
                if (week.Contains(availabilityTimestamp.Date)) dateList.Add(availabilityTimestamp.Date);
            }
            dateList.Sort();

            return (dateList, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailabilityTimestampRepo] GetWeekAvailability() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }

    public async Task<(List<DateOnly>, RepoStatus)> GetMonthAvailability(int providerId, DateOnly date)
    {
        try
        {
            List<AvailabilityTimestamp> availability = await _database.Availability
                .Where(av => av.ProviderId == providerId)
                .ToListAsync();
            if (!availability.Any()) return ([], RepoStatus.Success);

            int month = date.Month;

            List<DateOnly> dateList = new List<DateOnly>();
            foreach (AvailabilityTimestamp availabilityTimestamp in availability)
            {
                if (month == availabilityTimestamp.Date.Month) dateList.Add(availabilityTimestamp.Date);
            }
            dateList.Sort();

            return (dateList, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailabilityTimestampRepo] GetMonthAvailability() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }
    
    public async Task<RepoStatus> AddAvailabilityTimestamp(AvailabilityTimestamp availabilityTimestamp)
    {
        try
        {
            _database.Availability.Add(availabilityTimestamp);
            await _database.SaveChangesAsync();
            return RepoStatus.Success;
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableDateRepo] AddDate() failed to create new " + 
                            $"availableDate {@availabilityTimestamp}, this is the error message: {e.Message}");
            return RepoStatus.Error;
        }
    }
}