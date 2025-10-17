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

    public async Task<(List<AvailabilityTimestamp>, RepoStatus)> GetAvailability(int providerId)
    {
        try
        {
            List<AvailabilityTimestamp> availability = await _database.Availability
                .Where(av => av.ProviderId == providerId)
                .ToListAsync();
            return (availability, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailabilityTimestampRepo] GetAvailability() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }

    public async Task<(List<AvailabilityTimestamp>?, RepoStatus)> GetDateAvailability(int providerId, DateOnly date)
    {
        try
        {
            List<AvailabilityTimestamp>? dateAvailability = await _database.Availability
                .Where(aT => aT.ProviderId == providerId && aT.Date.Equals(date))
                .ToListAsync();
            return (dateAvailability, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableDateRepo] GetDateAvailability() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }

    public async Task<(List<AvailabilityTimestamp>, RepoStatus)> GetMonthAvailability(int providerId, DateOnly date)
    {
        try
        {
            List<AvailabilityTimestamp> monthAvailability = await _database.Availability
                .Where(aT => aT.ProviderId == providerId && aT.Date.Month == date.Month)
                .ToListAsync();
            return (monthAvailability, RepoStatus.Success);
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