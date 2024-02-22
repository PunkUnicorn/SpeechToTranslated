namespace SpeechToTranslatedCommon
{
    public interface ITranslate
    {
        void OutputLineBreak();
        void TranslateWords(bool isFinalParagraph, bool isAddTo, ulong offset, string englishWords, int sharedRandom);
    }
}
