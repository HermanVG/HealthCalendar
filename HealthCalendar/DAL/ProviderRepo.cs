using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public class ProviderRepo : IProviderRepo
{
    private readonly DatabaseContext _database;
    private readonly ILogger<ProviderRepo> _logger;

    public ProviderRepo(DatabaseContext database, ILogger<ProviderRepo> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task<(List<Patient>?, RepoStatus)> GetAssignedPatients(int providerId)
    {
        try
        {
            Provider? provider = await _database.Providers.FindAsync(providerId);
            if (provider == null)
            {
                _logger.LogWarning("[ProviderRepo] GetAssignedPatients() Personell " +
                                  $"with providerId = {providerId} was not found.");
                return ([], RepoStatus.NotFound);
            }
            return (provider.Patients, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[ProviderRepo] GetAssignedPatients() failed " +
                            $"when FindAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }
}