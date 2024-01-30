namespace SpeechToTranslatedCommon
{
    public interface IOutputStuff
    {
        void OutputColumns(string englishWords, string translatedWords);

        void OutputFlow(string translatedWords);

        void OutputLineBreak();
    }
}
