using SpeechToTranslated.WordHelpers;

namespace Tests
{
    [TestClass]
    public class ConsicrationTests
    {
        [TestMethod]
        public void ConsicratesHolyWords()
        {
            var c = new ConsicrationHelper();

            var words = "god, jesus, the holy spirit and the bible are holy holiness.";
            c.Consicrate(ref words);

            Assert.AreEqual("God, Jesus, the Holy spirit and the Bible are Holy Holiness.", words);
        }

        [TestMethod]
        public void SmitesFoulLanguage()
        {
            var c = new ConsicrationHelper();

            var words = "fuck fuckity fuck, shitty bollocks";
            c.Consicrate(ref words);

            Assert.AreEqual("   ity  ,    ", words);
        }

        [TestMethod]
        public void RestoresDevotionalUtterances()
        {
            var c = new ConsicrationHelper();

            var words = "blah blah blah.";
            c.Consicrate(ref words);

            Assert.AreEqual("(utterance) (utterance) (utterance).", words);
        }
    }
}
