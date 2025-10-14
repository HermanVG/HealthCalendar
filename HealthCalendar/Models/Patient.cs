using System;
namespace HealthCalendar.Models
{
    public class Patient
    {
        // primary key
        public int patientId { get; set; }
        // foreign key
        public int? personellId { get; set; }
        public String firstname { get; set; } = String.Empty;
        public String lastname { get; set; } = String.Empty;

        public String email { get; set; } = String.Empty;
        public String password { get; set; } = String.Empty;
        public int? phone { get; set; }
        public DateOnly dateOfBirth { get; set; }
        // navigation property
        public virtual Personell? personell { get; set; }
        // navigation property
        public virtual List<Event>? Events { get; set; }
    }
}