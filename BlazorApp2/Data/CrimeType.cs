namespace BlazorApp2.Data
{
    public class CrimeType
    {
        protected CrimeType() { }

        public CrimeType(string title) => Title = title;

        public int Id { get; set; }
        public string? Title { get; set; }
    }

}
