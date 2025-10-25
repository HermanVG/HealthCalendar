using System;
using HealthCalendar.Models;
using HealthCalendar.Shared;

namespace HealthCalendar.DAL;

public interface IWorkerRepo
{
    Task<(Worker?, OperationStatus)> GetWorkerByEmail(String email);
    //Task<(Worker?, OperationStatus)> GetWorker(int patientId);
    //Task<(List<String>, OperationStatus)> GetAllWorkerEmails();
    //Task<OperationStatus> RegisterWorker(Worker worker);
    //Task<OperationStatus> UpdateLastLogin(int workerId, DateTime loginTimestamp);
}