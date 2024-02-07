using System;
using System.Threading.Tasks;

class Program
{
    async static Task Main(string[] args)
    {
        var outputLanguages = args.Length > 0
            ? args
            : new[] { "en-GB" };

        var app = new ChurchSpeechToTranslated.Application(outputLanguages);
        AppDomain.CurrentDomain.ProcessExit += new EventHandler(app.OnProcessExit);

        await app.RunAsync();
    }

}