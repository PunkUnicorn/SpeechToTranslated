using Microsoft.Extensions.Configuration;

namespace SpeechToTranslatorCommon
{
    public static class ConfigurationLoader
    {
        public static IConfiguration Load()
        { 
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true)
                .AddJsonFile($"appsettings.Church.json", optional: true)
                .Build();
        }
    }
}
