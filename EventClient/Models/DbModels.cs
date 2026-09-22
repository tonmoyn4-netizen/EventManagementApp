using System;
using System.Collections.Generic;
using System.Text;

namespace EventClient.Models
{ 
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public DateTime EventDate { get; set; } = DateTime.Today;
        public bool IsActive { get; set; } = true;
        public string? ImageUrl { get; set; } // banner image stored as a Base64 string
        public List<Attendee> Attendees { get; set; } = new();
    }

    public class Attendee
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public int EventId { get; set; }
    }
}
