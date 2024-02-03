namespace SpeechToTranslatedCommon
{
    public interface IOutputStuff
    {
        void OutputColumns(string column1, string column2);

        void OutputFlow(string words);
        void OutputGreenFlow(string words);
        void OutputLineBreak();
    }
}
