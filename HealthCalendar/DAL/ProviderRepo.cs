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

    public async Task<(Provider?, RepoStatus)> GetProvider(int providerId)
    {
        try
        {
            Provider? provider = await _database.Providers.FindAsync(providerId);
            if (provider == null)
            {
                _logger.LogWarning("[ProviderRepo] GetProvider() Provider with  " +
                                  $"ProviderId = {providerId} was not found.");
                return (null, RepoStatus.NotFound);
            }
            return (provider, RepoStatus.Success);

        }
        catch (Exception e)
        {
            _logger.LogError("[ProviderRepo] GetProvider() failed " +
                            $"when FindAsync() was called, error message: {e.Message}");
            return (null, RepoStatus.Error);
        }
    }
}