namespace SpeechToTranslatedCommon
{
    public class ConsoleOutputAgainstOffset : IOutputStuffAgainstOffset
    {
        //private class OffsetDetails
        //{
        //    public OffsetDetails(ulong offset, List<string> lines) { Offset = offset; Lines = lines; }
        //    public ulong Offset { get; set; }
        //    public List<string> Lines { get; set; }
        //}

        //private List<OffsetDetails> offsetDetails = new List<OffsetDetails>();
        //private KeyValuePair<ulong/*offset*/, KeyValuePair<int/*x*/, int/*y*/>> origin;
        public ConsoleOutputAgainstOffset()
        {
            //origin = MakeOrigin(0u, Console.CursorLeft, 0);
        }

        //private KeyValuePair<ulong, KeyValuePair<int, int>> MakeOrigin(ulong offset, int left, int top) => new KeyValuePair<ulong, KeyValuePair<int, int>>(offset, new KeyValuePair<int, int>(left, top));

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

            return;
            //var lines = isFinalParagraph
            //    ? SplitLines(translatedWords).Where(s => s.Length!=0).ToList()
            //    : new List<string>(new [] { translatedWords });

            //var update = offsetDetails.FirstOrDefault(o => offset == o.Offset);

            //if (update != null)
            //{
            //    if (!isAddTo)
            //        update.Lines.Clear();

            //    update.Lines.AddRange(lines);
            //}
            //else
            //{
            //    offsetDetails.Add(new OffsetDetails(offset, lines));
            //}

            //if (offsetDetails.Count > 5)
            //{ 
            //    offsetDetails.Remove(offsetDetails.First());
            //}

            //if (isFinalParagraph)
            //{ 
            //    Console.CursorTop = startTop;
            //    Console.Clear();
            //}

            //foreach (var output in offsetDetails)
            //    if (isFinalParagraph)
            //        Console.WriteLine(string.Join('\n', output.Lines));
            //    else
            //        Console.Write(translatedWords);

        }

        private static List<string> SplitLines(string translatedWords)
        {
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
            return lines;
        }
    }
}
