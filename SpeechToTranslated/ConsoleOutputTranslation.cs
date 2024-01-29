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
            var otherLanguageTextOutput = wantEnglish ? translatedWords.Replace("\n\n", "") : translatedWords;

            var width = Console.WindowWidth;
            if (wantEnglish) 
            { 
                var column = width / 2;
                var otherLanguage = 5;
                var english = width - column;

                var formatString = $"{{0,{-otherLanguage}}}{{1,{english}}}\n";
                Console.Write(formatString, otherLanguageTextOutput.PadRight(width / 2, ' '), englishWords.PadRight(width / 2, ' '));
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
