using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechToTranslatedCommon
{
    public class FunkyColours
    {
        private int colourAdd = 0;

        public Color MakeFunkyColour(Color @base)
        {
            colourAdd += 10;
            if (colourAdd > 100)
                colourAdd = 0;

            byte[] bytes = BitConverter.GetBytes(@base.ToArgb());
            byte aVal = bytes[0];
            byte rVal = bytes[1];
            byte gVal = bytes[2];
            byte bVal = bytes[3];
            var wildcardR = new Random().Next(3) - 1;
            var wildcardG = new Random().Next(3) - 1;
            var wildcardB = new Random().Next(3) - 1;
            var r = Math.Max(0, Math.Min(255, wildcardR * colourAdd + rVal + new Random().Next(80) - 40));
            var g = Math.Max(0, Math.Min(255, wildcardG * colourAdd + gVal + new Random().Next(80) - 40));
            var b = Math.Max(0, Math.Min(255, wildcardB * colourAdd + bVal + new Random().Next(80) - 40));
            var funkyColour = Color.FromArgb(aVal, r, g, b);
            return funkyColour;
        }
    }
}
