using EmailLoader.EmailProcessing;

namespace EmailLoader.ExternalApi
{
    public class ExternalApiClientSettings
    {
        public string BaseUrl { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public static class ExternalApiClientSettingsExt
    {
        public static ExternalApiClientSettings GetExternalApiClientSettings(this EmailProcessorSettings settings) => new ExternalApiClientSettings
        {
            BaseUrl = settings.ExternalApiBaseUrl,
            Login = settings.ExternalApiLogin,
            Password = settings.ExternalApiPassword,
        };
    }
}
