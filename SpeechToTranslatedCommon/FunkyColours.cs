using System.Drawing;
using static System.Drawing.Color;
using static System.Math;

namespace SpeechToTranslatedCommon
{
    public class FunkyColours
    {
        private int colourAdd = 0;
        private Color previous = Color.Empty;
        private Random random = new Random();

        public Color MakeFunkyColour(Color @base)
        {
            colourAdd += 50;
            if (colourAdd > 200)
                colourAdd = 0;
            byte[] bytes = BitConverter.GetBytes(@base.ToArgb());
            byte aVal = bytes[0];
            byte rVal = bytes[1];
            byte gVal = bytes[2];
            byte bVal = bytes[3];
            var wildcardR = random.Next(3) - 1;
            var wildcardG = random.Next(3) - 1;
            var wildcardB = random.Next(3) - 1;
            var r = Max(50, Min(255, wildcardR * colourAdd + rVal + random.Next(127) - 65));
            var g = Max(50, Min(255, wildcardG * colourAdd + gVal + random.Next(127) - 65));
            var b = Max(50, Min(255, wildcardB * colourAdd + bVal + random.Next(127) - 65));

            var sum = r + g + b;
            var list = new[] {r, g, b};
            while ((list.Count(n => n>=80) < 2) || list.Count(n => n>=110) == 0)
            {
                r = Max(50, Min(255, r + random.Next(100)));
                g = Max(50, Min(255, g + random.Next(100)));
                sum = r + g + b;
                list = new[] { r, g, b };
            }

            if (previous == Empty)
                return previous = FromArgb(aVal, r, g, b);

            var group1 = new[] { r, g, b };
            var group2 = new[] { previous.R, previous.G, previous.B };
            var diffs = group1.Zip(group2, (one, two) => Abs(Max(one, two) - Min(one, two)));

            var tooCloseToThePreviousColour = diffs.Sum() < 70;
            if (tooCloseToThePreviousColour)
                return MakeFunkyColour(@base);

            previous = FromArgb(aVal, r, g, b);
            return previous;
        }

        public void SetSeed(int seed)
        {
            random = new Random(seed);
        }
    }
}
