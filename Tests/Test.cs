using SpeechToTranslated;

namespace Tests
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void CatchesSwearing1()
        {
            var swearing = "Fuck";
            
            var sf = new SwearingFilter();

            var actual = sf.IsSweary(swearing);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void AllowsTheWordSnatch()
        {
            var swearing = "Snatch";

            var sf = new SwearingFilter();

            var actual = sf.IsSweary(swearing);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void AllowsTheWordPuppies()
        {
            var swearing = "They were like two puppies fighting to get out.";

            var sf = new SwearingFilter();

            var actual = sf.IsSweary(swearing);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void AllowsTheWordCock()
        {
            var swearing = "Cock";

            var sf = new SwearingFilter();

            var actual = sf.IsSweary(swearing);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void CatchesSwearing2()
        {
            var swearing = "Shitty";

            var sf = new SwearingFilter();

            var actual = sf.IsSweary(swearing);

            Assert.IsTrue(actual);
        }
    }
}