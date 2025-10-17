using Microsoft.EntityFrameworkCore;
using HealthCalendar.Models;
using HealthCalendar.DAL;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace HealthCalendar.DAL;

public class WorkerAvailabilityRepo : IWorkerAvailabilityRepo
{
    private readonly DatabaseContext _database;
    private readonly ILogger<WorkerAvailabilityRepo> _logger;

    public WorkerAvailabilityRepo(DatabaseContext database, ILogger<WorkerAvailabilityRepo> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task<(List<WorkerAvailability>, RepoStatus)> GetAvailability(int workerId)
    {
        try
        {
            List<WorkerAvailability> availability = await _database.WorkerAvailability
                .Where(wA => wA.WorkerId == workerId)
                .ToListAsync();
            return (availability, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[WorkerAvailabilityRepo] GetAvailability() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }

    public async Task<(List<WorkerAvailability>?, RepoStatus)> GetDateAvailability(int workerId, DateOnly date)
    {
        try
        {
            List<WorkerAvailability>? dateAvailability = await _database.WorkerAvailability
                .Where(aW => aW.WorkerId == workerId && aW.Date.Equals(date))
                .ToListAsync();
            return (dateAvailability, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[WorkerAvailabilityRepo] GetDateAvailability() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }

    public async Task<(List<WorkerAvailability>, RepoStatus)> GetMonthAvailability(int workerId, DateOnly date)
    {
        try
        {
            List<WorkerAvailability> monthAvailability = await _database.WorkerAvailability
                .Where(aW => aW.WorkerId == workerId && aW.Date.Month == date.Month)
                .ToListAsync();
            return (monthAvailability, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[WorkerAvailabilityRepo] GetMonthAvailability() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }
    
    public async Task<RepoStatus> AddAvailability(WorkerAvailability timestampAvailability)
    {
        try
        {
            _database.WorkerAvailability.Add(timestampAvailability);
            await _database.SaveChangesAsync();
            return RepoStatus.Success;
        }
        catch (Exception e)
        {
            _logger.LogError("[WorkerAvailabilityRepo] AddDate() failed to create new " + 
                            $"availableDate {@timestampAvailability}, this is the error message: {e.Message}");
            return RepoStatus.Error;
        }
    }
}