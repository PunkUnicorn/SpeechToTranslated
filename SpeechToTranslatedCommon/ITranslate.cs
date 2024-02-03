namespace SpeechToTranslatedCommon
{
    public interface ITranslate
    {
        void OutputLineBreak();
        void TranslateWords(string englishWords);
        void TranslateGreenWords(string words);
    }
}
