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

    public async Task<(Personell?, RepoStatus)> GetAssignedPersonell(int patientId)
    {
        try
        {
            Patient? patient = await _database.Patients.FindAsync(patientId);
            if (patient == null)
            {
                _logger.LogWarning("[PatientRepo] GetAssignedPatients() Patient " +
                                  $"with personellId = {patientId} was not found.");
                return (null, RepoStatus.NotFound);
            }
            return (patient.Personell, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[PatientRepo] GetAssignedPatients() failed " +
                            $"when FindAsync() was called, error message: {e.Message}");
            return (null, RepoStatus.Error);
        }
    }
}