using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ChatSample
{
    public class TranslationList : ConcurrentDictionary<string, string>, ITranslationList
    {
        public List<string> Translations { get => ToArray().Select(s => s.Key).ToList(); }
    }
}