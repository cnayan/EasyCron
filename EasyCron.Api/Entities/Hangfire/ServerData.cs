using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bayantu.Evos.Services.CronJob.Api.Entities.Hangfire
{
    [Table("Hangfire_JobParameter")]
    public class ServerData
    {
        public int WorkerCount { get; set; }
        public string[] Queues { get; set; }
        public DateTime? StartedAt { get; set; }
    }
}