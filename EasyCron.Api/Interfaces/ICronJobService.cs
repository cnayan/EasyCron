using EasyCron.Api.Entities;
using EasyCron.Api.Models;
using System.Threading.Tasks;

namespace EasyCron.Api.Interfaces
{
    public interface ICronJobService
    {
        /// <summary>
        /// 新增或者修改循环任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="jobName"></param>
        /// <param name="callAddress"></param>
        /// <param name="paramJson"></param>
        /// <param name="frequency"></param>
        /// <param name="cron"></param>
        /// <returns></returns>
        Task<string> AddOrUpdateRecurringJobAsync(string jobId, string jobName, string callAddress, string paramJson, string cron, string group, string remarks, string createdBy);


        /// <summary>
        /// 新增或修改一次性任务
        /// </summary>
        /// <param name="jobId">任务id</param>
        /// <param name="jobName">任务名称</param>
        /// <param name="callAddress">调用地址</param>
        /// <param name="paramJson">参数json</param>
        /// <returns></returns>
        Task<string> AddFireAndForgetJobAsync(string jobName, string callAddress, string paramJson, string group, string remarks);

        /// <summary>
        /// 新增一次性延时任务
        /// </summary>
        /// <param name="jobId">任务id</param>
        /// <param name="jobName">任务名称</param>
        /// <param name="callAddress">调用地址</param>
        /// <param name="paramJson">参数json</param>
        /// <param name="delayMinutes">延迟秒数</param>
        /// <returns></returns>
        Task<string> AddDelayedJobAsync(string jobName, string callAddress, string paramJson, long delayMinutes, string group, string remarks);

        /// <summary>
        /// 新增后续任务
        /// </summary>
        /// <param name="jobId">任务id</param>
        /// <param name="jobName">任务名称</param>
        /// <param name="callAddress">调用地址</param>
        /// <param name="paramJson">参数Json</param>
        /// <returns></returns>
        Task<string> AddContinuationsJobAsync(string parentJobId, string jobName, string callAddress, string paramJson, string group, string remarks);

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task DeleteAsync(string jobId);

        /// <summary>
        /// 禁用循环任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task DisableRecurringJobAsync(string jobId);

        /// <summary>
        /// 启用循环任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task ActiveRecurringJobAsync(string jobId);

        /// <summary>
        /// 立即执行一次循环任务
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task TriggerRecurringJobAsync(string jobId);

        /// <summary>
        /// 查询任务列表
        /// </summary>
        /// <returns></returns>
        Task<PagedResult<Job>> GetJobListAsync(int pageNo = 1, int pageSize = 10, string keywords = "");

        /// <summary>
        /// 查询执行日志列表
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Task<PagedResult<JobLog>> GetJobLogList(string jobId, int pageNo = 1, int pageSize = 10, string keywords = "", bool? isSucess = null);
    }
}
