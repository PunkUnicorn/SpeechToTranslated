using DeepL;
using Microsoft.Extensions.Configuration;
using TranslateWordsProcess;

namespace Translate
{
    public class TranslateServiceDeepl : ITranslateService
    {
        private readonly Translator translator;
        //private readonly bool deepLLog;
        //private readonly int cacheExpireMinutes;
        //private readonly string inputLanguage;
        //private readonly MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions { });

        private readonly string outputLanguage;
        //public const string logpath = ".\\Logs\\";

        private class DeepLTranslateResult : ITranslateResult
        {
            public string Words { get; set;}

            public dynamic? Result { get; set; }
        }

        public TranslateServiceDeepl(IConfiguration appConfig, string outputLanguage)
        {
            this.outputLanguage = outputLanguage;
            translator = new Translator(appConfig["translate.deepl.key"] ?? throw new InvalidOperationException("Unable to find translate.deepl.key."));
            //deepLLog = bool.Parse(appConfig["translate.log"] ?? false.ToString());
            //cacheExpireMinutes = int.Parse(appConfig["translate.cache.expiretimeminutes"] ?? "10");
            //inputLanguage = "en"; //appConfig["speechtotext.input.language"] ?? LanguageCode.English;
        }

        public async Task<ITranslateResult> TranslateWordsAsync(string words)
        {
            var result = await translator.TranslateTextAsync(words, "en", outputLanguage);

            return new DeepLTranslateResult{Words = result.Text, Result = result };
        }
    }
}
