namespace SpeechToTranslated.SpeechRecognition
{
    public class WordsEventArgs
    {
        public WordsEventArgs(bool isAddTo, ulong offset, string words)
        {
            this.words = words;
            Offset = offset;
            IsAddTo = isAddTo;
        }

        private string words;
        public string Words => words;

        public bool IsAddTo { get; private set; }

        public ulong Offset { get; private set; }

        public delegate void WordsReadyHandler(WordsEventArgs args);

        public delegate void SentanceReadyHandler(WordsEventArgs args);
    }

    public interface IRecogniseSpeech
    {
        Task RunSpeechToTextForeverAsync();
        Task RestartSpeechToTextAsync();
        Task RunSpeechToTextAsync();

        event WordsEventArgs.WordsReadyHandler WordsReady;
        event WordsEventArgs.SentanceReadyHandler SentanceReady;
    }
}