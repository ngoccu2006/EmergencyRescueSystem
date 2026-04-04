using Microsoft.AspNetCore.Identity;

namespace RescueSystem.Domain.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public Role() { }

        public Role(string name)
        {
            Name = name;
        }

        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
