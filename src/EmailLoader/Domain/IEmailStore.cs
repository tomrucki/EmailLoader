using System.Threading.Tasks;

namespace EmailLoader.Domain
{
    public interface IEmailStore 
    {
        Task<string> Add(AddEmail emailData);
        Task<Email> GetEmail(string emailId, bool includeAttachments = true);
    }
}
