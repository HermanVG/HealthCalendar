using System;
using HealthCalendar.Models;
using HealthCalendar.Shared;
using HealthCalendar.DAL;

namespace HealthCalendar.Services;

public class UserService : IUserService
{
    private readonly IPatientRepo _patientRepo;
    private readonly IWorkerRepo _workerRepo;
    private readonly ILogger<UserService> _logger;
    public UserService(IPatientRepo patientRepo, IWorkerRepo workerRepo, ILogger<UserService> logger)
    {
        _patientRepo = patientRepo;
        _workerRepo = workerRepo;
        _logger = logger;
    }

    public async Task<(Patient? patient, OperationStatus)> PatientLogin(String email, String password)
    {
        try
        {
            (Patient? patient, OperationStatus operationStatus) = await _patientRepo.GetPatientByEmail(email);
            if (operationStatus == OperationStatus.Error)
            {
                _logger.LogError("[UserService] Something went wrong when " +
                                $"GetPatientByEmail() with parameter email = {email} " +
                                 "was called.");
                return (null, OperationStatus.Error);
            }
            if (operationStatus == OperationStatus.NotFound || patient == null)
                return (null, OperationStatus.NotFound);

            OperationStatus validationStatus = ValidatePassword(password, patient.Password);
            if (validationStatus == OperationStatus.Success) return (patient, OperationStatus.Success);
            if (validationStatus == OperationStatus.NotAcceptable) return (null, OperationStatus.NotAcceptable);

            _logger.LogError("[UserService] ValidatePassword() failed to validate password.");
            return (null, OperationStatus.Error);
        }
        catch (Exception e)
        {
            _logger.LogError("[UserService] PatientLogin() failed to login Patient " +
                            $"with Email = {email}, error message: {e.Message}");
            return (null, OperationStatus.Error);
        }
    }
    
    public async Task<(Worker? worker, OperationStatus)> WorkerLogin(String email, String password)
    {
        try
        {
            (Worker? worker, OperationStatus operationStatus) = await _workerRepo.GetWorkerByEmail(email);
            if (operationStatus == OperationStatus.Error)
            {
                _logger.LogError("[UserService] Something went wrong when " +
                                $"GetWorkerByEmail() with parameter email = {email} " +
                                 "was called.");
                return (null, OperationStatus.Error);
            }
            if (operationStatus == OperationStatus.NotFound || worker == null) 
                return (null, OperationStatus.NotFound);

            OperationStatus validationStatus = ValidatePassword(password, worker.Password);
            if (validationStatus == OperationStatus.Success) return (worker, OperationStatus.Success);
            if (validationStatus == OperationStatus.NotAcceptable) return (null, OperationStatus.NotAcceptable);

            _logger.LogError("[UserService] ValidatePassword() failed to validate password.");
            return (null, OperationStatus.Error);
        }
        catch (Exception e)
        {
            _logger.LogError("[UserService] WorkerLogin() failed to login Worker " +
                            $"with Email = {email}, error message: {e.Message}");
            return (null, OperationStatus.Error);
        }
    }
    
    // We know "hash" isn't actually a hash here, don't have time to set up hashing
    private OperationStatus ValidatePassword(String password, String hash)
    {
        try
        {
            if (password == hash) return OperationStatus.Success;
            
            _logger.LogInformation($"[UserService] password did not match.");
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