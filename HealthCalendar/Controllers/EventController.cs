// Function moved from Repo to Controller, needs to be uncommented and tweaked

/*
public async Task<(List<Event>?, RepoStatus)> GetEventsForWeek(List<Event> monthEvents, DateOnly date)
{
    try
    {
        int diffFromMonday = ((int)date.DayOfWeek + 6) % 7;
        DateOnly monday = date.AddDays(-diffFromMonday);

        List<DateOnly> week = new List<DateOnly>();
        for (int i = 0; i < 7; i++)
        {
            week.Add(monday.AddDays(i));
        }

        List<Event> weekEvents = new List<Event>();
        foreach (Event eventt in monthEvents)
        {
            if (week.Contains(eventt.Date)) weekEvents.Add(eventt);
        }

        return (weekEvents, RepoStatus.Success);
    }
    catch (Exception e)
    {
        _logger.LogError("[EventRepo] GetEventsForWeek() failed " +
                        $"when ToListAsync() was called, error message: {e.Message}");
        return ([], RepoStatus.Error);
    }
}
*/