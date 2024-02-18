using Pastel;
using SpeechToTranslatedCommon;
using System.Drawing;
using System.Globalization;
using System.IO.Pipes;
using System.Text;
using TranslateWordsProcess;

namespace TranslateWordsConsole
{
    internal class Program
    {
        private static TranslatorHelper? translatorHelper;
        private static string? languageCode;
        private static int nextFreeLine = 0;
        private static Color baseColour = Color.FromArgb(160, 160, 160);
        private static FunkyColours funkyColours = new FunkyColours();
        private static Color previewColour = Color.FromArgb(0, 100, 100, 100);

        static async Task Main(string[] args)
        {
            if ((args?.Length ?? 0) == 0) throw new ArgumentException("Need parameter of the language code e.g. es");
            languageCode = args?[0] ?? throw new ArgumentException("Need parameter of the language code e.g. es");

            var appConfig = ConfigurationLoader.Load() ?? throw new InvalidProgramException("Unable to read appsettings.json");
            translatorHelper = new TranslatorHelper(appConfig, languageCode);

            Console.Title = "";
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = await translatorHelper.TranslateWordsAsync($"Translation into {new CultureInfo(languageCode).DisplayName}");


            using var client = new NamedPipeClientStream(".", string.Format(NamedMemoryPipe.PipeNameFormatString, languageCode), PipeDirection.In);

            Console.WriteLine($"{DateTime.Now.ToShortTimeString()} Connecting to server...");
            client.Connect();
            Console.WriteLine($"{DateTime.Now.ToShortTimeString()} Connected to server!");
            Console.Clear();

            try
            {
                // Read data through the pipe
                var ss = new MessageStreamer(client);

                string GetTicks()
                    => new String(DateTime.Now.Ticks.ToString().Reverse().ToArray()).Substring(0, 5);

                var otherLanguageFilenameFormatString = $"{DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-')}-{GetTicks()}_{{0}}.txt";

                Console.WriteLine();
                for (var message = ss.ReadString(); true; message = ss.ReadString())
                {
                    var isTranslationMessage = MessageStreamer.DecodeTranslationMessage(message, out var words, out var isIncremental, out var isFinalParagraph, out var offset);

                    if (!isTranslationMessage)
                    {
                        ProcessLayoutMessage(message);
                        continue;
                    }

                    if (words == null || words.Length == 0)
                        continue;

                    string saveWords;
                    if (words == "\n\n" || words == "\n")
                    {
                        saveWords = words;
                    }
                    else
                    {
                        var translation = words.Length > 0
                            ? await translatorHelper.TranslateWordsAsync(words)
                            : words;

                        if (isFinalParagraph)
                        {
                            UpdateFinalParagraph(words, translation);
                        }
                        else
                        {
                            if (!isIncremental)
                                UpdateAbsolute(words, translation);
                            else
                                UpdateIncremental(words, translation);
                        }

                        saveWords = translation;
                    }

                    if (isFinalParagraph)
                        File.AppendAllText(string.Format($"{TranslatorHelper.logpath}{otherLanguageFilenameFormatString}", languageCode), $"{saveWords}\n\n");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("".PadLeft(Console.WindowWidth, '-'));
                Console.Error.WriteLine(ex.Message.PadLeft(Console.WindowWidth-ex.Message.Length));
                Console.Error.WriteLine("".PadLeft(Console.WindowWidth, '-'));
            }
        }

        private static bool ProcessLayoutMessage(string message)
        {
            if (!MessageStreamer.DecodeLayoutMessage(message, out var count, out var index))
                return false;

            if (OperatingSystem.IsWindows())
            {
                Console.SetWindowSize(Console.LargestWindowWidth/count, Console.LargestWindowHeight);
                Console.WindowTop = 0;
            }

            return true;
        }

        private static void UpdateIncremental(string words, string translation)
        {
            WordWrapWrite(translation, previewColour);
        }

        private static void UpdateAbsolute(string words, string translation)
        {
            SetupConsoleToOverwriteWords();

            WordWrapWrite(translation, previewColour);
        }

        private static void UpdateFinalParagraph(string words, string translation)
        {
            SetupConsoleToOverwriteWords();

            WordWrapWrite(translation, funkyColours.MakeFunkyColour(baseColour, FunkyColours.GetWordsSeed(words)));
            Console.WriteLine("".PadLeft(Console.WindowWidth - Console.CursorLeft));

            Console.WriteLine("".PadLeft(Console.WindowWidth));
            nextFreeLine = Console.CursorTop;
        }

        private static void WordWrapWrite(string translation, Color color, Func<string, string>? pastelFunc=null)
        {
            foreach (var word in translation.Split())
            {
                var left = Console.CursorLeft;
                if (word.Length + left > Console.WindowWidth - 5)
                    Console.WriteLine("".PadLeft(Console.WindowWidth - left));

                if (Console.CursorLeft > 0 && word.Length > 0 && !word.StartsWith(" "))
                    Console.Write(" ");

                Console.Write(pastelFunc?.Invoke(word) ?? word.Pastel(color));

            }

            var x = Console.CursorLeft;
            Console.Write("".PadLeft(Console.WindowWidth - Console.CursorLeft));
            Console.CursorLeft = x;
        }

        private static void SetupConsoleToOverwriteWords()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = nextFreeLine;
        }
    }
}