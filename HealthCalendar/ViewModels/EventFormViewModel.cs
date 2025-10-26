using System;
using System.Collections.Generic;
using HealthCalendar.Models;

namespace HealthCalendar.ViewModels
{
    // ViewModel for creating/editing an event, with available dates for dropdown
    public class EventFormViewModel
    {
        public Event Event { get; set; } = new Event();
        public List<DateOnly> AvailableDates { get; set; } = new List<DateOnly>();
    }
}
