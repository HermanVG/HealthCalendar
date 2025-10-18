using System;
namespace HealthCalendar.Models
{
    public class Patient
    {
        // primary key
        public int PatientId { get; set; }
        // foreign key
        public int? WorkerId { get; set; }
        public String FirstName { get; set; } = String.Empty;
        public String LastName { get; set; } = String.Empty;

        public String Email { get; set; } = String.Empty;
        public String Password { get; set; } = String.Empty;
        public int? Phone { get; set; }
        public DateOnly DateOfBirth { get; set; }
        // navigation property
        public virtual Worker? Worker { get; set; }
        // navigation property
        public virtual List<Event>? Events { get; set; }
    }
}