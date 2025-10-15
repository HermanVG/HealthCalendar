using System;
namespace HealthCalendar.Models
{
    public class Personell
    {
        // primary key
        public int PersonellId { get; set; }
        public String Firstname { get; set; } = String.Empty;
        public String Lastname { get; set; } = String.Empty;

        public String Email { get; set; } = String.Empty;
        public String Password { get; set; } = String.Empty;
        public int? Phone { get; set; }
        // navigation property
        public virtual List<AvailableDate>? AvailableDates { get; set; }
        // navigation property
        public virtual List<Patient>? Patients { get; set; }

    }
}