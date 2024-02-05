using System;
using System.Linq;

namespace ChurchSpeechToTranslated.WordHelpers
{
    public class SpellingHelper
    {
        private SymSpell symSpell;

        public SpellingHelper()
        {
            //create object
            int initialCapacity = 82765;
            int maxEditDistanceDictionary = 2; //maximum edit distance per dictionary precalculation
            symSpell = new SymSpell(initialCapacity, maxEditDistanceDictionary);

            //load dictionary
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string dictionaryPath = baseDirectory + "frequency_dictionary_en_82_765.txt";
            int termIndex = 0; //column of the term in the dictionary text file
            int countIndex = 1; //column of the term frequency in the dictionary text file
            if (!symSpell.LoadDictionary(dictionaryPath, termIndex, countIndex))
                throw new InvalidOperationException("Load dictionary failed!");
        }

        public string CheckSpelling(string inputTerm)
        {
            //lookup suggestions for single-word input strings
            int maxEditDistanceLookup = 1; //max edit distance per lookup (maxEditDistanceLookup<=maxEditDistanceDictionary)
            var suggestionVerbosity = SymSpell.Verbosity.Closest; //Top, Closest, All
            var suggestions = symSpell.Lookup(inputTerm, suggestionVerbosity, maxEditDistanceLookup);

            ////display suggestions, edit distance and term frequency
            //foreach (var suggestion in suggestions)
            //{
            //    retval.Add(suggestion.term);
            //    Console.WriteLine(suggestion.term + " " + suggestion.distance.ToString() + " " + suggestion.count.ToString("N0"));
            //}
            return $" {suggestions.FirstOrDefault()?.term ?? inputTerm.Trim()}";
            //return retval;
        }
    }
}
