using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IProviderRepo
{
    Task<(List<Patient>?, RepoStatus)> GetAssignedPatients(int providerId);
}