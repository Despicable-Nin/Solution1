namespace BlazorApp2.Data
{
    public class PoliceDistrict
    {
        protected PoliceDistrict() { }
        public PoliceDistrict(string title) => Title = title;

        public int Id { get; set; }
        public string? Title { get; set; }
    }

}
