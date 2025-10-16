using System;
using HealthCalendar.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task<(List<Patient>?, RepoStatus)> GetAssignedPatients(int providerId)
    {
        try
        {
            List<Patient>? patients = await _database.Patients
                .Where(pa => pa.ProviderId == providerId)
                .ToListAsync();
            if (!patients.Any())
            {
                _logger.LogWarning("[PatientRepo] GetAssignedPatients() Patients related to Provider " +
                                  $"with ProviderId = {providerId} was not found.");
                return ([], RepoStatus.NotFound);
            }
            return (patients, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[PatientRepo] GetAssignedPatients() failed " +
                            $"when FindAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }
}