using Microsoft.Extensions.Configuration;

namespace SpeechToTranslatedCommon
{
    public static class ConfigurationLoader
    {
        public static IConfiguration Load()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true)
                .AddJsonFile("appsettings.Church.json", optional: true)
                .Build();
        }
    }
}
