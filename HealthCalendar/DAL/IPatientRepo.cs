using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IPatientRepo
{
    Task<(Personell?, RepoStatus)> GetAssignedPersonell(int patientId);
}