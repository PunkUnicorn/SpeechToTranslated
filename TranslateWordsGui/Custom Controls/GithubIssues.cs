using System.Text;

namespace TranslateWordsGui.GraphicsUtils;


/// <summary>
/// Some person sharing their workaround:
/// https://github.com/dotnet/core/issues/3584
/// </summary>
public static class GraphicsUtils
{
    public static List<string> WrapLines(this Graphics graphics, string text, Font font, int width)
    {
        int usedSpace;
        int spaceLeft = width;
        int wordWidth;
        int spaceWidth = (int)graphics.MeasureString(" ", font).Width;

        string[] word = text.Split(" ");
        StringBuilder line = new StringBuilder();
        List<string> wrappedLines = new List<string>();

        for (int i = 0; i < word.Length; ++i)
        {
            wordWidth = (int)graphics.MeasureString(word[i], font).Width;
            usedSpace = (int)graphics.MeasureString(line.ToString(), font).Width;
            if (usedSpace + wordWidth + spaceWidth < spaceLeft)
            {
                line.Append(word[i]);
                usedSpace += wordWidth;
                if (i != (word.Length - 1))
                {
                    line.Append(" ");
                    usedSpace += spaceWidth;
                }


            }
            else
            {

                wrappedLines.Add(line.ToString());

                line.Clear();
                usedSpace = 0;


                line.Append(word[i]);
                usedSpace += wordWidth;
                if (i != word.Length - 1)
                {
                    line.Append(" ");
                    usedSpace += spaceWidth;
                }
            }

            if (i == word.Length - 1)
            {
                wrappedLines.Add(line.ToString());
            }
        }

        wrappedLines.Reverse();
        return wrappedLines;
    }
}