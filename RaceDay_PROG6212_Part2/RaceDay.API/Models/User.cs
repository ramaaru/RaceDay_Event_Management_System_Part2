namespace RaceDay.API.Models;

public class User
{
    public int UserID { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<EventEnrolment> EventEnrolments { get; set; } = new List<EventEnrolment>();
}
