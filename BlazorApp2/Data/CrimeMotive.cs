using BlazorApp2.Data.Interfaces;

namespace BlazorApp2.Data
{
    public class CrimeMotive : IEntity
    {
        protected CrimeMotive() { }

        public CrimeMotive(string title) => Title = title;

        public int Id { get; set; }
        public string? Title { get; set; }
    }

}
