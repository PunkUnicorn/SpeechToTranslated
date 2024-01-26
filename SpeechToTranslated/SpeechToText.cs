using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchSpeechToTranslator
{
    public class SpeechToText :IDisposable
    {
        public class WordsEventArgs
        {
            public WordsEventArgs(IEnumerable<string> words) => this.words.AddRange(words);
            private List<string> words = new List<string>();
            public IEnumerable<string> Words => words;
        }

        public delegate void WordsReadyHandler(WordsEventArgs args);
        public event WordsReadyHandler WordsReady;

        private readonly IConfiguration config;
        private readonly StringBuilder knownSentance = new StringBuilder();
        private readonly List<string> toTranslateCandidate = new List<string>();
        private AudioConfig audioConfig;
        private SpeechRecognizer speechRecognizer;
        private readonly TaskCompletionSource<int> stopRecognition = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);

        public SpeechToText(IConfiguration config)
        {
            this.config = config;
        }

        private SpeechConfig InternalSetup()
        {
            var speechKey = config["speechtotext.key"];
            var speechRegion = config["speechtotext.region"];

            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            speechConfig.SpeechRecognitionLanguage = "en-GB";
            speechConfig.SetProfanity(ProfanityOption.Raw);
            return speechConfig;
        }

        public async Task RunSpeechToTextForever()
        {
            var speechConfig = InternalSetup();
            audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

            speechRecognizer.Recognizing += SpeechRecognizer_Recognizing;
            speechRecognizer.SpeechEndDetected += SpeechRecognizer_SpeechEndDetected;
            speechRecognizer.SpeechStartDetected += SpeechRecognizer_SpeechStartDetected;
            speechRecognizer.Recognized += SpeechRecognizer_Recognized;

            await speechRecognizer.StartContinuousRecognitionAsync();
            Task.WaitAny(new[] { stopRecognition.Task });
            await speechRecognizer.StopContinuousRecognitionAsync();
        }

        private void SpeechRecognizer_Recognized(object sender, SpeechRecognitionEventArgs e)
        {
            //Console.WriteLine("SpeechRecognized\n");
            toTranslateCandidate.Add("\n\n");
            DumpAndFlushTranslateCandidates(toTranslateCandidate);
            knownSentance.Clear();
        }

        private void SpeechRecognizer_SpeechStartDetected(object sender, RecognitionEventArgs e)
        {
            //Console.WriteLine("SpeechStartDetected\n");
        }

        private void SpeechRecognizer_SpeechEndDetected(object sender, RecognitionEventArgs e)
        {
            //Console.WriteLine("SpeechEndDetected\n");
        }

        private void SpeechRecognizer_Recognizing(object sender, SpeechRecognitionEventArgs e)
        {
            var @new = e.Result.Text.Substring(knownSentance.Length);

            var isPartialWord = !@new.StartsWith(' ') || knownSentance.Length == 0;

            knownSentance.Append(@new);

            if (isPartialWord)
            {
                var newNew = $"{toTranslateCandidate.Last()}{@new}";
                toTranslateCandidate.RemoveAt(toTranslateCandidate.Count - 1);
                toTranslateCandidate.Add(newNew);
            }
            else
            {
                // Dump and flush toTranslateCandidate because we know this is a full word, not a partial word to be continued shortly
                DumpAndFlushTranslateCandidates(toTranslateCandidate);

                toTranslateCandidate.Add(@new);
            }
        }

        private void DumpAndFlushTranslateCandidates(List<string> toTranslateCandidate)
        {
            var words = new List<string>();
            foreach (var word in toTranslateCandidate)
                words.Add(word);

            WordsReady?.Invoke(new WordsEventArgs(words));
            toTranslateCandidate.Clear();
        }

        public void Dispose()
        {
            audioConfig.Dispose();
            speechRecognizer.Dispose();
        }
    }
}
