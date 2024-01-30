using SpeechToTranslatedCommon;
using SpeechToTranslatorCommon;
using System.Globalization;
using System.IO.Pipes;
using System.Text;
using TranslateWordsProcess;

Console.Title = string.Join(", ", args);
Console.OutputEncoding = Encoding.UTF8;

if (args.Length != 2)
    throw new ArgumentException("Need parameters of '[language_code] [want_english_bool]'");

var languageCode = args[0];
var wantEnglish = bool.Parse(args[1]);

var appConfig = ConfigurationLoader.Load() ?? throw new InvalidProgramException("Unable to read appsettings.json");
var translatorHelper = new TranslatorHelper(appConfig, languageCode);

Console.Title = translatorHelper.TranslateWords($"Translation into {new CultureInfo(languageCode).DisplayName}");

using var client = new NamedPipeClientStream(".", string.Format(NamedMemoryPipe.PipeNameFormatString, languageCode), PipeDirection.In);

Console.WriteLine("Connecting to server...");
client.Connect();
Console.WriteLine("Connected to server!");
Console.Clear();

// Read and write data through the pipe
var ss = new StreamString(client);

var output = new ConsoleOutput(wantEnglish);

string GetTicks() => new String(DateTime.Now.Ticks.ToString().Reverse().ToArray()).Substring(0, 5);
string otherLanguageFilenameFormatString = $"{DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-')}-{GetTicks()}_{{0}}.txt";

for (var words = ss.ReadString(); true; words = ss.ReadString())
{
    if (words == null || words.Length == 0)
        continue;

    try
    {
        string saveWords;
        if (words == "\n\n")
        { 
            output.OutputFlow(words);
            saveWords=words;
        }
        else
        {
            var translation = translatorHelper.TranslateWords(words);
            output.OutputColumns(words, translation);
            saveWords=translation;
        }

        File.AppendAllText(string.Format($"{TranslatorHelper.logpath}{otherLanguageFilenameFormatString}", languageCode), saveWords);
    }
    catch (Exception e)
    {
        var fg = Console.ForegroundColor;
        try
        { 
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"{e.Message} for {languageCode}");
        }
        finally
        {
            Console.ForegroundColor = fg;
        }
    }
}
