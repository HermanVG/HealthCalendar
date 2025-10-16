using System;
namespace HealthCalendar.Models
{
    public class AvailabilityTimestamp
    {
        // primary and foreign key
        public int ProviderId { get; set; }
        // primary key
        public DateOnly Date { get; set; }
        // primary key
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        // navigation property
        public virtual Provider Provider { get; set; } = default!;
    }
}
