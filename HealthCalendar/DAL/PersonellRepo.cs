using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public class PersonellRepo : IPersonellRepo
{
    private readonly DatabaseContext _database;
    private readonly ILogger<PersonellRepo> _logger;

    public PersonellRepo(DatabaseContext database, ILogger<PersonellRepo> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task<(List<Patient>?, RepoStatus)> GetAssignedPatients(int personellId)
    {
        try
        {
            Personell? personell = await _database.Personell.FindAsync(personellId);
            if (personell == null)
            {
                _logger.LogWarning("[PatientRepo] GetAssignedPatients() Personell " +
                                  $"with personellId = {personellId} was not found.");
                return ([], RepoStatus.NotFound);
            }
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