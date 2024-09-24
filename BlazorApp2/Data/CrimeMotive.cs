namespace BlazorApp2.Data
{
    public class CrimeMotive
    {
        protected CrimeMotive() { }

        public CrimeMotive(string title) => Title = title;

        public int Id { get; set; }
        public string? Title { get; set; }
    }

}
