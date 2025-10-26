using Microsoft.EntityFrameworkCore;
namespace HealthCalendar.DAL;
using HealthCalendar.Models;

public static class DatabaseInit
{
    public static void Seed(IApplicationBuilder app)
    {

        using var serviceScope = app.ApplicationServices.CreateScope();
        DatabaseContext context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();


        if (!context.Workers.Any())
        {
            var workers = new List<Worker>
            {
                new Worker
                {
                    WorkerId = 1,
                    FirstName = "workername",
                    LastName = "workerlastname",
                    Email = "a@a.com",
                    Password = "a",
                    Phone = 11111111,
                    WorkerAvailability = new List<WorkerAvailability>{},
                    Patients = new List<Patient>{}
                }
            };
            context.AddRange(workers);
            context.SaveChanges();
        }

        if (!context.Patients.Any())
        {
            var patients = new List<Patient>
            {
                new Patient
                {
                    PatientId = 1,
                    WorkerId = 1,
                    FirstName = "patientFirstName",
                    LastName = "patientLastName",
                    Email = "a@a.com",
                    Password = "a",
                    Phone = 11111111,
                    DateOfBirth = new DateOnly(2000, 1, 1),
                    Events = new List<Event>()
                }
            };
            context.AddRange(patients);
            context.SaveChanges();
        }
        
        if (!context.Events.Any())
        {
            var evt = new List<Event>
            {
                new Event 
                { 
                    EventId = 1,
                    PatientId = 1,
                    Description = "description1",
                    Location = "location1",
                    Date = new DateOnly(2025, 10, 26),
                    Start = new TimeOnly(9, 0),
                    End = new TimeOnly(10, 0)
                },
                new Event 
                { 
                    EventId = 2,
                    PatientId = 1,
                    Description = "description2",
                    Location = "location2",
                    Date = new DateOnly(2024, 10, 27),
                    Start = new TimeOnly(14, 30),
                    End = new TimeOnly(15, 30)
                }
            };
            context.AddRange(evt);
            context.SaveChanges();
        }
    }
}


