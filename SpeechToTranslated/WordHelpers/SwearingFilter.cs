using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace SpeechToTranslatedCommon.WordHelpers
{
    public class SwearingFilter
    {
        public string[] FilterWords { get; }

        public SwearingFilter()
        {
            // https://www.indy100.com/viral/british-swear-word-ranked-offensiveness-2659905092
            FilterWords = File.Exists("Swearwords.txt")
                ? File.ReadAllText("Swearwords.txt")
                    .Split(new[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(word => word.Trim().ToLower())
                    .ToArray()
                : throw new InvalidProgramException("No swear filter detected.");

            FilterWords = FilterWords.Where(w => w.Length > 0).ToArray();
            FilterWords = FilterWords.Union(FilterWords).ToArray(); /*de-dup hack*/
            Array.Sort(FilterWords);
        }

        public bool IsSweary(string swearyCheck) => swearyCheck.Length > 0
            ? FilterWords.Contains(swearyCheck.Trim().ToLower())
            : false;
    }
}
