using DeepL;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpeechToTranslated;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchSpeechToTranslator
{
    public class Application
    {
        private readonly IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true)
            .AddJsonFile($"appsettings.Church.json", optional: true)
            .Build();

        private readonly SpeechToText speechToText;
        //private readonly SpellingHelper spellingHelper = new SpellingHelper();
        private readonly Translator translator;

        private static string GetTicks() => new String(DateTime.Now.Ticks.ToString().Reverse().ToArray()).Substring(0, 5);
        private readonly string englishFilename = $"{DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-')}-{GetTicks()}_en.txt";
        private readonly string otherLanguageFilenameFormatString = $"{DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-')}-{GetTicks()}_{{0}}.txt";
        private readonly string outputLanguage;
        private readonly bool wantEnglish;
        private readonly bool deepLLog;
        private readonly SwearingFilter swearingFilter = new SwearingFilter();
        private const string version = "0.0.0.7";
        private IOutputTranslation outputter;

        public const string logpath = ".\\Logs\\";

        public Application(string outputLanguage, bool wantEnglish)
        {
            speechToText = new SpeechToText(config);
            speechToText.WordsReady += SpeechToText_WordsReady;

            translator = new Translator(config["deepl.key"]);
            Console.OutputEncoding = Encoding.UTF8;

            this.outputLanguage = outputLanguage;
            this.wantEnglish = wantEnglish;

            deepLLog = bool.Parse(config["deepl.log"]);

            Console.WriteLine(File.ReadAllText("OpeningPicture.txt"));
            Console.WriteLine($"Church Translator, version {version}\nConfiguration: {config["keys_description"]}\nTranslation language: {outputLanguage}\n\nListening...");

            if (!Directory.Exists(logpath))
                Directory.CreateDirectory(logpath);

            outputter = new ConsoleOutputTranslation(wantEnglish);
        }

        public async Task RunAsync() => await speechToText.RunSpeechToTextForever();

        private void SpeechToText_WordsReady(SpeechToText.WordsEventArgs args)
        {
            var words = string.Join("", args.Words);

            if (!EvenWorthBothering(words))
                return;

            Consicrate(words);

            try
            {
                var result = translator.TranslateTextAsync(
                    words,
                    LanguageCode.English,
                    outputLanguage).Result;

                if (deepLLog)
                {
                    var dump = new { Time = DateTime.Now.ToString(), Input = words, Result = result };
                    File.AppendAllText($"{logpath}deepl.log", JsonConvert.SerializeObject(dump, Formatting.Indented));
                }

                outputter.OutputTranslation(words, result.Text);

                if (wantEnglish)
                    File.AppendAllText($"{logpath}{englishFilename}", words);

                File.AppendAllText(string.Format($"{logpath}{otherLanguageFilenameFormatString}", outputLanguage), result.Text);
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

        private void Consicrate(string words)
        {
            foreach (var word in words.Split())
            {
                SmiteProfanity(words, word);
                RestoreDevotionalUtterance(words, word);
            }
        }

        private bool EvenWorthBothering(string words)
        {
            if (words.Length == 0)
                return false;

            if (words == "\n\n")
            {
                outputter.OutputLineBreak();
                return false;
            }

            return true;
        }

        private static void RestoreDevotionalUtterance(string words, string word)
        {
            // Speech to text makes speaking in tounges come out as blah blah blah. Technically this makes sense... but is too irreverent, so tweak this.
            if (word == " blah" || word == "blah")
                words.Replace("blah", "(utterance)");
        }

        private void SmiteProfanity(string words, string word)
        {
            // Remove accidental swear words.
            // Typically caused by a speech to text mis-hear, but since this is primarily for Church this is particularly unfortunate.
            if (swearingFilter.IsSweary(word.Trim()))
                words.Replace(word, "*".PadLeft(word.Length));
        }

        //private void OutputTranslation(string englishWords, string translatedWords)
        //{
        //    var otherLanguageTextNoLinefeed = translatedWords.Replace("\n\n", "");

        //    var width = Console.WindowWidth;
        //    var column = width / 4;
        //    var otherLanguage = column;
        //    var english = otherLanguage + column;

        //    var formatString = wantEnglish
        //        ? $"{{0,{-otherLanguage}}}{{1,{-english}}}"
        //        : $"{{0,{-otherLanguage}}}";

        //    Console.WriteLine(formatString, otherLanguageTextNoLinefeed.PadRight(column, ' '), englishWords.PadRight(column, ' '));
        //}

        //private void OutputLineBreak()
        //{
        //    Console.WriteLine();
        //}
    }
}
