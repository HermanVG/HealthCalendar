using System;
using HealthCalendar.Models;
using HealthCalendar.Shared;
using HealthCalendar.DAL;

namespace HealthCalendar.Services;

public class UserService : IUserService
{
    private readonly PatientRepo _patientRepo;
    private readonly WorkerRepo _workerRepo;
    private readonly ILogger<EventRepo> _logger;
    public UserService(PatientRepo patientRepo, WorkerRepo workerRepo, ILogger<EventRepo> logger)
    {
        _patientRepo = patientRepo;
        _workerRepo = workerRepo;
        _logger = logger;
    }
    
    // We know hash isn't actually a hash here, don't have time to set up hashing
    private OperationStatus ValidatePassword(String password, String hash)
    {
        try
        {
            if (password == hash) return OperationStatus.Success;
            return OperationStatus.NotAcceptable;
        }
        catch (Exception e)
        {
            _logger.LogError("[UserService] ValidatePassword() failed, " +
                            $"error message: {e.Message}");
            return OperationStatus.Error;
        }
    }
}