using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IPatientRepo
{
    Task<(List<Patient>?, RepoStatus)> GetAssignedPatients(int workerId);
    Task<(Patient?, RepoStatus)> GetPatientByEmail(String email);
    //Task<(List<String>, RepoStatus)> GetAllPatientEmails();
    //Task<RepoStatus> RegisterPatient(Patient patient);
}