﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using static System.Drawing.Color;

namespace SpeechToTranslatedCommon
{
    public class FunkyColours
    {
        private int colourAdd = 0;
        private Color previous = Color.Empty;

        public Color MakeFunkyColour(Color @base)
        {
            colourAdd += 50;
            if (colourAdd > 200)
                colourAdd = 0;
            var random = new Random();
            byte[] bytes = BitConverter.GetBytes(@base.ToArgb());
            byte aVal = bytes[0];
            byte rVal = bytes[1];
            byte gVal = bytes[2];
            byte bVal = bytes[3];
            var wildcardR = random.Next(3) - 1;
            var wildcardG = random.Next(3) - 1;
            var wildcardB = random.Next(3) - 1;
            var r = Math.Max(50, Math.Min(255, wildcardR * colourAdd + rVal + random.Next(127) - 65));
            var g = Math.Max(50, Math.Min(255, wildcardG * colourAdd + gVal + random.Next(127) - 65));
            var b = Math.Max(50, Math.Min(255, wildcardB * colourAdd + bVal + random.Next(127) - 65));

            var sum = r + g + b;
            var list = new[] {r, g, b};
            while ((list.Count(n => n>=80) < 2) || list.Count(n => n>=110) == 0)
            {
                r = Math.Max(50, Math.Min(255, r + random.Next(100)));
                g = Math.Max(50, Math.Min(255, g + random.Next(100)));
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
    }
}