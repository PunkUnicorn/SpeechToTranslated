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
            const string swearwordFilterFilename = "Swearwords.txt";
            // https://www.indy100.com/viral/british-swear-word-ranked-offensiveness-2659905092
            FilterWords = File.Exists(swearwordFilterFilename)
                ? File.ReadAllText(swearwordFilterFilename)
                    .Split(new[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(word => word.Trim().ToLower())
                    .ToArray()
                : throw new InvalidProgramException($"No swear filter file: '{Path.Combine(Directory.GetCurrentDirectory(), swearwordFilterFilename)}'.");

            FilterWords = FilterWords.Where(w => w.Length > 0).ToArray();
            FilterWords = FilterWords.Union(FilterWords).OrderBy(ob => ob.Length).ToArray(); /*de-dup hack*/
        }

        public bool IsSweary(string swearyCheck) => swearyCheck.Length > 0
            ? FilterWords.Contains(swearyCheck.Trim().ToLower())
            : false;

    }
}
