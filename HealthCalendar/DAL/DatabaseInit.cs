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
                    FirstName = "name",
                    LastName = "name",
                    Email = "a@a.com",
                    Password = "hashedPassword123",
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
                    FirstName = "name",
                    LastName = "name",
                    Email = "a@a.com",
                    Password = "password",
                    Phone = 11111111,
                    DateOfBirth = new DateOnly(2000, 1, 1),
                    Events = new List<Event>()
                }
            };
            context.AddRange(patients);
            context.SaveChanges();
        }
    }
}


