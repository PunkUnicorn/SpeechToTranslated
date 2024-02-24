using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    async static Task Main(string[] args)
    {
        var languageArgs = args.Where(a => !a.StartsWith("--"));
        var outputLanguages = languageArgs.Any()
            ? languageArgs.ToArray()
            : new[] { "en-GB" };

        var forceConsole = args.Length > 0
            ? args.Any(a => a == "--force-console")
            : false;

        var app = new ChurchSpeechToTranslated.Application(outputLanguages, forceConsole);
        AppDomain.CurrentDomain.ProcessExit += new EventHandler(app.OnProcessExit);

        await app.RunAsync();
    }
    /*
xcopy ..\TranslateWordsGui\bin\Debug\net6.0-windows\*.* $(OutDir) /S /Y
xcopy ..\TranslateWordsConsole\bin\Debug\net6.0\*.* $(OutDir) /S /Y*/
}
