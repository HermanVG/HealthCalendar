using System;
namespace HealthCalendar.Models
{
    public class Event
    {
        // primary key
        public int EventId { get; set; }
        // foreign key
        public int PatientId { get; set; }
        public String? Description { get; set; }
        public String Location { get; set; } = String.Empty;
        public DateOnly Date { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        //public DateTime CreationTimestamp { get; set; }
        // navigation property
        public virtual Patient Patient { get; set; } = default!;
    }
}