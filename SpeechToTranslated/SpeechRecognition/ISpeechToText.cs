//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace SpeechToTranslated.SpeechRecognition
//{
//    public class WordsEventArgs
//    {
//        public WordsEventArgs(IEnumerable<string> words) => this.words.AddRange(words);
//        private List<string> words = new List<string>();
//        public IEnumerable<string> Words => words;

//        public delegate void WordsReadyHandler(WordsEventArgs args);

//        public delegate void SentanceReadyHandler(WordsEventArgs args);
//    }

//    public interface ISpeechToText
//    {
//        Task RunSpeechToTextForeverAsync();
//        event WordsEventArgs.WordsReadyHandler WordsReady;
//        event WordsEventArgs.SentanceReadyHandler SentanceReady;
//    }
//}