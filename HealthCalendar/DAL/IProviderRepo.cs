using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IProviderRepo
{
    Task<(Provider?, RepoStatus)> GetProvider(int patientId);
}