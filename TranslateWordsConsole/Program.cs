using Pastel;
using SpeechToTranslatedCommon;
using System.Drawing;
using System.Globalization;
using System.IO.Pipes;
using System.Text;
using Translate;
using TranslateWordsProcess;

namespace TranslateWordsConsole
{
    internal class Program
    {
        private static ITranslateService? translateService;
        private static string? languageCode;
        private static int nextFreeLine = 0;
        private static Color baseColour = Color.FromArgb(160, 160, 160);
        private static FunkyColours funkyColours = new FunkyColours();
        private static Color previewColour = Color.FromArgb(0, 100, 100, 100);
        private static BroadcastHelper broadcastHelper;

        // send the colour code only
        // send the recieved time
        // use a message queue?
        // client renders with signalR?

        static async Task Main(string[] args)
        {
            if ((args?.Length ?? 0) == 0) throw new ArgumentException("Need parameter of the language code e.g. es");
            languageCode = args?[0] ?? throw new ArgumentException("Need parameter of the language code e.g. es");

            var appConfig = ConfigurationLoader.Load() ?? throw new InvalidProgramException("Unable to read appsettings.json");
            translateService = new TranslateService(appConfig, languageCode);

            Console.Title = "";
            Console.OutputEncoding = Encoding.UTF8;
            var result = await translateService.TranslateWordsAsync($"Translation into {new CultureInfo(languageCode).DisplayName}");
            Console.Title = result.Words;

            broadcastHelper = new BroadcastHelper(appConfig, languageCode);

            using var client = new NamedPipeClientStream(".", string.Format(NamedMemoryPipe.PipeNameFormatString, languageCode), PipeDirection.In);

            Console.WriteLine($"{DateTime.Now.ToShortTimeString()} Connecting to server...");
            client.Connect();
            Console.WriteLine($"{DateTime.Now.ToShortTimeString()} Connected to server!");
            Console.Clear();

            try
            {
                // Read data through the pipe
                var ss = new InterProcessMessageStreamer(client);

                string GetTicks()
                    => new String(DateTime.Now.Ticks.ToString().Reverse().ToArray()).Substring(0, 5);

                var otherLanguageFilenameFormatString = $"{DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-')}-{GetTicks()}_{{0}}.txt";

                Console.WriteLine();
                for (var message = ss.ReadString(); true; message = ss.ReadString())
                {
                    var isTranslationMessage = InterProcessMessageStreamer.DecodeTranslationMessage(message, 
                        out var words, 
                        out var isIncremental, 
                        out var isFinalParagraph, 
                        out var offset,
                        out int sharedRandom);

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
                        string translation;
                        if (words.Length > 0)
                        { 
                            var translateResult = await translateService.TranslateWordsAsync(words);
                            translation = translateResult.Words;
                        }
                        else
                        {
                            translation = words;
                        }

                        if (isFinalParagraph)
                        {
                            UpdateFinalParagraph(translation, sharedRandom);
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
                        File.AppendAllText(string.Format($"{TranslateService.logpath}{otherLanguageFilenameFormatString}", languageCode), $"{saveWords}\n\n");
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
            if (!InterProcessMessageStreamer.DecodeLayoutMessage(message, out var count, out var index))
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
            broadcastHelper.BroadcastIncremental(words, translation);
            WordWrapWrite(translation, previewColour);
        }

        private static void UpdateAbsolute(string words, string translation)
        {
            broadcastHelper.BroadcastAbsolute(words, translation);

            SetupConsoleToOverwriteWords();

            WordWrapWrite(translation, previewColour);
        }

        private static void UpdateFinalParagraph(string translation, int sharedRandom)
        {
            var colour = funkyColours.MakeFunkyColour(baseColour, sharedRandom);
            broadcastHelper.BroadcastFinalParagraph(translation, colour);

            SetupConsoleToOverwriteWords();

            WordWrapWrite(translation, colour);
            Console.WriteLine("".PadLeft(Console.WindowWidth - Console.CursorLeft));

            Console.WriteLine("".PadLeft(Console.WindowWidth));
            nextFreeLine = Console.CursorTop;
        }

        private static void WordWrapWrite(string translation, Color color)
        {
            foreach (var word in translation.Split())
            {
                var left = Console.CursorLeft;
                if (word.Length + left > Console.WindowWidth - 5)
                    Console.WriteLine("".PadLeft(Console.WindowWidth - left));

                if (Console.CursorLeft > 0 && word.Length > 0 && !word.StartsWith(" "))
                    Console.Write(" ");

                Console.Write( word.Pastel(color));

                // If the 'Pastel' extension method is not available, you need to install the Pastel NuGet package.
                // Run the following command in the NuGet Package Manager Console:
                // Install-Package Pastel -Version 3.0.0

                // Alternatively, if you are using .NET CLI, run:
                // dotnet add package Pastel --version 3.0.0
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