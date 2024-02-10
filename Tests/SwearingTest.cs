using SpeechToTranslatedCommon.WordHelpers;

namespace Tests
{
    [TestClass]
    public class SwearingTest
    {
        [TestMethod]
        public void CatchesSinglePurposeSwearing1()
        {
            var sf = new SwearingFilter();

            var actual = sf.IsSweary("Fuck");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void AllowsTheWordSnatch()
        {
            var sf = new SwearingFilter();

            var actual = sf.IsSweary("Snatch");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void AllowsTheWordPuppies()
        {
            var sf = new SwearingFilter();

            var actual = sf.IsSweary("Puppies");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void AllowsTheWordCock()
        {
            var sf = new SwearingFilter();

            var actual = sf.IsSweary("Cock");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void AllowsTheWordPussy()
        {
            var sf = new SwearingFilter();

            var actual = sf.IsSweary("Pussy");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void CatchesSinglePurposeSwearing2()
        {
            var sf = new SwearingFilter();

            var actual = sf.IsSweary(" Shitty");

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void CatchesSinglePurposeSwearing3()
        {
            var sf = new SwearingFilter();

            var actual = sf.IsSweary("Pissy");

            Assert.IsTrue(actual);
        }
    }
}