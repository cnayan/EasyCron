using System;

namespace EasyCron.UI.Models
{
    public class JobLog
    {
        public string Id { get; set; }

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
