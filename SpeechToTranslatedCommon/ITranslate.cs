namespace SpeechToTranslatedCommon
{
    public interface ITranslate
    {
        void OutputLineBreak();
        void TranslateWords(bool isFinalParagraph, bool isAddTo, ulong offset, string englishWords);
        //void TranslateWords(string englishWords);
        //void TranslateGreenWords(string words);
    }
}
