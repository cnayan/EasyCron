using System.ComponentModel.DataAnnotations.Schema;

namespace Bayantu.Evos.Services.CronJob.Api.Entities.Hangfire
{
    [Table("Hangfire_JobParameter")]
    public class JobParameter
    {
        public int JobId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
