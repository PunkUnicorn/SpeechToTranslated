namespace SpeechToTranslatedCommon
{
    public interface IOutputStuffAgainstOffset
    {
        void OutputFlow(bool isFinalParagraph, bool isAddTo, ulong offset, string words);
        void OutputLineBreak();
    }
}
