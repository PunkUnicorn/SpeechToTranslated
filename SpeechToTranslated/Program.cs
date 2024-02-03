using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    async static Task Main(string[] args)
    {
        var defaultLanguage = "bg";

        var outputLanguages = args.Length > 0 
            ? args
            : new [] { defaultLanguage, "en-GB" };

   
        var app = new ChurchSpeechToTranslated.Application(outputLanguages);
        AppDomain.CurrentDomain.ProcessExit += new EventHandler(app.OnProcessExit);

        await app.RunAsync();
    }

}