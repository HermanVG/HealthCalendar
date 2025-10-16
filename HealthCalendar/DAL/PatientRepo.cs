using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public class PatientRepo : IPatientRepo
{
    private readonly DatabaseContext _database;
    private readonly ILogger<PatientRepo> _logger;
    public PatientRepo(DatabaseContext database, ILogger<PatientRepo> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task<(Provider?, RepoStatus)> GetAssignedProvider(int patientId)
    {
        try
        {
            Patient? patient = await _database.Patients.FindAsync(patientId);
            if (patient == null)
            {
                _logger.LogWarning("[PatientRepo] GetAssignedPatients() Patient " +
                                  $"with patientId = {patientId} was not found.");
                return (null, RepoStatus.NotFound);
            }
            return (patient.Provider, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[PatientRepo] GetAssignedPatients() failed " +
                            $"when FindAsync() was called, error message: {e.Message}");
            return (null, RepoStatus.Error);
        }
    }
}