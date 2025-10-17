using System;
using HealthCalendar.Models;
using Microsoft.EntityFrameworkCore;

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
            provider.Password = "";
            return (provider, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[ProviderRepo] GetProvider() failed " +
                            $"when FindAsync() was called, error message: {e.Message}");
            return (null, RepoStatus.Error);
        }
    }

    public async Task<(Provider?, RepoStatus)> GetProviderLogin(String email, String hash)
    {
        try
        {
            Provider? provider = await _database.Providers.Where(pr => pr.Email == email).SingleAsync();
            if (provider == null)
            {
                _logger.LogInformation("[ProviderRepo] GetProviderLogin() could not find " +
                                      $"Provider with Email = {email}");
                return (null, RepoStatus.NotFound);
            }
            else if (provider.Password == hash)
            {
                _logger.LogInformation("[ProviderRepo] GetProviderLogin() given password did not match " +
                                      $"password of provider {email}");
                return (null, RepoStatus.Unauthorized);
            }

            provider.Password = "";
            return (provider, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[ProviderRepo] GetProviderLogin() failed " +
                            $"when SingleAsync() was called, error message: {e.Message}");
            return (null, RepoStatus.Error);
        }
    }
}