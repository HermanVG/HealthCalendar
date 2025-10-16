using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public class PersonellRepo : IPersonellRepo
{
        private readonly DatabaseContext _database;
    private readonly ILogger<AvailableDateRepo> _logger;

    public PersonellRepo(DatabaseContext database, ILogger<AvailableDateRepo> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task<(List<Patient>?, RepoStatus)> GetAssignedPatients(int personellId)
    {
        try
        {
            Personell? personell = await _database.Personell.FindAsync(personellId);
            if (personell == null) return ([], RepoStatus.Success);
            return (personell.Patients, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[PersonellRepo] GetAssignedPatients() failed " +
                            $"when FindAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }
}