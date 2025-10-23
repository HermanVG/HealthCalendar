using System;
using HealthCalendar.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCalendar.DAL;

public class WorkerRepo : IWorkerRepo
{
    private readonly DatabaseContext _database;
    private readonly ILogger<WorkerRepo> _logger;

    public WorkerRepo(DatabaseContext database, ILogger<WorkerRepo> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task<(Worker?, RepoStatus)> GetWorker(int workerId)
    {
        try
        {
            Worker? worker = await _database.Workers.FindAsync(workerId);
            if (worker == null)
            {
                _logger.LogWarning("[WorkerRepo] GetWorker() Worker with  " +
                                  $"WorkerId = {workerId} was not found.");
                return (null, RepoStatus.NotFound);
            }
            worker.Password = "";
            return (worker, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[WorkerRepo] GetWorker() failed " +
                            $"when FindAsync() was called, error message: {e.Message}");
            return (null, RepoStatus.Error);
        }
    }

    public async Task<(List<String>, RepoStatus)> GetAllWorkerEmails()
    {
        try
        {
            List<String> emails = await _database.Workers.Select(w => w.Email).ToListAsync();
            return (emails, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[WorkerRepo] GetAllWorkerEmails() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }

    public async Task<(Worker?, RepoStatus)> GetWorkerLogin(String email, String hash)
    {
        try
        {
            Worker? worker = await _database.Workers.Where(w => w.Email == email).SingleAsync();
            if (worker == null)
            {
                _logger.LogInformation("[WorkerRepo] GetWorkerLogin() could not find " +
                                      $"Worker with Email = {email}");
                return (null, RepoStatus.NotFound);
            }
            else if (worker.Password != hash)
            {
                _logger.LogInformation("[WorkerRepo] GetWorkerLogin() given password did not match " +
                                      $"password of worker {email}");
                return (null, RepoStatus.Unauthorized);
            }

            worker.Password = "";
            return (worker, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[WorkerRepo] GetWorkerLogin() failed " +
                            $"when SingleAsync() was called, error message: {e.Message}");
            return (null, RepoStatus.Error);
        }
    }

    public async Task<RepoStatus> RegisterWorker(Worker worker)
    {
        try
        {
            _database.Workers.Add(worker);
            await _database.SaveChangesAsync();
            return RepoStatus.Success;
        }
        catch (Exception e)
        {
            _logger.LogError("[WorkerRepo] RegisterWorker() failed to create new " +
                            $"Worker {@worker}, error message: {e.Message}");
            return RepoStatus.Error;
        }
    }

    public async Task<RepoStatus> UpdateLastLogin(int workerId, DateTime loginTimestamp)
    {
        try
        {
            _database.Workers
                .Where(w => w.WorkerId == workerId)
                .ExecuteUpdate(db => db.SetProperty(w => w.LastLogin, loginTimestamp));
            await _database.SaveChangesAsync();
            return RepoStatus.Success;
        }
        catch (Exception e)
        {
            _logger.LogError("[WorkerRepo] UpdateLastLogin() failed to update LastLogin of Worker with " + 
                            $"WorkerId = {workerId} to {@loginTimestamp}, error message: {e.Message}");
            return (RepoStatus.Error);
        }
    }

}