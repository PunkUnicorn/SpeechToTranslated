using ChurchSpeechToTranslated;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SpeechToTranslated.SpeechRecognition
{
    public class MicrosoftSpeechToText2 : IDisposable, ISpeechToText
    {
        public event WordsEventArgs.WordsReadyHandler WordsReady;
        public event WordsEventArgs.SentanceReadyHandler SentanceReady;

        private readonly IConfiguration config;
        private readonly List<string> toTranslateCandidate = new List<string>();
        private AudioConfig audioConfig;
        private SpeechRecognizer speechRecognizer;
        private readonly TaskCompletionSource<int> stopRecognition = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly bool speechToTextLog;
        private ulong knownOffset;
        private string knownSentance = "";
        private string clipped = "";

        public MicrosoftSpeechToText2(IConfiguration config)
        {
            this.config = config;
            speechToTextLog = bool.Parse(config["speechtotext.log"]);
        }

        private SpeechConfig MakeSpeechConfig()
        {
            var speechKey = config["speechtotext.microsoft.key"];
            var speechRegion = config["speechtotext.microsoft.region"];

            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            speechConfig.SpeechRecognitionLanguage = config["speechtotext.input.language"] ?? "en-GB";
            speechConfig.SetProfanity(ProfanityOption.Raw);
            speechConfig.OutputFormat = OutputFormat.Detailed;

            return speechConfig;
        }

        public async Task RunSpeechToTextForeverAsync()
        {
            speechRecognizer = new SpeechRecognizer(MakeSpeechConfig(), AudioConfig.FromDefaultMicrophoneInput());
            speechRecognizer.Recognizing += SpeechRecognizer_Recognizing;
            speechRecognizer.Recognized += SpeechRecognizer_Recognized;

            await speechRecognizer.StartContinuousRecognitionAsync();
            Task.WaitAny(new[] { stopRecognition.Task });
            await speechRecognizer.StopContinuousRecognitionAsync();
        }

        private void SpeechRecognizer_Recognized(object sender, SpeechRecognitionEventArgs e)
        {
            SentanceReady?.Invoke(new WordsEventArgs(true, e.Offset, e.Result.Text));
            LogSpeechResult(nameof(SpeechRecognizer_Recognized), e);
        }

        private void SpeechRecognizer_Recognizing(object sender, SpeechRecognitionEventArgs e)
        {
            var isIncremental = e.Result.Text.StartsWith(knownSentance);

            string passText = isIncremental
                ? passText = e.Result.Text.Substring(knownSentance.Length)
                : passText = e.Result.Text;

            WordsReady?.Invoke(new WordsEventArgs(isIncremental, e.Offset, passText));
            LogSpeechResult(nameof(SpeechRecognizer_Recognizing), e);

            knownSentance = e.Result.Text;
        }

        private void LogSpeechResult(string source, SpeechRecognitionEventArgs e)
        {
            if (speechToTextLog)
            {
                var dump = new { e.Offset, Source = source, Date = DateTime.Now.ToLongDateString(), Time = DateTime.Now.ToLongTimeString(), e.Result };
                File.AppendAllText($"{Application.logpath}SpeechToText.log", $",{JsonConvert.SerializeObject(dump, Formatting.Indented)}");
            }
        }

        public void Dispose()
        {
            audioConfig.Dispose();
            speechRecognizer.Dispose();
        }
    }
}
