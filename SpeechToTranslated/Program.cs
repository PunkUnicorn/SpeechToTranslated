using System.Threading.Tasks;
using DeepL;

class Program
{
    async static Task Main(string[] args)
    {
        var outputLanguage = args.Length > 0 ? args[0] : LanguageCode.Bulgarian.ToString();
        var app = new ChurchSpeechToTranslateor.Application(outputLanguage);
        await app.RunAsync();
    }
}