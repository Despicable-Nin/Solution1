using BlazorApp2.Data.Interfaces;

namespace BlazorApp2.Data
{
    public class CrimeType : IEntity
    {
        protected CrimeType() { }

        public CrimeType(string title) => Title = title;

        public int Id { get; set; }
        public string? Title { get; set; }
    }

}
