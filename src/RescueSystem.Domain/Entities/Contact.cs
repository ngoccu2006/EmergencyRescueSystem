using RescueSystem.Domain.Entities.Base;

namespace RescueSystem.Domain.Entities
{
    public class Contact : BaseEntities
    {
        public string Name { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
