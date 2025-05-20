using DeepL;
using Microsoft.Extensions.Configuration;
using TranslateWordsProcess;

namespace Translate
{
    public class TranslateServiceDeepl : ITranslateService
    {
        private readonly Translator translator;

        private readonly string outputLanguage;

        private class DeepLTranslateResult : ITranslateResult
        {
            public string Words { get; set;}

            public dynamic? Result { get; set; }
        }

        public TranslateServiceDeepl(IConfiguration appConfig, string outputLanguage)
        {
            this.outputLanguage = outputLanguage;
            translator = new Translator(appConfig["translate.deepl.key"] ?? throw new InvalidOperationException("Unable to find translate.deepl.key."));
        }

        public async Task<ITranslateResult> TranslateWordsAsync(string words)
        {
            var result = await translator.TranslateTextAsync(words, "en", outputLanguage);

            return new DeepLTranslateResult{Words = result.Text, Result = result };
        }
    }
}
