using CronExpressionDescriptor;
using EasyCron.Api.Interfaces;
using System.Threading.Tasks;

namespace EasyCron.Api.Implements
{
    public class CronTranslator : ICronTranslator
    {
        public Task<string> TranslateCron2DescriptionAsync(string cron)
        {
            var result = ExpressionDescriptor.GetDescription(cron
                , new Options() { Locale = "zh-Hans" });

            return Task.FromResult(result);
        }
    }
}
