namespace SpeechToTranslated
{
    public interface IOutputTranslation
    {
        void OutputTranslation(string englishWords, string translatedWords);

        void OutputLineBreak();
    }
}
