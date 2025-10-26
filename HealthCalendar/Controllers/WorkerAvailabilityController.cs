
// Function moved from Repo to Controller, needs to be uncommented and tweaked

/*
    public async Task<(List<AvailabilityTimestamp>, RepoStatus)> 
        GetWeekAvailability(List<AvailabilityTimestamp> monthAvailability, DateOnly date)
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

            List<AvailabilityTimestamp> weekAvailability = new List<AvailabilityTimestamp>();
            foreach (AvailabilityTimestamp availabilityTimestamp in monthAvailability)
            {
                if (week.Contains(availabilityTimestamp.Date)) weekAvailability.Add(availabilityTimestamp);
            }

            return (weekAvailability, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[AvailabilityTimestampRepo] GetWeekAvailability() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }
    */


