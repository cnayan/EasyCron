using System.Threading.Tasks;

namespace EasyCron.Api.Interfaces
{
    public interface ICronTranslator
    {
        /// <summary>
        /// 将Cron表达式翻译为描述
        /// </summary>
        /// <param name="cron"></param>
        /// <returns></returns>
        Task<string> TranslateCron2DescriptionAsync(string cron);
    }
}
