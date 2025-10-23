using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthCalendar.Models;

namespace HealthCalendar.DAL;

public class DatabaseContext : IdentityDbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<WorkerAvailability> WorkerAvailability { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

}
