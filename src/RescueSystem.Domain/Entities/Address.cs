using RescueSystem.Domain.Entities.Base;

namespace RescueSystem.Domain.Entities
{
    public class Address : BaseEntities
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string GPS { get; set; } = string.Empty;
    }
}
