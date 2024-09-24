using BlazorApp2.Data.Interfaces;

namespace BlazorApp2.Data
{
    public class Weather : IEntity
    {
        protected Weather() { }

        public Weather(string title) => Title = title;

        public int Id { get; set; }
        public string? Title { get; set; }
    }

}
