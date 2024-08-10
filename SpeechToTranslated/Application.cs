using Microsoft.Extensions.Configuration;
using SpeechToTranslated;
using SpeechToTranslated.SpeechRecognition;
using SpeechToTranslated.WordHelpers;
using SpeechToTranslatedCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChurchSpeechToTranslated
{
    public class Application
    {
        private readonly IConfiguration config = ConfigurationLoader.Load();

        private readonly IRecogniseSpeech speechToText;

        private static string GetTicks() => new String(DateTime.Now.Ticks.ToString().Reverse().ToArray()).Substring(0, 5);
        private readonly string englishFilename = $"{DateTime.Now.ToShortDateString().Replace('\\', '-').Replace('/', '-')}-{GetTicks()}_en.txt";
        private readonly ConsicrationHelper consicrationHelper = new ConsicrationHelper();
        private const string version = "0.0.0.9";
        private readonly IOutputStuffAgainstOffset outputter;
        private readonly List<TranslationSubProcess> translationSubProcesses = new List<TranslationSubProcess>();
        private Stopwatch Inactivity = new Stopwatch();
        public const string logpath = ".\\Logs\\";

        public Application(string[] outputLanguages, bool forceConsole)
        {
            speechToText = new MicrosoftSpeechToText(config); //new NAudioToGoogleSpeechToText(config); //
            speechToText.WordsReady += SpeechToText_WordsReady;
            speechToText.SentanceReady += SpeechToText_SentanceReady;

            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine(File.ReadAllText("OpeningPicture.txt"));
            Console.WriteLine($"Church Translator, version {version}\nConfiguration: {config["configuration_description"]}\nTranslation language: {string.Join(",", outputLanguages)}\n\nListening...");

            Console.WriteLine("\nWarning! Mishears And Then Translates Into Several Other Languages (MATTISOL)");

            if (!Directory.Exists(logpath))
                Directory.CreateDirectory(logpath);

            foreach (var language in outputLanguages)
                translationSubProcesses.Add(new TranslationSubProcess(language, forceConsole));

            outputter = new ConsoleOutputAgainstOffset();
            Inactivity.Start();
        }

        private void SpeechToText_SentanceReady(WordsEventArgs args)
        {
            var words = args.Words;
            consicrationHelper.Consicrate(ref words);

            if (words.Length > 0) 
                Inactivity.Restart();

            OutputWords(true, false, args.Offset, words);
            File.AppendAllText($"{logpath}{englishFilename}", $"{words}\n\n");
        }

        private void SpeechToText_WordsReady(WordsEventArgs args)
        {
            var words = args.Words;
            consicrationHelper.Consicrate(ref words);

            if (!EvenWorthBothering(words))
                return;

            Inactivity.Restart();

            OutputWords(false, args.IsAddTo, args.Offset, words);
        }

        private void OutputWords(bool isFinalParagraph, bool isAddTo, ulong offset, string words)
        {
            var sharedRandom = new Random().Next(int.MinValue, int.MaxValue);
            try
            {
                var cr = isAddTo ? "" : "\n";
                outputter.OutputFlow(isFinalParagraph, isAddTo, offset, words + cr);
                foreach (var proc in translationSubProcesses)
                    proc.TranslateWords(isFinalParagraph, isAddTo, offset, words, sharedRandom);

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

        public async Task RunAsync()
        {
            var keyListener = Task.Factory.StartNew(() => 
            {
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        switch (key.Key)
                        {
                            case ConsoleKey.F5:
                                foreach (var proc in translationSubProcesses)
                                    proc.LayoutShift(translationSubProcesses.Count, translationSubProcesses.IndexOf(proc)+1);
                                break;
                            default:
                                break;
                        }
                    }
                    Thread.Sleep(0);
                }
            });

            await speechToText.RunSpeechToTextAsync();

            while (true)
            {
                const int TenSecondsMs = 5*1000;
                if (Inactivity.ElapsedMilliseconds > TenSecondsMs)
                { 
                    await speechToText.RestartSpeechToTextAsync();
                    Inactivity.Restart();
                }
                Thread.Sleep(0);
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

        public void OnProcessExit(object sender, EventArgs e)
        {
            foreach (var proc in translationSubProcesses)
                proc.Kill();
        }
    }
}
