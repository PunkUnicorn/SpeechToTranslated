using SpeechToTranslatedCommon;
using System.Globalization;
using System.IO.Pipes;
using System.Text;
using System.Text.RegularExpressions;
using TranslateWordsProcess;

Console.Title = "";
Console.OutputEncoding = Encoding.UTF8;

if (args.Length != 2)
    throw new ArgumentException("Need parameters: '[language_code] [want_english_bool]'");

var languageCode = args[0];
var wantEnglish = bool.Parse(args[1]);

var appConfig = ConfigurationLoader.Load() ?? throw new InvalidProgramException("Unable to read appsettings.json");
var translatorHelper = new TranslatorHelper(appConfig, languageCode);

Console.Title = translatorHelper.TranslateWords($"Translation into {new CultureInfo(languageCode).DisplayName}");

using var client = new NamedPipeClientStream(".", string.Format(NamedMemoryPipe.PipeNameFormatString, languageCode), PipeDirection.In);

Console.WriteLine($"{DateTime.Now.ToShortTimeString()} Connecting to server...");
client.Connect();
Console.WriteLine($"{DateTime.Now.ToShortTimeString()} Connected to server!");
Console.Clear();

// Read data through the pipe
var ss = new StreamString(client);

var output = new ConsoleOutputAgainstOffset();

string GetTicks() 
    => new String(DateTime.Now.Ticks.ToString().Reverse().ToArray()).Substring(0, 5);

var otherLanguageFilenameFormatString = $"{DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-')}-{GetTicks()}_{{0}}.txt";
var offsetRegex = new Regex("^offset:(\\d*):(.*$)", RegexOptions.Compiled);

for (var words = ss.ReadString(); true; words = ss.ReadString())
{
    if (words == null || words.Length == 0)
        continue;
    
    try
    {

        string saveWords;
        if (words == "\n\n" || words == "\n")
        { 
            output.OutputLineBreak();
            saveWords=words;
        }
        else if (words.StartsWith("offset:"))
        {
            var offsetCapture = offsetRegex.Match(words);
            var offset = ulong.Parse(offsetCapture.Groups[1].Value);
            words = offsetCapture.Groups[2].Value;
            var translation = translatorHelper.TranslateWords(words);
            output.OutputFlow(false, true, offset, translation);
            saveWords = translation;
        }
        else
        {
            throw new ArgumentException($"No offset for:'{words}'");
        }

        File.AppendAllText(string.Format($"{TranslatorHelper.logpath}{otherLanguageFilenameFormatString}", languageCode), saveWords);
    }
    catch (Exception e)
    {
        var fg = Console.ForegroundColor;
        try
        { 
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(e.Message);
        }
        finally
        {
            Console.ForegroundColor = fg;
        }
    }
}
