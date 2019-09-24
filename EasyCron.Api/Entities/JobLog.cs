using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyCron.Api.Entities
{
    [Table("Cron_Job_Log")]
    public class JobLog
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; }

        [MaxLength(36)]
        public string JobId { get; set; }

        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string ResponseContent { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 执行完成时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
