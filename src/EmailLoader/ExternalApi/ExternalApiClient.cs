using EmailLoader.Domain;
using System;
using System.Threading.Tasks;

namespace EmailLoader.ExternalApi
{
    public class ExternalApiClient : IExternalApiClient
    {
        static class Routes 
        {
            public const string Login = "v0/login";
            public const string AddCode = "v0/code/addFromEmail";
        }

        public ExternalApiClient(ExternalApiClientSettings settings)
        {
            this.settings = settings;
        }

        private readonly ExternalApiClientSettings settings;

        public bool IsLoggedIn { get; private set; } = false;

        public async Task AddCode(string code, int organizationId)
        {
            // todo:
        }

        private async Task Login()
        {
            var loginUrl = $"{settings.BaseUrl.TrimEnd('/')}/{Routes.Login}";

            // todo:
        }
    }

    public class ExternalApiClientException : Exception
    {
        public ExternalApiClientException(string message) : base(message)
        {
        }
    }
}
