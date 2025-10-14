using System;
namespace HealthCalendar.Models
{
    public class Personell
    {
        // primary key
        public int personellId { get; set; }
        public String firstname { get; set; } = String.Empty;
        public String lastname { get; set; } = String.Empty;

        public String email { get; set; } = String.Empty;
        public String password { get; set; } = String.Empty;
        public int? phone { get; set; }
        // navigation property
        public virtual List<AvailableDate>? AvailableDates { get; set; }
        // navigation property
        public virtual List<Patient>? Patients { get; set; }

    }
}