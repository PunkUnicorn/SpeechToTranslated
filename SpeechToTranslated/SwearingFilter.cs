using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace SpeechToTranslated
{
    public class SwearingFilter
    {
        public string[] FilterWords { get; }

        public SwearingFilter()
        {
            // https://www.indy100.com/viral/british-swear-word-ranked-offensiveness-2659905092
            FilterWords = File.Exists("Swearwords.txt")
                ? File.ReadAllText("Swearwords.txt")
                    .Split(new [] {' ', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(word => word.Trim().ToLower())
                    .ToArray()
                : new string[] { };

            FilterWords = FilterWords.Where(w => w.Length > 0).ToArray();
            FilterWords = FilterWords.Union(FilterWords).ToArray(); /*de-dup hack*/
            Array.Sort(FilterWords);
        }

        public bool IsSweary(string swearyCheck) => swearyCheck.Length > 0
            ? FilterWords.Contains(swearyCheck.ToLower())
            : false;
    }
}
