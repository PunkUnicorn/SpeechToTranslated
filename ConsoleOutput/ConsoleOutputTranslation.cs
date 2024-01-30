using System;

namespace SpeechToTranslatedCommon
{
    public class ConsoleOutput : IOutputStuff
    {
        private readonly bool wantEnglish;
        private int paragraphBreakIndex;

        public ConsoleOutput(bool wantEnglish)
        {
            this.wantEnglish = wantEnglish;
        }

        public void OutputLineBreak()
        {
            Console.WriteLine();
        }

        public void OutputFlow(string translatedWords) => OutputEverything(false, "", translatedWords);

        public void OutputColumns(string column1, string column2) => OutputEverything(wantEnglish, column1, column2);

        private void OutputEverything(bool wantColumns, string column1, string column2)
        {
            var otherLanguageTextOutput = wantColumns ? column2.Replace("\n\n", "") : column2;

            var width = Console.WindowWidth;
            if (wantColumns) 
            {
                paragraphBreakIndex = 0;
                var column = width / 2;
                var otherLanguage = 5;
                var english = width - column;

                var formatString = $"{{0,{-otherLanguage}}}{{1,{english}}}\n";
                Console.Write(formatString, otherLanguageTextOutput.PadRight(width / 2, ' '), column1.PadRight(width / 2, ' '));
            }
            else
            { 
                paragraphBreakIndex += otherLanguageTextOutput.Length;
                if (paragraphBreakIndex > width-20)
                { 
                    Console.WriteLine();
                    paragraphBreakIndex = 0;
                }
                var formatString = $"{{0}}";
                Console.Write(formatString, otherLanguageTextOutput);
            }
        }
    }
}
