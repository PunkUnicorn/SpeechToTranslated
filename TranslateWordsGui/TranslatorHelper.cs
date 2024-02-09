//using DeepL;
//using DeepL.Model;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;

//namespace TranslateWordsProcess
//{
//    public class TranslatorHelper
//    {
//        private readonly Translator translator;
//        private readonly bool deepLLog;

//        private readonly string outputLanguage;
//        public const string logpath = ".\\Logs\\";

//        public TranslatorHelper(IConfiguration appConfig, string outputLanguage)
//        {
//            this.outputLanguage = outputLanguage;
//            translator = new Translator(appConfig["translate.deepl.key"] ?? throw new InvalidOperationException("Unable to find translate.deepl.key."));
//            deepLLog = bool.Parse(appConfig["translate.log"] ?? false.ToString());
//        }

//        public async Task<string> TranslateWords(string words)
//        {
//            if (words.Length == 0)
//                return words;

//            if (words == "\n\n" || words == "\n")
//                return words;

//            if (outputLanguage.StartsWith("en"))
//                return words;

//            TextResult result = await translator.TranslateTextAsync(
//                            words,
//                            LanguageCode.English,
//                            outputLanguage);

//            if (deepLLog)
//            {
//                var dump = new { Date = DateTime.Now.ToLongDateString(), Time = DateTime.Now.ToLongTimeString(), Input = words, Result = result };
//                File.AppendAllText($"{logpath}deepl_{outputLanguage}.log", $",{JsonConvert.SerializeObject(dump, Formatting.Indented)}");
//            }

//            return result.Text;
//        }
//    }
//}
