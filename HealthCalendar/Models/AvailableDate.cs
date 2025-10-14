using System;
namespace HealthCalendar.Models
{
    public class AvailableDate
    {
        // primary and foreign key
        public int personellId { get; set; }
        // primary key
        public DateOnly date { get; set; }
        // navigation property
        public virtual Personell personell { get; set; } = default!;
    }
}
