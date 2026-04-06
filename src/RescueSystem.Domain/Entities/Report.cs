using RescueSystem.Domain.Entities.Base;
using RescueSystem.Domain.Enums;

namespace RescueSystem.Domain.Entities
{
    public class Report : BaseEntities
    {
        public Guid MissionId { get; set; }

        public Guid CreatedById { get; set; }

        public string Content { get; set; } = string.Empty;

        public string AttachmentUrl { get; set; } = string.Empty;

        public ReportType Type { get; set; }

        // Navigation Properties
        public Mission? Mission { get; set; }
        public User? CreatedBy { get; set; }
    }
}
