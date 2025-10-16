using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IPatientRepo
{
    Task<(Provider?, RepoStatus)> GetAssignedProvider(int patientId);
}