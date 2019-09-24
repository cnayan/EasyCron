using EasyCron.Api.DbContextes;
using EasyCron.Api.Entities;
using EasyCron.Api.Interfaces;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EasyCron.Api.Implements
{
    public class JobWorker : IJobWorker
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CronJobDbContext _cronJobDbContext;
        public JobWorker(IHttpClientFactory httpClientFactory, CronJobDbContext cronJobDbContext)
        {
            _httpClientFactory = httpClientFactory;
            _cronJobDbContext = cronJobDbContext;
        }

        public async Task DoWorkAsync(string jobId, string callAddress, string paramJson)
        {
            paramJson = string.IsNullOrWhiteSpace(paramJson) ? string.Empty : paramJson;

            var job = await _cronJobDbContext.Jobs.FindAsync(jobId);

            if (job == null || !job.IsActive) return;

            var log = new JobLog
            {
                Id = Guid.NewGuid().ToString(),
                JobId = jobId,
                StartTime = DateTime.Now,
                IsSuccess = true
            };

            using (var client = _httpClientFactory.CreateClient("http_client"))
            {
                try
                {
                    var res = await client.PostAsync(callAddress,
                        new StringContent(paramJson, Encoding.UTF8, "application/json"));

                    log.IsSuccess = res.IsSuccessStatusCode;
                    log.ResponseContent = await res.Content.ReadAsStringAsync();
                    log.EndTime = DateTime.Now;
                }
                catch (Exception e)
                {
                    log.IsSuccess = false;
                    log.ErrorMessage = e.Message;
                    log.EndTime = DateTime.Now;
                }
                finally
                {
                    job.LastExecuteTime = DateTime.Now;
                    job.TriggeredTimes++;
                    await _cronJobDbContext.JobLogs.AddAsync(log);
                    await _cronJobDbContext.SaveChangesAsync();
                }
            }
        }
    }
}
