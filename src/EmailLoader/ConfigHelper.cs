using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace EmailLoader
{
    public static class ConfigHelper 
    {
        public static IConfigurationRoot Build(ExecutionContext context) => new ConfigurationBuilder()
            .SetBasePath(context.FunctionAppDirectory)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        public static string GetStorageConnectionString(this IConfigurationRoot configuration) 
            => configuration["AzureWebJobsStorage"];
    }
}
