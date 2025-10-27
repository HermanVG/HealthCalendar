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
                    FirstName = "Lars",
                    LastName = "Larsen",
                    Email = "a@a.com",
                    Password = "a",
                    Phone = 11111111,
                    WorkerAvailability = new List<WorkerAvailability>
                    {
                        new WorkerAvailability
                        {
                            WorkerId = 1,
                            Date = new DateOnly(2025, 10, 26)
                        },
                        new WorkerAvailability
                        {
                            WorkerId = 1,
                            Date = new DateOnly(2025, 10, 27)
                        },
                        new WorkerAvailability
                        {
                            WorkerId = 1,
                            Date = new DateOnly(2025, 10, 30)
                        },
                        new WorkerAvailability
                        {
                            WorkerId = 1,
                            Date = new DateOnly(2025, 10, 31)
                        }
                    },
                    Patients = new List<Patient>{}
                },
                new Worker
                {
                    WorkerId = 2,
                    FirstName = "Per",
                    LastName = "Persen",
                    Email = "b@a.com",
                    Password = "b",
                    Phone = 11111111,
                    WorkerAvailability = new List<WorkerAvailability>
                    {
                        new WorkerAvailability
                        {
                            WorkerId = 2,
                            Date = new DateOnly(2025, 10, 31)
                        }
                    },
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
                    FirstName = "Knut",
                    LastName = "Knutsen",
                    Email = "a@a.com",
                    Password = "a",
                    Phone = 11111111,
                    DateOfBirth = new DateOnly(2000, 1, 1),
                    Events = new List<Event>()
                },
                new Patient
                {
                    PatientId = 2,
                    WorkerId = 2,
                    FirstName = "Per",
                    LastName = "Persen",
                    Email = "c@a.com",
                    Password = "c",
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
                    Description = "Description of event",
                    Location = "Oslo",
                    Date = new DateOnly(2025, 10, 26),
                    Start = new TimeOnly(9, 0),
                    End = new TimeOnly(10, 0)
                },
                new Event
                {
                    EventId = 2,
                    PatientId = 1,
                    Description = "Description of event 2",
                    Location = "Oslo",
                    Date = new DateOnly(2025, 10, 27),
                    Start = new TimeOnly(14, 30),
                    End = new TimeOnly(15, 30)
                },
                new Event 
                { 
                    EventId = 3,
                    PatientId = 2,
                    Description = "Description of event 3",
                    Location = "Oslo",
                    Date = new DateOnly(2025, 10, 28),
                    Start = new TimeOnly(14, 30),
                    End = new TimeOnly(15, 30)
                }
            };
            context.AddRange(evt);
            context.SaveChanges();
        }
    }
}


