namespace EasyCron.UI.Models
{
    public class AddOrUpdateJobCommand
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public string JobId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 调用地址
        /// </summary>
        public string CallAddress { get; set; }

        /// <summary>
        /// 参数JSON
        /// </summary>
        public string ParamJson { get; set; }

        /// <summary>
        /// Job类型
        /// </summary>
        public JobType JobType { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// 延迟多少分钟执行
        /// </summary>
        public long DelayMinutes { get; set; }

        /// <summary>
        /// 父级任务Id
        /// </summary>
        public string ParentJobId { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
