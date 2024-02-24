using DeepL;
using DeepL.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace TranslateWordsProcess
{
    public class TranslatorHelper
    {
        private readonly Translator translator;
        private readonly bool deepLLog;
        private readonly int cacheExpireMinutes;
        private readonly MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions { });

        private readonly string outputLanguage;
        public const string logpath = ".\\Logs\\";

        public TranslatorHelper(IConfiguration appConfig, string outputLanguage)
        {
            this.outputLanguage = outputLanguage;
            translator = new Translator(appConfig["translate.deepl.key"] ?? throw new InvalidOperationException("Unable to find translate.deepl.key."));
            deepLLog = bool.Parse(appConfig["translate.log"] ?? false.ToString());
            cacheExpireMinutes = int.Parse(appConfig["translate.cache.expiretimeminutes"] ?? "10");
        }

        public async Task<string> TranslateWordsAsync(string words)
        {
            if (words.Length == 0)
                return words;

            if (words == "\n\n" || words == "\n")
                return words;

            if (outputLanguage.StartsWith("en"))
                return words;

            TextResult? result = null;
            var text = await memoryCache.GetOrCreateAsync<string>(words.GetHashCode(), async (c) => 
                { 
                    c.SetSlidingExpiration(TimeSpan.FromMinutes(cacheExpireMinutes));
                    result = await translator.TranslateTextAsync(
                                    words,
                                    LanguageCode.English,
                                    outputLanguage);

                    return result.Text;
                });

                if (deepLLog)
                { 
                    object dump = result is null
                        ? new { Date = DateTime.Now.ToLongDateString(), Time = DateTime.Now.ToLongTimeString(), Input = words, IsCached = true }
                        : new { Date = DateTime.Now.ToLongDateString(), Time = DateTime.Now.ToLongTimeString(), Input = words, Result = result };

                    File.AppendAllText($"{logpath}deepl_{outputLanguage}.log", $",{JsonConvert.SerializeObject(dump, Formatting.Indented)}");
                }

            return text!;
        }
    }
}
