using Pastel;
using SpeechToTranslatedCommon;
using static System.Math;
using static System.Drawing.Color;
using static System.Console;
using System.Linq;

namespace TestColours
{
    public class Program
    {
        static void Main(string[] args)
        {
            var previous = Empty;
            var funkyColurs = new FunkyColours();
            var @base = FromArgb(160, 160, 160);

            while (true) 
            {
                var funkyColour = funkyColurs.MakeFunkyColour(@base);

                if (previous != Empty)
                {
                    var group1 = new[] { funkyColour.R, funkyColour.G, funkyColour.B };
                    var group2 = new[] { previous.R, previous.G, previous.B };
                    var diffs = group1.Zip(group2, (one, two) => Abs(Max(one, two) - Min(one, two)));

                    var tooClose = diffs.Sum() < 70;
                    if (tooClose)
                        WriteLine($"AARGHH TOO CLOSE {diffs.Sum()}".Pastel(funkyColour));
                }

                string hw = $"Hello, World! {funkyColour.R}, {funkyColour.G}, {funkyColour.B} ";
                WriteLine(hw.PadRight(WindowWidth-hw.Length,'X').Pastel(funkyColour));

                previous=funkyColour;
                Thread.Sleep(200);
            }
        }
    }
}