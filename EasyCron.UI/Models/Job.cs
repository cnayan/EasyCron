using System;
using System.ComponentModel.DataAnnotations;

namespace EasyCron.UI.Models
{
    public class Job
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [Required(ErrorMessage ="任务名称必填")]
        [MaxLength(50,ErrorMessage ="任务名称超长")]
        public string Name { get; set; }

        /// <summary>
        /// 调用地址
        /// </summary>
        [Required(ErrorMessage = "调用地址必填")]
        [Url(ErrorMessage ="非法地址")]
        [MaxLength(500, ErrorMessage = "任务名称超长")]
        public string CallAddress { get; set; }

        /// <summary>
        /// 参数JSON
        /// </summary>
        public string ParamJson { get; set; }

        /// <summary>
        /// Job类型
        /// </summary>
        [Required]
        public JobType JobType { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        [Required(ErrorMessage ="Cron表达式必填")]
        public string Cron { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        [Required(ErrorMessage ="分组必填")]
        public string Group { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Cron表达式描述
        /// </summary>
        [MaxLength(200)]
        public string CronDescription { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [MaxLength(50)]
        public string CreatedBy { get; set; }

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
    }
}
