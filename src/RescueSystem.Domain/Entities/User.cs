using Microsoft.AspNetCore.Identity;

namespace RescueSystem.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ICollection<Request> Requests { get; set; } = new List<Request>();
        public ICollection<RescueTeam> LeadingTeams { get; set; } = new List<RescueTeam>();
        public ICollection<RescueTeam> TeamsAsMember { get; set; } = new List<RescueTeam>();
        public ICollection<Mission> DispatchedMissions { get; set; } = new List<Mission>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
