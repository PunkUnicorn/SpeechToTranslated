using Microsoft.Extensions.Configuration;
using SpeechToTranslated;
using SpeechToTranslatedCommon;
using SpeechToTranslatorCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchSpeechToTranslator
{
    public class Application
    {
        private readonly IConfiguration config = ConfigurationLoader.Load(); 

        private readonly SpeechToText speechToText;

        private static string GetTicks() => new String(DateTime.Now.Ticks.ToString().Reverse().ToArray()).Substring(0, 5);
        private readonly string englishFilename = $"{DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-')}-{GetTicks()}_en.txt";
        private readonly SwearingFilter swearingFilter = new SwearingFilter();
        private const string version = "0.0.0.8";
        private IOutputStuff outputter;
        private readonly List<TranslationSubProcess> translationSubProcesses = new List<TranslationSubProcess>();
        public const string logpath = ".\\Logs\\";

        public Application(string[] outputLanguages, bool wantEnglishShownWithTheTranslation)
        {
            speechToText = new SpeechToText(config);
            speechToText.WordsReady += SpeechToText_WordsReady;

            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine(File.ReadAllText("OpeningPicture.txt"));
            Console.WriteLine($"Church Translator, version {version}\nConfiguration: {config["keys_description"]}\nTranslation language: {string.Join(",", outputLanguages)}\n\nListening...");

            if (!Directory.Exists(logpath))
                Directory.CreateDirectory(logpath);

            foreach (var language in outputLanguages)
                translationSubProcesses.Add(new TranslationSubProcess(language, wantEnglishShownWithTheTranslation));

            outputter = new ConsoleOutput(false);
        }

        public async Task RunAsync() => await speechToText.RunSpeechToTextForeverAsync();

        private void SpeechToText_WordsReady(WordsEventArgs args)
        {
            var words = string.Join("", args.Words);

            if (!EvenWorthBothering(words))
                return;

            try
            {
                Consicrate(ref words);

                outputter.OutputFlow(words);

                foreach (var subTranslator in translationSubProcesses)
                    subTranslator.TranslateWords(words);

                File.AppendAllText($"{logpath}{englishFilename}", words);
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

        private bool EvenWorthBothering(string words)
        {
            if (words.Length == 0)
                return false;

            if (words == "\n\n")
            {
                outputter.OutputLineBreak();
                foreach (var subTranslator in translationSubProcesses)
                    subTranslator.OutputLineBreak();

                return false;
            }

            return true;
        }

        private void Consicrate(ref string words)
        {
            foreach (var word in words.Split())
            {
                SmiteProfanity(ref words, word);
                RestoreDevotionalUtterance(ref words, word);
                CapitaliseHolyWords(ref words, word);
            }
        }

        private static void RestoreDevotionalUtterance(ref string words, string word)
        {
            /* And they were filled with the Holy Ghost, and began to speak with other tongues, as the Spirit gave them utterance
             * — Acts 2:4.
             */

            // Speech to text makes speaking in tounges come out as blah blah blah. Technically this makes sense... but is too irreverent, so tweak this.
            if (word.StartsWith(" blah") || word.StartsWith("blah"))
                if (words.IndexOf("blah") > -1)
                    words = words.Replace("blah", "(utterance)");
        }

        private void SmiteProfanity(ref string words, string word)
        {
            // Remove accidental swear words.
            // Typically caused by a speech to text mis-hear, but since this is primarily for Church this is particularly unfortunate lol. Smite them.
            if (swearingFilter.IsSweary(word))
                words = words.Replace(word, " ".PadLeft(word.Length, '*'));
        }

        private void CapitaliseHolyWords(ref string words, string word)
        {
            if (word.StartsWith("jesus") || word.StartsWith(" jesus"))
                if (words.IndexOf("jesus") > -1)
                    words = words.Replace("jesus", "Jesus");

            if (word.StartsWith("christ") || word.StartsWith(" christ"))
                if (words.IndexOf("christ") > -1)
                    words = words.Replace("christ", "Christ");

            if (word.StartsWith("god") || word.StartsWith(" god"))
                if (words.IndexOf("god") > -1)
                    words = words.Replace("god", "God");

            if (word.StartsWith("holy") || word.StartsWith(" holy"))
                if (words.IndexOf("holy") > -1)
                    words = words.Replace("holy", "Holy");
        }
        
        public void OnProcessExit(object sender, EventArgs e)
        {
            foreach (var proc in translationSubProcesses)
                proc.Kill();
        }
    }
}
