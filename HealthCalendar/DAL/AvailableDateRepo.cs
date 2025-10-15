using Microsoft.EntityFrameworkCore;
using HealthCalendar.Models;
using HealthCalendar.DAL;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace HealthCalendar.DAL;

public class AvailableDateRepo : IAvailableDateRepo
{
    private readonly DatabaseContext _database;
    private readonly ILogger<AvailableDateRepo> _logger;

    public AvailableDateRepo(DatabaseContext database, ILogger<AvailableDateRepo> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task<(List<DateOnly>?, RepoStatus)> GetAvailableDates(int id, DateOnly date)
    {
        try
        {
            List<AvailableDate> availableDates = await _database.AvailableDates
                .Where(aD => aD.PersonellId == id)
                .ToListAsync();
            if (!availableDates.Any()) return ([], RepoStatus.Success);

            List<DateOnly> dateList = new List<DateOnly>();
            foreach (AvailableDate availableDate in availableDates)
            {
                dateList.Add(availableDate.Date);
            }
            dateList.Sort();

            return (dateList, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableDateRepo] GetAvailableDates() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return (null, RepoStatus.Error);
        }
    }

    public async Task<(DateOnly?, RepoStatus)> GetDateAvailability(int id, DateOnly date)
    {
        try
        {
            AvailableDate? availableDate = await _database.AvailableDates.FindAsync(id);
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

    public async Task<(List<DateOnly>?, RepoStatus)> GetWeekAvailability(int id, DateOnly date)
    {
        try
        {
            List<AvailableDate> availableDates = await _database.AvailableDates
                .Where(aD => aD.PersonellId == id)
                .ToListAsync();
            if (!availableDates.Any()) return ([], RepoStatus.Success);

            int diffFromMonday = ((int)date.DayOfWeek + 6) % 7;
            DateOnly monday = date.AddDays(-diffFromMonday);

            List<DateOnly> week = new List<DateOnly>();
            for (int i = 0; i < 7; i++)
            {
                week.Add(monday.AddDays(i));
            }

            List<DateOnly> dateList = new List<DateOnly>();
            foreach (AvailableDate availableDate in availableDates)
            {
                if (week.Contains(availableDate.Date)) dateList.Add(availableDate.Date);
            }
            dateList.Sort();

            return (dateList, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableDateRepo] GetWeekAvailability() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return (null, RepoStatus.Error);
        }
    }

    public async Task<(List<DateOnly>?, RepoStatus)> GetMonthAvailability(int id, DateOnly date)
    {
        try
        {
            List<AvailableDate> availableDates = await _database.AvailableDates
                .Where(aD => aD.PersonellId == id)
                .ToListAsync();
            if (!availableDates.Any()) return ([], RepoStatus.Success);

            int month = date.Month;

            List<DateOnly> dateList = new List<DateOnly>();
            foreach (AvailableDate availableDate in availableDates)
            {
                if (month == availableDate.Date.Month) dateList.Add(availableDate.Date);
            }
            dateList.Sort();

            return (dateList, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableDateRepo] GetMonthAvailability() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return (null, RepoStatus.Error);
        }
    }
    
    public async Task<RepoStatus> AddDate(AvailableDate availableDate)
    {
        try
        {
            _database.AvailableDates.Add(availableDate);
            await _database.SaveChangesAsync();
            return RepoStatus.Success;
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailableDateRepo] AddDate() failed to create new " + 
                            $"availableDate {@availableDate} when Add() was called: {e.Message}");
            return RepoStatus.Error;
        }
    }
}