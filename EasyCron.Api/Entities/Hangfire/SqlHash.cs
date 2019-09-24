using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bayantu.Evos.Services.CronJob.Api.Entities.Hangfire
{
    [Table("Hangfire_JobParameter")]
    public class SqlHash
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public DateTime? ExpireAt { get; set; }
    }
}
