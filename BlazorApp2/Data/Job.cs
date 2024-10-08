using BlazorApp2.Data.Interfaces;

namespace BlazorApp2.Data
{
    public class Job  : IEntity
    {
        public Guid Id { get; set; } 
        public string? Name { get; set; }
        public JobType JobType { get; set; }

        public JobStatus Status { get; set; } = JobStatus.Created;
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public DateTime LastUpdatedDateTime { get; set; } = DateTime.Now;
        public int Retries { get; private set; }

        public Job()
        {
            Retries = 0;
        }

        public void Run()
        {
            Status = JobStatus.Running;
            LastUpdatedDateTime = DateTime.Now;
        }

        public void MarkAsSuccessful()
        {
            Status = JobStatus.Suceeded;
            LastUpdatedDateTime = DateTime.Now;
        }

        public void MarkAsFailed()
        {
            Status = Retries > 3 ? JobStatus.HardFail : JobStatus.Failed;
            LastUpdatedDateTime = DateTime.Now;
        }

    }
   
    public enum JobStatus 
    {
         Created = 0,
         Running = 1,       
         Suceeded = 2,
         Failed = 3,
         HardFail = 4
    }

    public enum JobType
    {
        Upload = 0,
        GIS = 1,
    }


}
