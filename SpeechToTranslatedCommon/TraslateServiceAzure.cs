using Azure;
using Azure.AI.Translation.Text;
using DeepL;
using Microsoft.Extensions.Configuration;
using TranslateWordsProcess;

namespace Translate
{
    public class TranslateServiceAzure : ITranslateService
    {
        private readonly string outputLanguage;
        private readonly TextTranslationClient translator;
        //private readonly bool log;
        //private readonly int cacheExpireMinutes;
        //private readonly string inputLanguage;
        //private readonly MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions { });

        private class AzureTranslateResult : ITranslateResult
        {
            public string? Words { get; set; }

            public dynamic Result { get; set; }
        }

        public TranslateServiceAzure(IConfiguration appConfig, string outputLanguage)
        {
            this.outputLanguage = outputLanguage;
            translator = new TextTranslationClient(new AzureKeyCredential(appConfig["translate.azure.key"]??throw new Exception("translate.azure.key not found")), 
                new Uri(@"https://api.cognitive.microsofttranslator.com/"),
                appConfig["translate.azure.region"] ?? throw new Exception("translate.azure.region not found"),
                new TextTranslationClientOptions()
                );
            //log = bool.Parse(appConfig["translate.log"] ?? false.ToString());
            //cacheExpireMinutes = int.Parse(appConfig["translate.cache.expiretimeminutes"] ?? "10");
            //inputLanguage = "en"; //appConfig["speechtotext.input.language"] ?? LanguageCode.English;
        }
        public async Task<ITranslateResult> TranslateWordsAsync(string words)
        {
            //var output = await translator.TranslateAsync($"to={outputLanguage}", words, "en");
            var output = await translator.TranslateAsync($"{outputLanguage}", words, "en");

            var text = output.Value.FirstOrDefault()?.Translations?.FirstOrDefault()?.Text;// .Transliteration?.Text;
            return new AzureTranslateResult { Words = text??null!, Result = output };
        }
    }
}