using BlazorApp2.Data.Interfaces;

namespace BlazorApp2.Data
{
    public class PoliceDistrict : IEntity
    {
        protected PoliceDistrict() { }
        public PoliceDistrict(string title) => Title = title;

        public int Id { get; set; }
        public string? Title { get; set; }
    }

}
