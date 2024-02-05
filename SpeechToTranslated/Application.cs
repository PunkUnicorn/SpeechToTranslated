using Microsoft.Extensions.Configuration;
using SpeechToTranslated;
using SpeechToTranslated.SpeechRecognition;
using SpeechToTranslatedCommon;
using SpeechToTranslatedCommon.WordHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchSpeechToTranslated
{
    public class Application
    {
        private readonly IConfiguration config = ConfigurationLoader.Load();

        private readonly ISpeechToText speechToText;

        private static string GetTicks() => new String(DateTime.Now.Ticks.ToString().Reverse().ToArray()).Substring(0, 5);
        private readonly string englishFilename = $"{DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-')}-{GetTicks()}_en.txt";
        private readonly SwearingFilter swearingFilter = new SwearingFilter();
        private const string version = "0.0.0.8";
        private readonly IOutputStuffAgainstOffset outputter;
        private readonly List<TranslationSubProcess> translationSubProcesses = new List<TranslationSubProcess>();
        public const string logpath = ".\\Logs\\";

        public Application(string[] outputLanguages)
        {
            speechToText = new MicrosoftSpeechToText2(config); //new NAudioToGoogleSpeechToText(config); //
            speechToText.WordsReady += SpeechToText_WordsReady;
            speechToText.SentanceReady += SpeechToText_SentanceReady;

            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine(File.ReadAllText("OpeningPicture.txt"));
            Console.WriteLine($"Church Translator, version {version}\nConfiguration: {config["configuration_description"]}\nTranslation language: {string.Join(",", outputLanguages)}\n\nListening...");

            if (!Directory.Exists(logpath))
                Directory.CreateDirectory(logpath);

            foreach (var language in outputLanguages)
                translationSubProcesses.Add(new TranslationSubProcess(language));

            outputter = new ConsoleOutputAgainstOffset();
        }

        private void SpeechToText_SentanceReady(WordsEventArgs args)
        {
            var words = args.Words;
            Consicrate(ref words);

            OutputWords(true, false, args.Offset, words);
            File.AppendAllText($"{logpath}{englishFilename}", $"{words}\n\n");
        }

        private void SpeechToText_WordsReady(WordsEventArgs args)
        {
            var words = args.Words;
            Consicrate(ref words);

            if (!EvenWorthBothering(words))
                return;

            OutputWords(false, args.IsAddTo, args.Offset, words);
        }

        private void OutputWords(bool isFinalParagraph, bool isAddTo, ulong offset, string words)
        {
            try
            {
                var cr = isAddTo ? "" : "\n";
                outputter.OutputFlow(isFinalParagraph, isAddTo, offset, words + cr);
                foreach (var proc in translationSubProcesses)
                    proc.TranslateWords(isFinalParagraph, isAddTo, offset, words);

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

        public async Task RunAsync() => await speechToText.RunSpeechToTextForeverAsync();

        //private void SpeechToText_WordsReady(WordsEventArgs args)
        //{
        //    var words = string.Join("", args.Words);

        //    if (!EvenWorthBothering(words))
        //        return;

        //    try
        //    {
        //        Consicrate(ref words);

        //        outputter.OutputFlow(words);

        //        foreach (var subTranslator in translationSubProcesses)
        //            subTranslator.TranslateWords(words);

        //        File.AppendAllText($"{logpath}{englishFilename}", words);
        //    }
        //    catch (Exception e)
        //    {

        //        var fg = Console.ForegroundColor;
        //        try
        //        {
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            Console.Error.WriteLine(e.Message);
        //        }
        //        finally
        //        {
        //            Console.ForegroundColor = fg;
        //        }
        //    }
        //}

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
            if (word == "blah")
                if (words.IndexOf("blah") > -1)
                    words = words.Replace("blah", "(utterance)");
        }

        private void SmiteProfanity(ref string words, string word)
        {
            // Remove accidental swear words.
            // Typically caused by a speech to text mis-hear, but since this is primarily for Church this is particularly unfortunate lol. Smite them.
            if (swearingFilter.IsSweary(word))
                words = words.Replace(word, " ");// Blanking out with stars makes it worse... " ".PadLeft(word.Length, '*'));
        }

        private void CapitaliseHolyWords(ref string words, string word)
        {
            const string jesus = "jesus";
            if (word == jesus)
                if (words.IndexOf(jesus) > -1)
                    words = words.Replace(jesus, "Jesus");

            const string christ = "christ";
            if (word.StartsWith(christ))
                if (words.IndexOf(christ) > -1)
                    words = words.Replace(christ, "Christ");

            const string god = "god";
            if (word.StartsWith(god))
                if (words.IndexOf(god) > -1)
                    words = words.Replace(god, "God");

            const string holy = "holy";
            if (word == holy)
                if (words.IndexOf(holy) > -1)
                    words = words.Replace(holy, "Holy");

            const string holiness = "holiness";
            if (word == holiness)
                if (words.IndexOf(holiness) > -1)
                    words = words.Replace(holiness, "Holiness");
        }

        public void OnProcessExit(object sender, EventArgs e)
        {
            foreach (var proc in translationSubProcesses)
                proc.Kill();
        }
    }
}
