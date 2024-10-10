using BlazorApp2.Data;

namespace BlazorApp2.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<JobStatus> GetJobStatus(Guid id);
        Task<Job> GetSingleUnprocessedJob(JobType jobType);  
        Task<Job?> GetJobByIdAsync(Guid id);
        Task<Job> CreateJob(Job jobTyp);
        Task<Job> UpdateJob(Job job);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
