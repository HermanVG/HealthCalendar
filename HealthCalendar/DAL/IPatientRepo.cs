using System;
using HealthCalendar.Models;
using HealthCalendar.Shared;

namespace HealthCalendar.DAL;

public interface IPatientRepo
{
    Task<(List<Patient>?, OperationStatus)> GetAssignedPatients(int workerId);
    Task<(Patient?, OperationStatus)> GetPatientByEmail(String email);
    //Task<(List<String>, OperationStatus)> GetAllPatientEmails();
    //Task<OperationStatus> RegisterPatient(Patient patient);
}