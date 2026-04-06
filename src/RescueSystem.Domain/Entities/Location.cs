using RescueSystem.Domain.Entities.Base;

namespace RescueSystem.Domain.Entities
{
    public class Location : BaseEntities
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Landmark { get; set; } = string.Empty;
    }
}
