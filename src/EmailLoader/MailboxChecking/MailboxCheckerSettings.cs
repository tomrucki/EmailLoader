using Microsoft.Extensions.Configuration;
using System;

namespace EmailLoader
{
    public class MailboxCheckerSettings
    {
        public string EmailLogin { get; set; }
        public string EmailPassword { get; set; }
        public string EmailServerDomain { get; set; }
        public int EmailServerPort { get; set; }
    }

    public static class MailboxCheckerSettingsExt
    {
        public static MailboxCheckerSettings GetMailboxCheckerSettings(this IConfigurationRoot config) => new MailboxCheckerSettings
        {
            EmailLogin = config[nameof(MailboxCheckerSettings.EmailLogin)],
            EmailPassword = config[nameof(MailboxCheckerSettings.EmailPassword)],
            EmailServerDomain = config[nameof(MailboxCheckerSettings.EmailServerDomain)],
            EmailServerPort = Convert.ToInt32(config[nameof(MailboxCheckerSettings.EmailServerPort)]),
        };
    }
}
