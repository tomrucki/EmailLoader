using System.Threading.Tasks;

namespace EmailLoader.Storage.Emails
{
    public interface IEmailProcessorQueue 
    {
        Task Enqueue(string storedEmailId);
    }
}
