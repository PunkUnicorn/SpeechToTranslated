using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    async static Task Main(string[] args)
    {
        var defaultLanguage = "bg";
        var englishSwitch = "--horizontal-with-english";

        var outputLanguages = args.Length > 0 
            ? args.Where(a => !a.StartsWith("--")).ToArray() 
            : new [] { defaultLanguage };

        var wantEnglish = args.Length > 0
            ? (args.Any(a => a == englishSwitch)) 
            : true;
   
        var app = new ChurchSpeechToTranslator.Application(outputLanguages, wantEnglish);
        AppDomain.CurrentDomain.ProcessExit += new EventHandler(app.OnProcessExit);

        await app.RunAsync();
    }

}