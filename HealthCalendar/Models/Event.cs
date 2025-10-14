using System;
namespace HealthCalendar.Models
{
    public class Event
    {
        // primary key
        public int eventId { get; set; }
        // foreign key
        public int patientId { get; set; }
        public String? description { get; set; }
        public String location { get; set; } = String.Empty;
        public DateOnly date { get; set; }
        public TimeOnly start { get; set; }
        public TimeOnly end { get; set; }
        // navigation property
        public virtual Patient patient { get; set; } = default!;
    }
}