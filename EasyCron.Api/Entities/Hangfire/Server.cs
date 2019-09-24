using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bayantu.Evos.Services.CronJob.Api.Entities.Hangfire
{
    [Table("Hangfire_JobParameter")]
    public class Server
    {
        public string Id { get; set; }
        public string Data { get; set; }
        public DateTime LastHeartbeat { get; set; }
    }
}