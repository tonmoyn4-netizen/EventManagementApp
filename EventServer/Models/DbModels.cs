using Microsoft.EntityFrameworkCore;

namespace EventServer.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public DateTime EventDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; } // banner image stored as Base64 text
        public List<Attendee> Attendees { get; set; } = new List<Attendee>();
    }

    public class Attendee
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public int EventId { get; set; } // foreign key -> Event.Id
    }

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
    }

}
