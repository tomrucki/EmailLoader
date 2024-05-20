using Microsoft.Extensions.Configuration;

namespace EmailLoader.EmailProcessing
{
    public class EmailProcessorSettings
    {
        public string ExternalApiBaseUrl { get; set; }
        public string ExternalApiLogin { get; set; }
        public string ExternalApiPassword { get; set; }
    }

    public static class EmailProcessorSettingsExt 
    {
        public static EmailProcessorSettings GetEmailProcessorSettings(this IConfigurationRoot config) => new EmailProcessorSettings
        {
            ExternalApiBaseUrl = config[nameof(EmailProcessorSettings.ExternalApiBaseUrl)],
            ExternalApiLogin = config[nameof(EmailProcessorSettings.ExternalApiLogin)],
            ExternalApiPassword = config[nameof(EmailProcessorSettings.ExternalApiPassword)],
        };
    }
}
