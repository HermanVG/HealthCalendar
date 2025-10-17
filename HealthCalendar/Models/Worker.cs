using System;
namespace HealthCalendar.Models
{
    public class Worker
    {
        // primary key
        public int WorkerId { get; set; }
        public String Firstname { get; set; } = String.Empty;
        public String Lastname { get; set; } = String.Empty;

        public String Email { get; set; } = String.Empty;
        public String Password { get; set; } = String.Empty;
        public int? Phone { get; set; }
        public DateTime? LastLogin { get; set; }
        // navigation property
        public virtual List<WorkerAvailability>? WorkerAvailability { get; set; }
        // navigation property
        public virtual List<Patient>? Patients { get; set; }

    }
}