using System;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public interface IWorkerRepo
{
    Task<(Worker?, RepoStatus)> GetWorker(int patientId);
    Task<(Worker?, RepoStatus)> GetWorkerLogin(String email, String hash);
    Task<RepoStatus> UpdateLastLogin(int workerId, DateTime loginTimestamp);
}