using DeepL;
using DeepL.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TranslateWordsProcess;

namespace Translate
{
    public class TranslateService : ITranslateService
    {
        private readonly ITranslateService translateService;
        private readonly bool deepLLog;
        private readonly int cacheExpireMinutes;
        private readonly string inputLanguage;
        private readonly MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions { });

        private readonly string outputLanguage;
        public const string logpath = ".\\Logs\\";

        private class TranslationNoResult : ITranslateResult
        {
            public string Words { get; set; }

            public dynamic Result { get; set; }
        }
           
        public TranslateService(IConfiguration appConfig, string outputLanguage)
        {
            this.outputLanguage = outputLanguage;

            if (string.IsNullOrWhiteSpace(appConfig["translate.deepl.key"]))
                translateService = new TranslateServiceAzure(appConfig, outputLanguage);
            else
                translateService = new TranslateServiceDeepl(appConfig, outputLanguage);

            //translator = new Translator(appConfig["translate.deepl.key"] ?? throw new InvalidOperationException("Unable to find translate.deepl.key."));
            deepLLog = bool.Parse(appConfig["translate.log"] ?? false.ToString());
            cacheExpireMinutes = int.Parse(appConfig["translate.cache.expiretimeminutes"] ?? "10");
            inputLanguage = "en"; //appConfig["speechtotext.input.language"] ?? LanguageCode.English;
        }

        public async Task<ITranslateResult> TranslateWordsAsync(string words)
        {
            if (words.Length == 0)
                return new TranslationNoResult { Words = words, Result = new { } };

            if (words == "\n\n" || words == "\n")
                return new TranslationNoResult { Words = words, Result = new { } };

            if (outputLanguage.StartsWith("en"))
                return new TranslationNoResult { Words = words, Result = new { } };

            bool isCached = true;
            var output = await memoryCache.GetOrCreateAsync(words.GetHashCode(), async (c) => 
            {
                isCached = false;
                ITranslateResult? result = null;
                c.SetSlidingExpiration(TimeSpan.FromMinutes(cacheExpireMinutes));
                    result = await translateService.TranslateWordsAsync(words);

                return result;
             });

            if (deepLLog)
            {
                object dump = isCached
                    ? new { Date = DateTime.Now.ToLongDateString(), Time = DateTime.Now.ToLongTimeString(), Input = words, IsCached = true }
                    : new { Date = DateTime.Now.ToLongDateString(), Time = DateTime.Now.ToLongTimeString(), Input = words, output };

                File.AppendAllText($"{logpath}deepl_{outputLanguage}.log", $",{JsonConvert.SerializeObject(dump, Formatting.Indented)}");
            }

            return output!;
        }
    }
}
