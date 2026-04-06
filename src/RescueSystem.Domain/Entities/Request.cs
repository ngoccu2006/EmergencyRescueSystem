using RescueSystem.Domain.Entities.Base;
using RescueSystem.Domain.Enums;

namespace RescueSystem.Domain.Entities
{
    public class Request : BaseEntities
    {
        public Guid UserId { get; set; }

        public EmergencyType EmergencyType { get; set; }

        public Priority Priority { get; set; }

        public RequestStatus Status { get; set; } = RequestStatus.PENDING;

        public Guid LocationId { get; set; }

        public string Description { get; set; } = string.Empty;

        public string MediaUrl { get; set; } = string.Empty;

        public DateTime SubmittedTime { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User? RequestedBy { get; set; }
        public Location? Location { get; set; }
        public ICollection<Mission> Missions { get; set; } = new List<Mission>();
    }
}
