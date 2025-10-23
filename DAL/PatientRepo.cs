using System;
using Castle.Components.DictionaryAdapter.Xml;
using HealthCalendar.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCalendar.DAL;

public class PatientRepo : IPatientRepo
{
    private readonly DatabaseContext _database;
    private readonly ILogger<PatientRepo> _logger;
    public PatientRepo(DatabaseContext database, ILogger<PatientRepo> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task<(List<Patient>?, RepoStatus)> GetAssignedPatients(int workerId)
    {
        try
        {
            List<Patient>? patients = await _database.Patients
                .Where(p => p.WorkerId == workerId)
                .ToListAsync();
            if (!patients.Any())
            {
                _logger.LogWarning("[PatientRepo] GetAssignedPatients() Patients related to Worker " +
                                  $"with WorkerId = {workerId} was not found.");
                return ([], RepoStatus.NotFound);
            }
            patients.ForEach(p => p.Password = "");
            return (patients, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[PatientRepo] GetAssignedPatients() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }

    public async Task<(List<String>, RepoStatus)> GetAllPatientEmails()
    {
        try
        {
            List<String> emails = await _database.Patients.Select(p => p.Email).ToListAsync();
            return (emails, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[PatientRepo] GetAllPatientEmails() failed " +
                            $"when ToListAsync() was called, error message: {e.Message}");
            return ([], RepoStatus.Error);
        }
    }

    public async Task<(Patient?, RepoStatus)> GetPatientLogin(String email, String hash)
    {
        try
        {
            Patient? patient = await _database.Patients.Where(p => p.Email == email).SingleAsync();
            if (patient == null)
            {
                _logger.LogInformation("[PatientRepo] GetPatientLogin() could not find " +
                                      $"Patient with Email = {email}");
                return (null, RepoStatus.NotFound);
            }
            else if (patient.Password == hash)
            {
                _logger.LogInformation("[PatientRepo] GetPatientLogin() given password did not match " +
                                      $"password of patient {email}");
                return (null, RepoStatus.Unauthorized);
            }

            patient.Password = "";
            return (patient, RepoStatus.Success);
        }
        catch (Exception e)
        {
            _logger.LogError("[PatientRepo] GetPatientLogin() failed " +
                            $"when SingleAsync() was called, error message: {e.Message}");
            return (null, RepoStatus.Error);
        }
    }

    public async Task<RepoStatus> RegisterPatient(Patient patient)
    {
        try
        {
            _database.Patients.Add(patient);
            await _database.SaveChangesAsync();
            return RepoStatus.Success;
        }
        catch (Exception e)
        {
            _logger.LogError("[PatientRepo] RegisterPatient() failed to create new " +
                            $"Patient {@patient}, error message: {e.Message}");
            return RepoStatus.Error;
        }
    }
}