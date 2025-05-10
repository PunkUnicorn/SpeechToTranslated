using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ChatSample
{
    public class TranslationList : List<string>, ITranslationList
    {
        public List<string> Translations { get => this; }
    }
}