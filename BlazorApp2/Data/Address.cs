using BlazorApp2.Data.Interfaces;

namespace BlazorApp2.Data
{
    public class Address : IEntity
    {
        public Address() { }

        public Guid Id { get; set; }
        public string? Description { get; set; }
        public double? Latitude { get; set; } = 0D;
        public double? Longitude { get; set; } = 0D;
    }
}
