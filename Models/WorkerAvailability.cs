using System;
namespace HealthCalendar.Models
{
    public class WorkerAvailability
    {
        // primary key
        public int AvailabilityId { get; set; }
        // foreign key
        public int WorkerId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        // navigation property
        public virtual Worker Worker { get; set; } = default!;
    }
}
