using System;
namespace HealthCalendar.Models
{
    public class AvailableDate
    {
        // primary and foreign key
        public int PersonellId { get; set; }
        // primary key
        public DateOnly Date { get; set; }
        // navigation property
        public virtual Personell Personell { get; set; } = default!;
    }
}
