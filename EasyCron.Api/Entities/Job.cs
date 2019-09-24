using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyCron.Api.Entities
{
    [Table("Cron_Job")]
    public class Job
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; }

        /// <summary>
        /// hangfire任务Id
        /// </summary>
        [MaxLength(36)]
        [Required]
        public string HangfireJobId { get; set; }

        /// <summary>
        /// Job名称
        /// </summary>
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        [MaxLength(10)]
        public string Cron { get; set; }

        /// <summary>
        /// Cron表达式描述
        /// </summary>
        [MaxLength(200)]
        public string CronDescription { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// 延迟分钟数，延时任务专用
        /// </summary>
        public long DelayMinutes { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        [MaxLength(50)]
        public string Group { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 调用地址
        /// </summary>
        [MaxLength(2000)]
        public string CallAddress { get; set; }

        /// <summary>
        /// 参数JSON
        /// </summary>
        public string ParamJson { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 父级任务Id，后续任务专用
        /// </summary>
        [MaxLength(36)]
        public string ParentJobId { get; set; }

        /// <summary>
        /// 任务执行日志
        /// </summary>
        public List<JobLog> JobLogs { get; set; }

        /// <summary>
        /// 是否激活状态
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 最近一次执行时间
        /// </summary>
        public DateTime? LastExecuteTime { get; set; }

        /// <summary>
        /// 任务创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public long TriggeredTimes { get; set; }
    }
}
