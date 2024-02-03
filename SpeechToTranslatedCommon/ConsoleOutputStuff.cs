using System;
using static System.Net.Mime.MediaTypeNames;

namespace SpeechToTranslatedCommon
{
    public class ConsoleOutputStuff : IOutputStuff
    {
        private readonly bool wantEnglish;
        private int paragraphBreakIndex;

        public ConsoleOutputStuff(bool wantEnglish)
        {
            this.wantEnglish = wantEnglish;
        }

        public void OutputLineBreak()
        {
            Console.WriteLine();
        }

        public void OutputGreenFlow(string translatedWords)
        {
            var fg = Console.ForegroundColor;
            try 
            {
                Console.ForegroundColor = ConsoleColor.Green;

                //https://stackoverflow.com/questions/20534318/make-console-writeline-wrap-words-instead-of-letters
                var words = translatedWords.Split(' ');
                var lines = words.Skip(1).Aggregate(words.Take(1).ToList(), (l, w) =>
                {
                    if (l.Last().Length + w.Length >= Console.WindowWidth)
                        l.Add(w);
                    else
                        l[l.Count - 1] += " " + w;
                    return l;
                });
                OutputEverything(false, "", string.Join('\n', lines));
            }
            finally
            {
                Console.ForegroundColor = fg;
            }
        }

        public void OutputFlow(string translatedWords) => OutputEverything(false, "", translatedWords);

        public void OutputColumns(string column1, string column2) => OutputEverything(wantEnglish, column1, column2);

        private void OutputEverything(bool wantColumns, string column1, string column2)
        {
            var column2Use = wantColumns ? column2.Replace("\n\n", "") : column2;

            var width = Console.WindowWidth;
            if (wantColumns) 
            {
                paragraphBreakIndex = 0;
                var column = width / 2;
                var otherLanguage = 5;
                var english = width - column;

                var formatString = $"{{0,{-otherLanguage}}}{{1,{english}}}\n";
                Console.Write(formatString, column2Use.PadRight(width / 2, ' '), column1.PadRight(width / 2, ' '));
            }
            else
            { 
                paragraphBreakIndex += column2Use.Length;
                if (paragraphBreakIndex > width-25)
                { 
                    Console.WriteLine();
                    paragraphBreakIndex = 0;
                }
                var formatString = $"{{0}}";
                Console.Write(formatString, column2Use);
            }
        }
    }
}
