using BlazorApp2.Data;

namespace BlazorApp2.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<JobStatus> GetJobStatus(Guid id);
        Task<Job> GetFirstJob(JobType jobType);     
        Task<Job> CreateJob(JobType jobType, string name);
        Task<Job> UpdateJob(Job job);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
