using ChurchSpeechToTranslator;
using DeepL;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchSpeechToTranslateor
{
    public class Application
    {
        private readonly IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true)
            .Build();

        private readonly SpeechToText speechToText;
        //private readonly SpellingHelper spellingHelper = new SpellingHelper();
        private readonly Translator translator;

        private static string GetTicks() => new String(DateTime.Now.Ticks.ToString().Reverse().ToArray()).Substring(0, 5);
        private readonly string englishFilename = $"{DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-')}-{GetTicks()}_en.txt";
        private readonly string bulgarianFilenameFormatString = $"{DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-')}-{GetTicks()}_{{0}}.txt";
        private readonly string outputLanguage;

        public Application(string outputLanguage)
        {
            speechToText = new SpeechToText(config);
            speechToText.WordsReady += SpeechToText_WordsReady;

            translator = new Translator(config["deepl.key"]);
            Console.OutputEncoding = Encoding.UTF8;

            this.outputLanguage = outputLanguage;
        }

        public async Task RunAsync() => await speechToText.RunSpeechToTextForever();

        private void SpeechToText_WordsReady(SpeechToText.WordsEventArgs args)
        {
            var words = string.Join("", args.Words);
            if (words.Length == 0)
                return;

            if (words == "\n\n")
            {
                Console.WriteLine();
                return;
            }

            try
            {
                var result = translator.TranslateTextAsync(
                    words,
                    LanguageCode.English,
                    outputLanguage).Result;

                var bulgarianTextNoLinefeed = result.Text.Replace("\n\n", "");

                var width = Console.WindowWidth;
                var column = width / 5;
                var bulgarian = 5;
                var english = bulgarian + column;
                var formatString = $"{{0,{-bulgarian}}}{{1,{-english}}}";
                Console.WriteLine(formatString, bulgarianTextNoLinefeed.PadRight(column, ' '), words.PadRight(column, ' '));

                File.AppendAllText(englishFilename, words);
                File.AppendAllText(string.Format(bulgarianFilenameFormatString, outputLanguage), result.Text);
            }
            catch (Exception e)
            {
                var fg = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    Console.ForegroundColor = fg;
                }
            }
        }
    }
}
