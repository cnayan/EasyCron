using EasyCron.Api.DbContextes;
using EasyCron.Api.Entities;
using EasyCron.Api.Interfaces;
using EasyCron.Api.Models;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyCron.Api.Implements
{
    public class CronJobService : ICronJobService
    {
        private readonly CronJobDbContext _cronJobDbContext;
        private readonly ICronTranslator _cronTranslator;
        public CronJobService
            (CronJobDbContext cronJobDbContext
            , ICronTranslator cronTranslator
            )
        {
            _cronJobDbContext = cronJobDbContext;
            _cronTranslator = cronTranslator;
        }

        public async Task<string> AddOrUpdateRecurringJobAsync(string jobId, string jobName, string callAddress, string paramJson, string cron, string group, string remarks, string createdBy)
        {
            Job job = null;
            if (string.IsNullOrWhiteSpace(jobId))
            {
                string newId = Guid.NewGuid().ToString();
                job = new Job
                {
                    Id = newId,
                    CallAddress = callAddress,
                    CreatedBy = createdBy,
                    Name = jobName,
                    JobType = JobType.RecurringJob.ToString(),
                    Cron = cron,
                    CronDescription = await _cronTranslator.TranslateCron2DescriptionAsync(cron),
                    ParamJson = paramJson,
                    Group = group,
                    HangfireJobId = newId,
                    Remarks = remarks,
                    CreatedOn = DateTime.Now
                };

                await _cronJobDbContext.Jobs.AddAsync(job);
                jobId = job.Id;
            }
            else
            {
                job = await _cronJobDbContext.Jobs.FindAsync(jobId);
                if (job == null)
                {
                    throw new InvalidOperationException($"can't find job {jobId}");
                }
            }

            try
            {
                RecurringJob.AddOrUpdate<IJobWorker>(jobId,
                x => x.DoWorkAsync(jobId, callAddress, paramJson), cron,
                TimeZoneInfo.Local);

                await _cronJobDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                RecurringJob.RemoveIfExists(job.Id);
                throw ex;
            }


            return jobId;
        }

        public async Task<string> AddFireAndForgetJobAsync(string jobName, string callAddress, string paramJson, string group, string remarks)
        {
            var job = new Job
            {
                CallAddress = callAddress,
                CreatedBy = string.Empty,
                Name = jobName,
                Group = group,
                ParamJson = paramJson,
                JobType = JobType.FireAndForgetJob.ToString(),
                Remarks = remarks
            };

            await _cronJobDbContext.AddAsync(job);

            try
            {
                job.HangfireJobId =
                BackgroundJob.Enqueue<IJobWorker>(_ => _.DoWorkAsync(job.Id, callAddress, paramJson));

                await _cronJobDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                BackgroundJob.Delete(job.HangfireJobId);
                throw ex;
            }

            return job.Id;
        }

        public async Task<string> AddContinuationsJobAsync(string parentJobId, string jobName, string callAddress, string paramJson, string group, string remarks)
        {
            var job = new Job
            {
                CallAddress = callAddress,
                CreatedBy = string.Empty,
                Name = jobName,
                Group = group,
                ParamJson = paramJson,
                JobType = JobType.ContinuationsJob.ToString(),
                ParentJobId = parentJobId,
                Remarks = remarks
            };

            await _cronJobDbContext.AddAsync(job);

            try
            {
                job.HangfireJobId
               = BackgroundJob.ContinueJobWith<IJobWorker>(
               parentJobId, _ => _.DoWorkAsync(job.Id, callAddress, paramJson));
                await _cronJobDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                BackgroundJob.Delete(job.HangfireJobId);
                throw ex;
            }


            return job.Id;
        }

        public async Task<string> AddDelayedJobAsync(string jobName, string callAddress, string paramJson, long delayMinutes, string group, string remarks)
        {
            var job = new Job
            {
                CallAddress = callAddress,
                CreatedBy = string.Empty,
                Name = jobName,
                Group = group,
                ParamJson = paramJson,
                JobType = JobType.DelayedJob.ToString(),
                DelayMinutes = delayMinutes,
                Remarks = remarks
            };

            try
            {
                await _cronJobDbContext.Jobs.AddAsync(job);
                job.HangfireJobId =
                    BackgroundJob.Schedule<IJobWorker>(_ => _.DoWorkAsync(job.Id, callAddress, paramJson), TimeSpan.FromMinutes(delayMinutes));

                await _cronJobDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                BackgroundJob.Delete(job.HangfireJobId);
                throw ex;
            }

            return job.Id;
        }

        public async Task DeleteAsync(string jobId)
        {
            var job = await _cronJobDbContext.Jobs.Include(_ => _.JobLogs).FirstOrDefaultAsync(_ => _.Id == jobId);

            if (job == null || string.IsNullOrWhiteSpace(job.HangfireJobId)) return;

            if (job.JobType == JobType.RecurringJob.ToString())
            {
                RecurringJob.RemoveIfExists(job.HangfireJobId);
            }
            else
            {
                BackgroundJob.Delete(job.HangfireJobId);
            }

            _cronJobDbContext.Jobs.Remove(job);
            await _cronJobDbContext.SaveChangesAsync();
        }

        public async Task DisableRecurringJobAsync(string jobId)
        {
            var job = await _cronJobDbContext.Jobs.FindAsync(jobId);
            if (job == null)
            {
                throw new InvalidOperationException($"can't find job {jobId}");
            }
            if (string.IsNullOrWhiteSpace(job.HangfireJobId))
            {
                throw new InvalidOperationException($"hangfireJobId is empty");
            }

            if (job.JobType != JobType.RecurringJob.ToString())
            {
                throw new InvalidOperationException($"cannot disable {job.JobType}");
            }

            job.IsActive = false;

            RecurringJob.RemoveIfExists(job.HangfireJobId);
            _cronJobDbContext.Jobs.Update(job);
            await _cronJobDbContext.SaveChangesAsync();
        }

        public async Task ActiveRecurringJobAsync(string jobId)
        {
            var job = await _cronJobDbContext.Jobs.FindAsync(jobId);
            if (job == null)
            {
                throw new InvalidOperationException($"can't find job {jobId}");
            }
            if (string.IsNullOrWhiteSpace(job.HangfireJobId))
            {
                throw new InvalidOperationException($"hangfireJobId is empty");
            }
            if (job.JobType != JobType.RecurringJob.ToString())
            {
                throw new InvalidOperationException($"cannot active {job.JobType}");
            }

            await AddOrUpdateRecurringJobAsync(job.Id, job.Name, job.CallAddress, job.ParamJson, job.Cron, job.Group, job.Remarks, string.Empty);

            job.IsActive = true;
            await _cronJobDbContext.SaveChangesAsync();
        }

        public async Task TriggerRecurringJobAsync(string jobId)
        {
            var job = await _cronJobDbContext.Jobs.FindAsync(jobId);
            if (job == null)
            {
                throw new InvalidOperationException($"can't find job {jobId}");
            }
            if (string.IsNullOrWhiteSpace(job.HangfireJobId))
            {
                throw new InvalidOperationException($"hangfireJobId is empty");
            }
            if (job.JobType != JobType.RecurringJob.ToString())
            {
                throw new InvalidOperationException($"cannot trigger {job.JobType}");
            }

            RecurringJob.Trigger(job.HangfireJobId);
        }

        public Task<PagedResult<Job>> GetJobListAsync(int pageNo = 1, int pageSize = 10, string keywords = "")
        {
            var result = new PagedResult<Job>
            {
                PageNo = pageNo,
                PageSize = pageSize,
                List = new List<Job>()
            };

            var jobs = _cronJobDbContext.Jobs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                jobs = jobs.Where(_ => _.CallAddress.Contains(keywords)
                || _.CreatedBy.Contains(keywords)
                || _.JobType.Contains(keywords)
                || _.Remarks.Contains(keywords)
                || _.Group.Contains(keywords)
                );
            }
            result.TotalCount = jobs.Count();

            jobs = jobs.OrderByDescending(_ => _.CreatedOn).Skip((pageNo - 1) * pageSize).Take(pageSize);

            result.RecordCount = jobs.Count();
            result.List = jobs.ToList();

            return Task.FromResult(result);
        }

        public Task<PagedResult<JobLog>> GetJobLogList(string jobId, int pageNo = 1, int pageSize = 10, string keywords = "", bool? isSucess = null)
        {
            var result = new PagedResult<JobLog>
            {
                PageNo = pageNo,
                PageSize = pageSize,
                List = new List<JobLog>()
            };

            var logs = _cronJobDbContext.JobLogs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(jobId))
            {
                logs = logs.Where(_ => _.JobId == jobId);
            }

            if (!string.IsNullOrWhiteSpace(keywords))
            {
                logs = logs.Where(_ =>
                _.ErrorMessage.Contains(keywords)
                || _.ResponseContent.Contains(keywords)
                );
            }
            result.TotalCount = logs.Count();

            if (isSucess != null)
            {
                logs = logs.Where(_ => _.IsSuccess == isSucess);
            }

            logs = logs.OrderByDescending(_ => _.StartTime).Skip((pageNo - 1) * pageSize).Take(pageSize);
            result.RecordCount = logs.Count();
            result.List = logs.ToList();

            return Task.FromResult(result);
        }
    }
}
