using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bayantu.Evos.Services.CronJob.Api.Entities.Hangfire
{
    [Table("Hangfire_JobParameter")]
    public class SqlState
    {
        public int JobId { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Data { get; set; }
    }
}
