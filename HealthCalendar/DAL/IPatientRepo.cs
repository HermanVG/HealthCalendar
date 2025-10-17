using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IPatientRepo
{
    Task<(List<Patient>?, RepoStatus)> GetAssignedPatients(int workerId);
    Task<(Patient?, RepoStatus)> GetPatientLogin(String email, String hash);
}