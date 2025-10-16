using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IPatientRepo
{
    Task<(List<Patient>?, RepoStatus)> GetAssignedPatients(int providerId);
}