using System.Linq;
using System.Threading.Tasks;
using DeepL;

class Program
{
    async static Task Main(string[] args)
    {
        var defaultLanguage = LanguageCode.Bulgarian.ToString();
        var noEnglishSwitch = "--no-english";

        var outputLanguage = args.Length > 0 
            ? args.Where(a => !a.StartsWith("--")).FirstOrDefault() ?? defaultLanguage
            : defaultLanguage;

        var wantEnglish = args.Length > 0
            ? !(args.Any(a => a == noEnglishSwitch)) 
            : true;

        var app = new ChurchSpeechToTranslator.Application(outputLanguage, wantEnglish);
        await app.RunAsync();
    }
}