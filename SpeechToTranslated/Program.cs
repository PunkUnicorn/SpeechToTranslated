using System.Threading.Tasks;

class Program
{
    async static Task Main(string[] args)
    {
        var app = new ChurchSpeechToTranslateor.Application();
        await app.RunAsync();
    }
}