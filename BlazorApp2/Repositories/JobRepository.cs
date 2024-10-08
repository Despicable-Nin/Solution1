using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Repositories
{
    public class JobRepository(ApplicationDbContext dbContext) : IJobRepository
    {

        public async Task<Job> CreateJob(JobType jobType, string name)
        {
            
            var job = new Job { Id = Guid.NewGuid(), Name = name, JobType = jobType };         
            dbContext.Jobs.Add(job);
            return job;
        }       

        public async Task<Job> GetFirstJob(JobType jobType)
        {
            var job = await dbContext.Jobs
                .FirstOrDefaultAsync(i => i.JobType == jobType && i.Status == JobStatus.Created);

            return job!;
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
    }
}
