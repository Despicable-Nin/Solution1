namespace BlazorApp2.Data
{
    public class Weather
    {
        protected Weather() { }

        public Weather(string title) => Title = title;

        public int Id { get; set; }
        public string? Title { get; set; }
    }

}
