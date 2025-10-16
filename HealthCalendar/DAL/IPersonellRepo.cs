using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IPersonellRepo
{
    Task<(List<Patient>?, RepoStatus)> GetAssignedPatients(int personellId);
}