using System.Threading.Tasks;

namespace EmailLoader.Domain
{
    public interface IExternalApiClient
    {
        Task AddCode(string code, int organizationId);
    }
}
