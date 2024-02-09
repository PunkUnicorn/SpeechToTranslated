using System;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    async static Task Main(string[] args)
    {
        var outputLanguages = args.Length > 0
            ? args.Where(a => !a.StartsWith("--")).ToArray()
            : new[] { "en-GB" };

        var forceConsole = args.Length > 0
            ? args.Any(a => a == "--force-console")
            : false;

        var app = new ChurchSpeechToTranslated.Application(outputLanguages, forceConsole);
        AppDomain.CurrentDomain.ProcessExit += new EventHandler(app.OnProcessExit);

        await app.RunAsync();
    }

}