using BlazorApp2.Data.Interfaces;

namespace BlazorApp2.Data
{
    public class Severity : IEntity
    {
        protected Severity() { }

        public Severity(string title) => Title = title;

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; } = string.Empty;
    }

}
