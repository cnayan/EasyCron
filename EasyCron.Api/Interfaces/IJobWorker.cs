using System.Threading.Tasks;

namespace EasyCron.Api.Interfaces
{
    public interface IJobWorker
    {
        Task DoWorkAsync(string jobId, string callAddress, string paramJson);
    }
}
