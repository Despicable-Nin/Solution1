using BlazorApp2.BackgroundServices;
using BlazorApp2.Data;
using BlazorApp2.Repositories.Interfaces;
using Hangfire;

namespace BlazorApp2.Services.Jobs
{
    public interface IJobService
    {
        Task<Guid> UpdateJob(JobDto job);
        void DeleteJob(Guid jobId);
        Task<JobDto?> GetJobByIdAsync(Guid jobId);
        Task<JobDto?> GetAJob();
        Task<Guid> CreateJobAsync(string batchId, JobType jobType);
    }

    public class JobService(IJobRepository jobRepository) : IJobService
    {

        public async Task<Guid> CreateJobAsync(string batchId, JobType jobType)
        {
            var job = new Job
            {
                Name = batchId,
                JobType = jobType,
                Status = JobStatus.Created,
            };

            await jobRepository.CreateJob(job);
            await jobRepository.SaveChangesAsync(CancellationToken.None);


            return job.Id;
        }

        public void DeleteJob(Guid jobId)
        {
            throw new NotImplementedException();
        }

        public async Task<JobDto?> GetAJob()
        {
            var job = await jobRepository.GetSingleUnprocessedJob(JobType.Upload);
            if (job == null) return null;

            return new JobDto
            {
                Id = job.Id,
                JobType = job.JobType,
                Name = job.Name,
                LastUpdatedDateTime = job.LastUpdatedDateTime,
                Retries = job.Retries,
                Status = job.Status,
                CreatedDateTime = job.CreatedDateTime,
            };
        }

        public async Task<JobDto?> GetJobByIdAsync(Guid jobId)
        {
            var job = await jobRepository.GetJobByIdAsync(jobId);
            if (job == null) return null;

            return new JobDto
            {
                Id = job.Id,
                JobType = job.JobType,
                Name = job.Name,
                LastUpdatedDateTime = job.LastUpdatedDateTime,
                Retries = job.Retries,
                Status = job.Status ,
                CreatedDateTime = job.CreatedDateTime,
            };
        }

        public async Task<Guid> UpdateJob(JobDto jobDto)
        {
            var job = await jobRepository.GetJobByIdAsync(jobDto.Id);
            if(job == null)
            {
                throw new KeyNotFoundException($"Job with Id: {jobDto.Id} not found.");
            }

           if(jobDto.Status == JobStatus.Failed) {
                job.MarkAsFailed();
            }else if(jobDto.Status == JobStatus.Suceeded)
            {
                job.MarkAsSuccessful();
            } else
            {
                job.Run();
            }

            await jobRepository.UpdateJob(job);
            await jobRepository.SaveChangesAsync(CancellationToken.None);

            return job.Id;

        }
    }

    public record JobDto
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public JobType JobType { get; init; }

        public JobStatus Status { get; init; }
        public DateTime CreatedDateTime { get; init; } 
        public DateTime LastUpdatedDateTime { get; init; } 
        public int Retries { get; init; } = 0;


    }
}
