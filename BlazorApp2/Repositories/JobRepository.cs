using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Repositories
{
    public class JobRepository(ApplicationDbContext dbContext) : IJobRepository
    {

        public async Task<Job> CreateJob(Job job)
        {
            return (await dbContext.Jobs.AddAsync(job)).Entity;
        }       

        public async Task<Job> GetSingleUnprocessedJob(JobType jobType)
        {
            var job = await dbContext.Jobs
                .FirstOrDefaultAsync(i => i.JobType == jobType && i.Status == JobStatus.Created);

            return job!;
        }

        public async Task<IEnumerable<Job>> GetJobsByStatus(JobStatus jobStatus, JobType? jobType = null)
        {
            var jobs = dbContext.Jobs.AsNoTracking().Where(i => i.Status == jobStatus);

            if (jobType.HasValue)
            {
                jobs = jobs.Where(i => i.JobType == jobType);
            }

            return await jobs.ToArrayAsync();
        }

        public async Task<JobStatus> GetJobStatus(Guid id)
        {
            var job = await dbContext.Jobs.FirstOrDefaultAsync(i => i.Id == id);
            if(job == null)
            {
                throw new KeyNotFoundException($"{id} not found for {nameof(Job)} entity");
            }

            return job.Status;
        }

        public async Task<Job> UpdateJob(Job job)
        {
            dbContext.Jobs.Update(job);
            return job;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Job?> GetJobByIdAsync(Guid id)
        {
            return await dbContext.Jobs.FindAsync(id);
        }
    }
}
