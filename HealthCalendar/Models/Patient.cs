using System;
namespace HealthCalendar.Models
{
    public class Patient
    {
        // primary key
        public int PatientId { get; set; }
        // foreign key
        public int? ProviderId { get; set; }
        public String Firstname { get; set; } = String.Empty;
        public String Lastname { get; set; } = String.Empty;

        public String Email { get; set; } = String.Empty;
        public String Password { get; set; } = String.Empty;
        public int? Phone { get; set; }
        public DateOnly DateOfBirth { get; set; }
        // navigation property
        public virtual Provider? Provider { get; set; }
        // navigation property
        public virtual List<Event>? Events { get; set; }
    }
}