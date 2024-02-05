//using ChurchSpeechToTranslated;
//using Microsoft.CognitiveServices.Speech;
//using Microsoft.CognitiveServices.Speech.Audio;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace SpeechToTranslated.SpeechRecognition
//{
//    public class MicrosoftSpeechToText : IDisposable, ISpeechToText
//    {
//        public event WordsEventArgs.WordsReadyHandler WordsReady;
//        public event WordsEventArgs.SentanceReadyHandler SentanceReady;

//        private readonly IConfiguration config;
//        private readonly List<string> toTranslateCandidate = new List<string>();
//        private AudioConfig audioConfig;
//        private SpeechRecognizer speechRecognizer;
//        private readonly TaskCompletionSource<int> stopRecognition = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
//        private readonly bool speechToTextLog;
//        private ulong knownOffset;
//        private string knownSentance = "";
//        private string clipped = "";

//        public MicrosoftSpeechToText(IConfiguration config)
//        {
//            this.config = config;
//            speechToTextLog = bool.Parse(config["speechtotext.log"]);
//        }

//        private SpeechConfig MakeSpeechConfig()
//        {
//            var speechKey = config["speechtotext.microsoft.key"];
//            var speechRegion = config["speechtotext.microsoft.region"];

//            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
//            speechConfig.SpeechRecognitionLanguage = config["speechtotext.input.language"] ?? "en-GB";
//            speechConfig.SetProfanity(ProfanityOption.Raw);
//            speechConfig.OutputFormat = OutputFormat.Detailed;

//            return speechConfig;
//        }

//        public async Task RunSpeechToTextForeverAsync()
//        {
//            speechRecognizer = new SpeechRecognizer(MakeSpeechConfig(), AudioConfig.FromDefaultMicrophoneInput());
//            speechRecognizer.Recognizing += SpeechRecognizer_Recognizing;
//            speechRecognizer.SpeechEndDetected += SpeechRecognizer_SpeechEndDetected;
//            speechRecognizer.Recognized += SpeechRecognizer_Recognized;

//            await speechRecognizer.StartContinuousRecognitionAsync();
//            Task.WaitAny(new[] { stopRecognition.Task });
//            await speechRecognizer.StopContinuousRecognitionAsync();
//        }

//        private void SpeechRecognizer_Recognized(object sender, SpeechRecognitionEventArgs e)
//        {
//            SentanceReady?.Invoke(new WordsEventArgs(e.Result.Text.Split()));
//            Recognizing(e.Result.Text.Replace("?", "").Replace(",", "").Replace(".", "").Replace("!", "").ToLower(), nameof(SpeechRecognizer_Recognized));
//            Recognized();
//            if (speechToTextLog)
//            {
//                var dump = new { Type = nameof(SpeechRecognizer_Recognized), Date = DateTime.Now.ToLongDateString(), Time = DateTime.Now.ToLongTimeString(), e.Result };
//                File.AppendAllText($"{Application.logpath}SpeechToText.log", $",{JsonConvert.SerializeObject(dump, Formatting.Indented)}");
//            }

//        }

//        private void Recognized()
//        {
//            knownSentance = "";
//            toTranslateCandidate.Add("\n\n");
//            DumpAndFlushTranslateCandidates(toTranslateCandidate);

//        }

//        private void SpeechRecognizer_SpeechEndDetected(object sender, RecognitionEventArgs e)
//        {
//            Recognized();
//        }

//        private void SpeechRecognizer_Recognizing(object sender, SpeechRecognitionEventArgs e)
//        {
//            string clip="";
//            if (knownOffset != e.Offset)
//            {
//                if (clipped.Length > 0)
//                {
//                    clip = $" {clipped}\n";
//                    clipped = "";
//                }
//            }

//            // don't bother with the three newest words, they get corrected shortly
//            var bits = e.Result.Text.Split();
//            clip = clip+string.Join(' ', bits.Take(Math.Max(0, bits.Length-3)));
//            clipped = string.Join(' ', bits.Skip(Math.Max(0, bits.Length - 3)));

//            knownOffset = e.Offset;

//            Recognizing(clip, nameof(SpeechRecognizer_Recognizing));

//            if (speechToTextLog)
//            {
//                var dump = new { Type=nameof(SpeechRecognizer_Recognizing), Date = DateTime.Now.ToLongDateString(), Time = DateTime.Now.ToLongTimeString(), e.Result };
//                File.AppendAllText($"{Application.logpath}SpeechToText.log", $",{JsonConvert.SerializeObject(dump, Formatting.Indented)}");
//            }
//        }

//        private void Recognizing(string resultText, string logsource)
//        {
//            // Use word count

//            var @new = knownSentance == ""
//                ? resultText
//                : $"{string.Join(' ', resultText.Split().Skip(knownSentance.Split().Length))}";//.Substring(knownSentance.Length)            
//               // : "";
//            //var @new = knownSentance.Length <= resultText.Length
//            //    ? resultText.Substring(knownSentance.Length)
//            //    : "";

//            if (@new.Length == 0)
//                return;

//            @new = $" {@new}";

//            var isPartialWord = knownSentance.Length == 0;

//            if (speechToTextLog)
//                File.AppendAllText($"{Application.logpath}SpeechToText.log", $",{JsonConvert.SerializeObject(new { logsource, resultText, knownSentance = knownSentance.ToString(), @new, isPartialWord, clipped }, Formatting.Indented)}");

//            knownSentance = resultText;
//            //knownSentance.Append(@new);

//            if (isPartialWord)
//            {
//                if (toTranslateCandidate.Any())
//                {
//                    @new = $"{toTranslateCandidate.Last()}{@new}";
//                    toTranslateCandidate.RemoveAt(toTranslateCandidate.Count - 1);
//                }
//                toTranslateCandidate.Add(@new);
//            }
//            else
//            {
//                // Dump and flush toTranslateCandidate because we know this is a full word, not a partial word to be continued shortly
//                DumpAndFlushTranslateCandidates(toTranslateCandidate);

//                toTranslateCandidate.Add(@new);
//            }
//        }

//        private void DumpAndFlushTranslateCandidates(List<string> toTranslateCandidate)
//        {
//            //var words = new List<string>();
//            //foreach (var word in toTranslateCandidate)
//            //    words.Add(word);

//            WordsReady?.Invoke(new WordsEventArgs(toTranslateCandidate));
//            toTranslateCandidate.Clear();
//        }

//        public void Dispose()
//        {
//            audioConfig.Dispose();
//            speechRecognizer.Dispose();
//        }
//    }
//}
