using EasyCron.UI.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyCron.UI.Apis
{
    public class CronJobApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CronJobApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task SubmitJobAsync(Job job)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = "http://localhost:10000/api/job/addOrUpdate";

            var command = new AddOrUpdateJobCommand
            {
                CallAddress = job.CallAddress,
                Cron = job.Cron,
                Group = job.Group,
                JobId = string.Empty,
                JobName = job.Name,
                JobType = JobType.RecurringJob,
                ParamJson = job.ParamJson,
                Remarks = job.Remarks
            };
            var json = System.Text.Json.JsonSerializer.Serialize(command);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(url, stringContent);
        }

        public async Task<PagedResult<Job>> GetJobList()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = "http://localhost:10000/api/job/getJobList";
            var result = await httpClient.GetAsync(url);
            var resultString = await result.Content.ReadAsStringAsync();
            var jobs = JsonConvert.DeserializeObject<PagedResult<Job>>(resultString);
            return jobs;
        }

        public async Task<PagedResult<JobLog>> GetJobLogsList()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = "http://localhost:10000/api/job/getJobLogList";
            var result = await httpClient.GetAsync(url);
            var resultString = await result.Content.ReadAsStringAsync();
            var logs = JsonConvert.DeserializeObject<PagedResult<JobLog>>(resultString);
            return logs;
        }
    }
}
