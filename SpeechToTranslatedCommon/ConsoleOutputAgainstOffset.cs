namespace SpeechToTranslatedCommon
{
    public class ConsoleOutputAgainstOffset : IOutputStuffAgainstOffset
    {
        public void OutputLineBreak()
        {
            Console.WriteLine();
        }

        public void OutputFlow(bool isFinalParagraph, bool isAddTo, ulong offset, string translatedWords)
        {
            if (!isAddTo)
                Console.WriteLine();

            Console.Write(translatedWords);

            if (isFinalParagraph)
                Console.WriteLine();
        }

        /// <summary>
        /// https://stackoverflow.com/questions/20534318/make-console-writeline-wrap-words-instead-of-letters
        /// </summary>
        private static List<string> SplitLines(string translatedWords)
        {
            var words = translatedWords.Split(' ');
            var lines = words.Skip(1).Aggregate(words.Take(1).ToList(), (l, w) =>
            {
                if (l.Last().Length + w.Length >= Console.WindowWidth)
                    l.Add(w);
                else
                    l[l.Count - 1] += " " + w;
                return l;
            });
            return lines;
        }
    }
}
