using System;

namespace SpeechToTranslated
{
    public class ConsoleOutputTranslation : IOutputTranslation
    {
        private readonly bool wantEnglish;
        private int paragraphBreakIndex;

        public ConsoleOutputTranslation(bool wantEnglish)
        {
            this.wantEnglish = wantEnglish;
        }

        public void OutputLineBreak()
        {
            Console.WriteLine();
        }

        public void OutputTranslation(string englishWords, string translatedWords)
        {
            var otherLanguageTextNoLinefeed = wantEnglish ? translatedWords.Replace("\n\n", "") : translatedWords;

            var width = Console.WindowWidth;
            var column = width / 2;
            var otherLanguage = 5;
            var english = width - column;

            var formatString = wantEnglish
                ? $"{{0,{-otherLanguage}}}{{1,{english}}}\n"
                : $"{{0}}";

            paragraphBreakIndex += otherLanguageTextNoLinefeed.Length;
            if (paragraphBreakIndex > width-20)
            { 
                Console.WriteLine();
                paragraphBreakIndex = 0;
            }

            if (wantEnglish)
                Console.Write(formatString, otherLanguageTextNoLinefeed.PadRight(column, ' '), englishWords.PadRight(column, ' '));
            else
                Console.Write(formatString, otherLanguageTextNoLinefeed);
        }
    }
}
