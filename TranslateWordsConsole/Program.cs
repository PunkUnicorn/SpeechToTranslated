using Pastel;
using Spectre.Console;
using SpeechToTranslatedCommon;
using System.Globalization;
using System.IO.Pipes;
using System.Text;
using System.Text.RegularExpressions;
using TranslateWordsProcess;

namespace TranslateWordsConsole
{
    internal class Program
    {
        private static TranslatorHelper translatorHelper;
        private static string languageCode;
        private static int nextFreeLine=0;
        private static System.Drawing.Color baseColour = System.Drawing.Color.FromArgb(160, 160, 160);
        private static FunkyColours funkyColours = new FunkyColours();
        private const int topTranslationRow = 5;
        private static System.Drawing.Color previewColour = System.Drawing.Color.DarkGray;

        static async Task<int> Main(string[] args)
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

            #region Spectre.Console
            //await AnsiConsole.Progress()
            //    .StartAsync(async ctx =>
            //    {
            //        //ctx.Spinner();
            //        //ctx.Spinner = Spinner.Known.Noise;
            //        //ctx.SpinnerStyle(new Style(Color.Black));
            //        ctx.AddTask("wut", new ProgressTaskSettings() { AutoStart = true, MaxValue = 100 });
            //        // Simulate some work
            //        AnsiConsole.MarkupLine("Doing some more work...");
            //        Thread.Sleep(2000);

            //        for (int i = 0; i < 40; i++)
            //        {
            //            AnsiConsole.Write(new Text($"This and that {i}", new Style(MakeFunkyColur(c, f))).LeftJustified());
            //            AnsiConsole.WriteLine();
            //            AnsiConsole.WriteLine();
            //            Thread.Sleep(3000);
            //        }
            //        ctx.Refresh();
            //    });


            //await AnsiConsole.Live(layout)
            //    .StartAsync(async ctx =>
            //    {
            //        //var c = System.Drawing.Color.FromArgb(160, 160, 160);
            //        //var f = new FunkyColours();

            //        Text[] texts = new Text[]{ }; 
            //        for (int i = 0; i<40; i++)
            //        {

            //            Grid grid = new Grid();
            //            grid.AddColumn();
            //            //for (int x=0; x<i+1; x++)
            //                texts = texts
            //                    .Concat(new [] { 
            //                        new Text($"This and that {i}", new Style(MakeFunkyColur(c, f))).LeftJustified() })
            //                    .ToArray();


            //            layout["Left"].Update(grid.Expand());

            //            layout["Right"]["Bottom"].Update(
            //                new Panel(
            //                    new Text($"Hello {i}", new Style(Color.DarkSlateGray1))));

            //            ctx.Refresh();

            //            await Task.Delay(1000);
            //        }
            //    });
            #endregion 

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
                    var words = MessageStreamer.DecodeMessage(message, out var isIncremental, out var isFinalParagraph, out var offset);

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
                Console.Error.WriteLine(ex.Message);
                return 1;
            }

            return 0;
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

            WordWrapWrite(translation, funkyColours.MakeFunkyColour(baseColour));
            Console.WriteLine("".PadLeft(Console.WindowWidth));
            Console.WriteLine("".PadLeft(Console.WindowWidth));
            nextFreeLine = Console.CursorTop;
        }

        private static void WordWrapWrite(string translation, System.Drawing.Color color)
        {
            foreach (var word in translation.Split())
            {
                var left = Console.CursorLeft;
                if (word.Length + left > Console.WindowWidth - 5)
                    Console.WriteLine("".PadLeft(Console.WindowWidth - left));

                if (Console.CursorLeft > 0 && word.Length > 0 && !word.StartsWith(" "))
                    Console.Write(" ");

                Console.Write(word.Pastel(color));
            }
        }

        private static void SetupConsoleToOverwriteWords()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = nextFreeLine;
        }
    }
}