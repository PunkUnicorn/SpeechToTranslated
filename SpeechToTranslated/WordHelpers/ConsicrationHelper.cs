using SpeechToTranslatedCommon.WordHelpers;
using System.Linq;

namespace SpeechToTranslated.WordHelpers
{
    public class ConsicrationHelper
    {
        private SwearingFilter swearingFilter = new SwearingFilter();

        public void Consicrate(ref string words)
        {
            foreach (var word in words.Replace('-', ' ').Split().Select(w => w.Trim(new[] { '.', '!', '?', '\'', ',', ';', ':' })))
            {
                SmiteProfanity(ref words, word);
                RestoreDevotionalUtterance(ref words, word);
                CapitaliseHolyWords(ref words, word);
            }
        }

        private static void RestoreDevotionalUtterance(ref string words, string word)
        {
            /* And they were filled with the Holy Ghost, and began to speak with other tongues, as the Spirit gave them utterance
             * — Acts 2:4.
             */

            // Speech to text makes speaking in tounges come out as "blah blah blah". Technically this makes sense... but is too irreverent, so tweak this.
            if (word == "blah")
                if (words.IndexOf("blah") > -1)
                    words = words.Replace("blah", "(utterance)");
        }

        private void SmiteProfanity(ref string words, string word)
        {
            // Remove accidental swear words.
            // Typically caused by a speech to text mis-hear, but since this is primarily for Church this is particularly unfortunate lol. Smite them.
            if (swearingFilter.IsSweary(word))
                words = words.Replace(word, " ");// Blanking out with stars makes it worse... " ".PadLeft(word.Length, '*'));
        }

        private void CapitaliseHolyWords(ref string words, string word)
        {
            const string jesus = "jesus";
            if (word == jesus)
                if (words.IndexOf(jesus) > -1)
                    words = words.Replace(jesus, "Jesus");

            const string christ = "christ";
            if (word.StartsWith(christ))
                if (words.IndexOf(christ) > -1)
                    words = words.Replace(christ, "Christ");

            const string god = "god";
            if (word.StartsWith(god))
                if (words.IndexOf(god) > -1)
                    words = words.Replace(god, "God");

            const string holy = "holy";
            if (word == holy)
                if (words.IndexOf(holy) > -1)
                    words = words.Replace(holy, "Holy");

            const string holiness = "holiness";
            if (word == holiness)
                if (words.IndexOf(holiness) > -1)
                    words = words.Replace(holiness, "Holiness");

            const string bible = "bible";
            if (word == bible)
                if (words.IndexOf(bible) > -1)
                    words = words.Replace(bible, "Bible");
        }
    }
}
