using EasyCron.Api.Interfaces;
using EasyCron.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Bayantu.Evos.Services.CronJob.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        public readonly ICronJobService _cronJobService;

        public JobController(ICronJobService cronJobService)
        {
            _cronJobService = cronJobService;
        }

        [Route("addOrUpdate")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateAsync([FromBody] AddOrUpdateJobCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.ParamJson))
            {
                command.ParamJson = JsonConvert.SerializeObject(new { });
            }

            ValidateAddOrUpdateJobCommand(command);

            string jobId = string.Empty;
            switch (command.JobType)
            {
                case JobType.RecurringJob:
                    jobId = await _cronJobService.AddOrUpdateRecurringJobAsync(command.JobId, command.JobName, command.CallAddress, command.ParamJson, command.Cron, command.Group, command.Remarks, string.Empty);
                    break;
                case JobType.FireAndForgetJob:
                    jobId = await _cronJobService.AddFireAndForgetJobAsync(command.JobName, command.CallAddress, command.ParamJson, command.Group, command.Remarks);
                    break;
                case JobType.DelayedJob:
                    jobId = await _cronJobService.AddDelayedJobAsync(command.JobName, command.CallAddress, command.ParamJson, command.DelayMinutes, command.Group, command.Remarks);
                    break;
                case JobType.ContinuationsJob:
                    jobId = await _cronJobService.AddContinuationsJobAsync(command.ParentJobId, command.JobName, command.CallAddress, command.ParamJson, command.Group, command.Remarks);
                    break;
                default:
                    break;
            }

            return Ok(new { jobId });
        }


        [Route("delete")]
        [HttpPost]
        public async Task DeleteAsync([FromBody] JToken command)
        {
            string id = command.Value<string>("id");
            await _cronJobService.DeleteAsync(id);
        }

        [Route("disable")]
        [HttpPost]
        public async Task DisableRecurringJobAsync([FromBody] JToken command)
        {
            string id = command.Value<string>("id");
            await _cronJobService.DisableRecurringJobAsync(id);
        }

        [Route("active")]
        [HttpPost]
        public async Task ActiveRecurringJobAsync([FromBody] JToken command)
        {
            string id = command.Value<string>("id");
            await _cronJobService.ActiveRecurringJobAsync(id);
        }

        [Route("trigger")]
        [HttpPost]
        public async Task TriggerRecurringJobAsync([FromBody] JToken command)
        {
            string id = command.Value<string>("id");
            await _cronJobService.TriggerRecurringJobAsync(id);
        }

        [Route("getJobList")]
        [HttpGet]
        public async Task<IActionResult> GetJobListAsync([FromQuery] JobListQuery query)
        {
            var result = await _cronJobService.GetJobListAsync(query.PageNo, query.PageSize, query.Keywords);
            return Ok(result);
        }

        [Route("getJobLogList")]
        [HttpGet]
        public async Task<IActionResult> GetJobLogListAsync([FromQuery] JobLogListQuery query)
        {
            var result = await _cronJobService.GetJobLogList(query.JobId, query.PageNo, query.PageSize, query.Keywords, query.IsSuccess);
            return Ok(result);
        }

        #region private method
        private void ValidateAddOrUpdateJobCommand(AddOrUpdateJobCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.CallAddress))
            {
                throw new InvalidOperationException("callAddress is required");
            }

            if (string.IsNullOrWhiteSpace(command.JobName))
            {
                throw new InvalidOperationException("jobName is required");
            }

            try
            {
                JObject.Parse(command.ParamJson);
            }
            catch
            {
                throw new InvalidOperationException("paramJson is not valid  object json string");
            }

            switch (command.JobType)
            {
                case JobType.None:
                    throw new InvalidOperationException("invalid jobType");
                case JobType.RecurringJob:

                    if (string.IsNullOrWhiteSpace(command.Cron))
                        throw new InvalidOperationException("cron is required for recurringJob");
                    break;
                case JobType.FireAndForgetJob:
                    break;
                case JobType.DelayedJob:
                    if (command.DelayMinutes <= 0)
                        throw new InvalidOperationException("delayMinutes is required for delayedJob");
                    break;
                case JobType.ContinuationsJob:
                    if (string.IsNullOrWhiteSpace(command.ParentJobId))
                        throw new InvalidOperationException("parentJobId is required for continuationsJob");
                    break;
                default:
                    throw new InvalidOperationException("invalid jobType");
            }
        }
        #endregion

    }
}
