using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IWorkerRepo
{
    Task<(Worker?, RepoStatus)> GetWorkerByEmail(String email);
    //Task<(Worker?, RepoStatus)> GetWorker(int patientId);
    //Task<(List<String>, RepoStatus)> GetAllWorkerEmails();
    //Task<RepoStatus> RegisterWorker(Worker worker);
    //Task<RepoStatus> UpdateLastLogin(int workerId, DateTime loginTimestamp);
}